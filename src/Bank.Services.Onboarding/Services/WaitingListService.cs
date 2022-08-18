using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Exceptions;
using Bank.Core.Extensions;
using Bank.Core.Interfaces;
using Bank.Services.Onboarding.Enum;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.Models.WaitingList;
using System;
using System.Linq;
using System.Net.Mail;

namespace Bank.Services.Onboarding
{
    public class WaitingListService : IWaitingListService
    {
        private readonly OnboardingDbContext _db;
        private readonly IEmailSender _emailSender;

        public WaitingListService(OnboardingDbContext db,
                                  IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        public ServiceResponse Attend(int prospectId, int waitingListId, int statusId = (int)WaitingListStatusEnum.Completed)
        {
            var response = new ServiceResponse();

            try
            {
                _db.BeginTransaction();

                var subject = _db.CustomerWaitingLists.First(a => a.ProspectId == prospectId &&
                                                                a.WaitingListId == waitingListId);

                subject.ChangeStatus(statusId);

                var history = new CustomerWaitingListStatus
                {
                    ProspectId = prospectId,
                    StatusId = statusId
                };

                _db.CustomerWaitingListStatuses.Add(history);

                SendMailWaitingListAttended(prospectId);

                _db.Commit();
            }
            catch (Exception ex)
            {
                _db.Rollback();
                return response.AddError(ex);
            }

            return response;
        }

        public ServiceResponse CreateWaitingList(string name, int dailyCapacity, string active)
        {
            var response = new ServiceResponse();

            try
            {
                var existingWaitingList = _db.WaitingLists.FirstOrDefault(a => a.Name == name);

                if (existingWaitingList != null)
                    throw new DomainException("There is already an waiting list with this name. Please choose another name.");

                var waitingList = new WaitingList();
                waitingList.Name = name;
                waitingList.DailyCapacity = dailyCapacity;
                waitingList.Active = active;

                _db.WaitingLists.Add(waitingList);
                _db.Commit();

                response.Data = waitingList;
            }
            catch (Exception ex)
            {
                return response.AddError(ex);
            }

            return response;
        }

        public int GetEstimatePosition(int waitingListId)
        {
            var totalWaiting = _db.CustomerWaitingLists.Where(a =>  
                                                             a.WaitingListId == waitingListId &&
                                                             a.StatusId == (int)WaitingListStatusEnum.Waiting)
                         .Count();

            return totalWaiting + 1;
        }

        public DateTime GetAttendanceEstimativeDate(int prospectId, int waitingListId)
        {
            var waitingList = _db.WaitingLists.First(a => a.WaitingListId == waitingListId);
            var position = _db.CustomerWaitingLists.First(a => a.ProspectId == prospectId &&
                                                               a.WaitingListId == waitingListId &&
                                                               a.StatusId != (int)WaitingListStatusEnum.Completed);

            var capacity = waitingList.DailyCapacity;

            return DateTime.Now.AddBusinessDays((position.PositionInQueue / capacity));
        }

        public ServiceResponse GetWaitingListElements(int waitingListId, int maxQuantity = 5, int[] statuses = null)
        {
            var response = new ServiceResponse();

            try
            {
                response.Data = _db.CustomerWaitingLists.Where(a => a.WaitingListId == waitingListId &&
                                                                    (statuses == null || statuses.Contains(a.StatusId)))
                                                                    .Take(maxQuantity)
                                                                    .ToList();
            }
            catch (Exception ex)
            {
                return response.AddError(ex);
            }

            return response;
        }

        public ServiceResponse InsertOnWaitingList(int prospectId, int waitingListId)
        {
            var response = new ServiceResponse();

            try
            {
                _db.BeginTransaction();

                var estimatePosition = GetEstimatePosition(waitingListId);

                var newElementOnWaitingList = new CustomerWaitingList
                {
                    ProspectId = prospectId,
                    WaitingListId = waitingListId,
                    PositionInQueue = estimatePosition,
                    StatusId = (int)WaitingListStatusEnum.Waiting

                };

                _db.CustomerWaitingLists.Add(newElementOnWaitingList);
                
                var estimative = CalculateEstimativeDay(waitingListId, estimatePosition);
                var mailSent = SendMailWaitingListEnqueued(prospectId, estimatePosition, estimative);

                response.Data = new { EstimateDate = estimative, EstimatePosition = estimatePosition };

                _db.Commit();
            }
            catch (Exception ex)
            {
                _db.Rollback();
                return response.AddError(ex);
            }

            return response;
        }

        private DateTime CalculateEstimativeDay(int waitingListId, int position)
        {
            var waitingList = _db.WaitingLists.First(a => a.WaitingListId == waitingListId);

            var capacity = waitingList.DailyCapacity;

            return DateTime.Now.AddBusinessDays((position / capacity));
        }

        public ServiceResponse UpdateWaitingList(int waitingListId, string name, int dailyCapacity, string active)
        {
            var response = new ServiceResponse();

            try
            {
                var waitingList = _db.WaitingLists.First(a => a.WaitingListId == waitingListId);
                waitingList.Name = name;
                waitingList.DailyCapacity = dailyCapacity;
                waitingList.Active = active;

                _db.WaitingLists.Update(waitingList);
                _db.Commit();
            }
            catch (Exception ex)
            {
                return response.AddError(ex);
            }

            return response;
        }


        private bool SendMailWaitingListEnqueued(int prospectId, int queuePosition, DateTime estimative)
        {
            var prospect = _db.Prospects.First(a => a.ProspectId == prospectId);
            var template = OnboardingResources.WaitingListQueueMailBody
                            .Replace("#NAME#", prospect.Name)
                            .Replace("#POSITION#", queuePosition.ToString())
                            .Replace("#DATE#", estimative.ToShortDateString());

            MailMessage message = new MailMessage
            {
                Subject = OnboardingResources.WaitingListQueueMailSubject
            };

            message.To.Add(new MailAddress(prospect.Email, prospect.Name));
            message.Body = template;
            message.IsBodyHtml = false;

            return _emailSender.SendEmail(message);
        }

        private bool SendMailWaitingListAttended(int prospectId)
        {
            var prospect = _db.Prospects.First(a => a.ProspectId == prospectId);
            var template = OnboardingResources.WaitingListAttendanceMailBody;
            var subject = OnboardingResources.WaitingListAttendanceMailSubject;

            MailMessage message = new MailMessage
            {
                Subject = OnboardingResources.WaitingListAttendanceMailSubject
            };

            message.To.Add(new MailAddress(prospect.Email, prospect.Name));
            message.Body = template;
            message.IsBodyHtml = false;

            return _emailSender.SendEmail(message);
        }
    }
}
