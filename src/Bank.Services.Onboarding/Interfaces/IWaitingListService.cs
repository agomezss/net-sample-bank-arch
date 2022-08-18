using Bank.Core;
using Bank.Services.Onboarding.Enum;
using System;

namespace Bank.Services.Onboarding.Interfaces
{
    public interface IWaitingListService
    {
        ServiceResponse CreateWaitingList(string name, int capacity, string active);
        ServiceResponse UpdateWaitingList(int waitingListId, string name, int capacity, string active);
        ServiceResponse InsertOnWaitingList(int prospectId, int waitingListId);
        DateTime GetAttendanceEstimativeDate(int prospectId, int waitingListId);
        ServiceResponse Attend(int prospectId, int waitingListId, int statusId = (int)WaitingListStatusEnum.Completed);
        ServiceResponse GetWaitingListElements(int waitingListId, int maxQuantity = 5, int[] statuses = null);
    }
}
