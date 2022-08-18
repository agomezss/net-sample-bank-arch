using Microsoft.AspNetCore.Identity;
using Bank.Core;
using Bank.Core.Context;
using Bank.Core.Interfaces;
using Bank.Core.Models;
using Bank.Services.Authentication;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.Models;
using Bank.Services.Onboarding.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.Services.Onboarding
{
    public class ProspectService : IProspectService
    {
        private readonly AuthDbContext _authDb;
        private readonly OnboardingDbContext _db;
        private readonly INoSqlClient _noSql;
        private readonly UserManager<BankUser> _userManager;

        public ProspectService(OnboardingDbContext db,
                               INoSqlClient noSqlClient,
                               UserManager<BankUser> userManager)
        {
            _db = db;
            _noSql = noSqlClient;
            _userManager = userManager;
        }

        public ProspectViewModel GetById(int prospectId, string phoneNumber)
        {
            var data = _db.Prospects.FirstOrDefault(a => a.ProspectId == prospectId && a.PhoneNumber == phoneNumber);

            if (data == null) return null;

            var model = new ProspectViewModel
            {
                ProspectId = data.ProspectId,
                BirthDate = data.BirthDate,
                CountryId = data.CountryId,
                DocumentNumber = data.DocumentNumber,
                Email = data.Email,
                AreaCode = data.AreaCode,
                Name = data.Name,
                PhoneNumber = data.PhoneNumber
            };

            return model;
        }

        public async Task<ServiceResponse> Register(ProspectViewModel model)
        {
            var result = new ServiceResponse();

            bool flagCreatedUser = false;

            BankUser newCustomersUser = new BankUser() { Email = model.Email, UserName = model.PhoneUid, Name = model.Name };

            try
            {
                await _userManager.CreateAsync(newCustomersUser, model.Password);

                flagCreatedUser = true; 
                
                var prospect = new Prospect
                {
                    BirthDate = model.BirthDate,
                    CountryId = model.CountryId,
                    DocumentNumber = model.DocumentNumber,
                    DocumentTypeId = BankTypes.CONST_DOC_TYPE_CPF.DocumentTypeId,
                    Email = model.Email,
                    AreaCode = model.AreaCode,
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    CreationDate = DateTime.Now,
                    AspNetUserId = newCustomersUser.Id,
                    ProspectStatus = "I"
                };

                if (_db.Prospects.FirstOrDefault(a => a.Email == prospect.Email && a.DocumentNumber == prospect.DocumentNumber) != null)
                {
                    return result.AddError("The selected e-mail has already been taken. Please choose another one.");
                }

                if (_db.Prospects.FirstOrDefault(a => a.PhoneNumber == prospect.PhoneNumber && a.DocumentNumber == prospect.DocumentNumber) != null)
                {
                    return result.AddError("The phone has already been taken. Please choose another one.");
                }

                _db.Prospects.Add(prospect);
                
                _db.Commit();

                result.Data = prospect;

                _noSql.InsertNoWait($"ProspectLog", result.Data);
            }
            catch (Exception ex)
            {
                if (flagCreatedUser)
                {
                    //the aspNet UserMAnager doesn't work well with transactions the way we've made them here, so let's undo by hand in case something happens
                    await _userManager.DeleteAsync(newCustomersUser);
                }

                _db.Rollback();

                result.AddError(ex);
            }

            return result;
        }

        public ServiceResponse Update(ProspectViewModel model)
        {
            var result = new ServiceResponse();

            try
            {
                var prospect = _db.Prospects.Find(model.ProspectId);
                prospect.BirthDate = model.BirthDate;
                prospect.CountryId = model.CountryId;
                prospect.DocumentNumber = model.DocumentNumber;
                prospect.Email = model.Email;
                prospect.AreaCode = model.AreaCode;
                prospect.Name = model.Name;
                prospect.PhoneNumber = model.PhoneNumber;

                _db.Update(prospect);

                //if (!_db.Commit())
                //{
                //    return result.AddError("An unexpected error ocurred while updating your data. Please try again");
                //}

                result.Data = prospect;

                _noSql.InsertNoWait($"ProspectLog", result.Data);
            }
            catch (Exception ex)
            {
                result.AddError(ex);
            }

            return result;
        }

        public ServiceResponse ProceedToNextOnboardingStep(int customerId)
        {
            return new ServiceResponse();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
