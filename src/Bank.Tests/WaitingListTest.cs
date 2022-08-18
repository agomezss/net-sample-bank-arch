using Bank.Core.Context;
using Bank.Core.Extensions;
using Bank.Services.Authentication;
using Bank.Services.Onboarding.Enum;
using Bank.Services.Onboarding.Interfaces;
using Bank.Services.Onboarding.Models;
using Bank.Services.Onboarding.Models.WaitingList;
using Bank.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Bank.Tests
{
    public class WaitingListTest : IClassFixture<DependencyFixture>, IDisposable
    {
        private ServiceProvider _serviceProvider;
        private IWaitingListService _service;
        private OnboardingDbContext _context;

        public WaitingListTest(DependencyFixture fixture)
        {
            _serviceProvider = _serviceProvider ?? fixture.ServiceProvider;

            var db = _serviceProvider.GetService<OnboardingDbContext>();
            db.SetTestMode();

            var dbAuth = _serviceProvider.GetService<AuthDbContext>();
            dbAuth.SetTestMode();

            _context = db;

            SetupMocks();
        }

        private void SetupMocks()
        {
            var svc = _serviceProvider.GetService<IWaitingListService>();
            //_service = Substitute.For(svc);
        }

        private WaitingList CreateWaitingList(int capacity = 5)
        {
            var name = Guid.NewGuid().ToString();
            var service = _serviceProvider.GetService<IWaitingListService>();

            var result = service.CreateWaitingList(name, capacity, "A").Data as WaitingList;

            return result;
        }

        private Prospect CreateProspect(string mail = "agomez@samplebank.com",
                                        string name = "Unit Test Prospect",
                                        string documentNumber = "123456789",
                                        string phoneNumber = "5511928282828")
        {
            var authContext = _serviceProvider.GetService<AuthDbContext>();
            var userId = authContext.Users.FirstOrDefault().Id;

            var prospect = new Prospect()
            {
                AreaCode = "55",
                AspNetUserId = userId,
                BirthDate = DateTime.Now.AddYears(-20),
                CountryId = 1,
                DocumentNumber = documentNumber,
                DocumentTypeId = 1,
                Email = mail,
                Name = name,
                PhoneNumber = phoneNumber,
                ProspectStatus = "A",
                CreationDate = DateTime.Now
            };

            _context.Prospects.Add(prospect);
            _context.SaveChanges();

            return prospect;
        }

        /// <summary>
        /// Cover creation of waiting list.
        /// </summary>
        [Fact]
        public void TestCreateWaitingList()
        {
            // Arrange
            var name = Guid.NewGuid().ToString();

            // Act
            //var service = _serviceProvider.GetService<IWaitingListService>();
            var result = _service.CreateWaitingList(name, 5, "A");

            // Assert
            Assert.True(!result.HasErrors());
        }

        /// <summary>
        /// Cover updating data of waiting list.
        /// </summary>
        [Fact]
        public void TestUpdateWaitingList()
        {
            // Arrange
            var name = Guid.NewGuid().ToString();
            var service = _serviceProvider.GetService<IWaitingListService>();
            var waitingList = service.CreateWaitingList(name, 5, "A").Data as WaitingList;

            // Act
            var result = service.UpdateWaitingList(waitingList.WaitingListId, "Changed", 5, "A");
            var updated = _context.WaitingLists.First(a => a.WaitingListId == waitingList.WaitingListId);

            // Assert
            Assert.True(!result.HasErrors());
            Assert.Equal("Changed", updated.Name);
        }


        /// <summary>
        /// Cover simple insertion of customer into a waiting list.
        /// </summary>
        [Fact]
        public void TestInsertOnWaitingList()
        {
            // Arrange
            var prospect = CreateProspect(mail: NewMail());
            var waitingList = CreateWaitingList();
            var service = _serviceProvider.GetService<IWaitingListService>();

            // Act
            var result = service.InsertOnWaitingList(prospect.ProspectId, waitingList.WaitingListId);

            // Assert
            Assert.True(!result.HasErrors());
            Assert.NotNull(result.Data);
        }


        /// <summary>
        /// This test creates a waiting list with a capacity of 1 attendance per day.
        /// So, the expected result after inserting 2 customers is that each one is expected
        /// to be attendant on diferent days.
        /// </summary>
        [Fact]
        public void TestInsertMultipleOnWaitingList()
        {
            // Arrange
            var prospect = CreateProspect(mail: NewMail());
            var prospect2 = CreateProspect(name:"Test2", mail: NewMail());
            var waitingList = CreateWaitingList(1);
            var service = _serviceProvider.GetService<IWaitingListService>();

            // Act
            var result = service.InsertOnWaitingList(prospect.ProspectId, waitingList.WaitingListId);
            var result2 = service.InsertOnWaitingList(prospect2.ProspectId, waitingList.WaitingListId);

            // Assert
            Assert.True(!result.HasErrors());
            Assert.NotNull(result.Data);
            Assert.NotEqual(result.Data.GetProperty("EstimateDate"),
                            result2.Data.GetProperty("EstimateDate"));

            Assert.NotEqual(result.Data.GetProperty("EstimatePosition"),
                            result2.Data.GetProperty("EstimatePosition"));
        }

        /// <summary>
        /// Cover updating data of waiting list.
        /// </summary>
        [Fact]
        public void TestAttend()
        {
            // Arrange
            var prospect = CreateProspect(mail: NewMail());
            var waitingList = CreateWaitingList();
            var service = _serviceProvider.GetService<IWaitingListService>();
            var insertion = service.InsertOnWaitingList(prospect.ProspectId, waitingList.WaitingListId);

            // Act
            var result = service.Attend(prospect.ProspectId, waitingList.WaitingListId);
            var waitingListSubjects = service.GetWaitingListElements(waitingList.WaitingListId).Data as List<CustomerWaitingList>;
            var subject = waitingListSubjects.First(a => a.ProspectId == prospect.ProspectId);

            // Assert
            Assert.True(!result.HasErrors());
            Assert.Equal((int)WaitingListStatusEnum.Completed, subject.StatusId);
        }

        public void Dispose()
        {
            try
            {
                _context.Rollback();
            }
            catch (Exception ex)
            {
            }
        }

        private string NewMail()
        {
            return $"{Guid.NewGuid()}@samplebank.com";
        }
    }
}
