using System;

namespace Bank.Core.Interfaces
{
    public interface IActivityLogger
    {
        IActivityLogger Log(string message);
        IActivityLogger LogAndSwitchActivity(string newActivityKey, string message);
        IActivityLogger LogAndSwitchActivityFormat(string newActivityKey, string message, params object[] args);
        IActivityLogger LogError(Exception ex, bool terminateActivity = false);
        IActivityLogger LogFormat(string message, params object[] args);
        IActivityLogger SwitchActivityAndLog(string newActivityKey, string message = "Activity started");
        IActivityLogger SwitchActivityAndLogFormat(string newActivityKey, string message, params object[] args);
        void StartExtendedActivity(string processKey, string activityKey, string startingMessage = "Activity started");
        string FinishExtendedActivity();
    }
}
