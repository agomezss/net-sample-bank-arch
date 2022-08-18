using Bank.Core.Interfaces;
using System;

namespace Bank.Infra.Logging
{
    public class ExtendedActivityLogger : IActivityLogger
    {
        private ExtendedActivityLog _logs;

        public void StartExtendedActivity(string processKey, string activityKey, string startingMessage = "Activity started")
        {
            _logs = new ExtendedActivityLog(processKey, activityKey, startingMessage);
        }

        public IActivityLogger Log(string message)
        {
            _logs.Log(message);
            return this;
        }

        public IActivityLogger LogFormat(string message, params object[] args)
        {
            Log(string.Format(message, args));
            return this;
        }

        public IActivityLogger LogAndSwitchActivity(string newActivityKey, string message)
        {
            _logs.LogAndSwitchActivity(newActivityKey, message);
            return this;
        }

        public IActivityLogger LogAndSwitchActivityFormat(string newActivityKey, string message, params object[] args)
        {
            LogAndSwitchActivity(newActivityKey, string.Format(message, args));
            return this;
        }

        public IActivityLogger SwitchActivityAndLog(string newActivityKey, string message = "Activity started")
        {
            _logs.SwitchActivityAndLog(newActivityKey, message);
            return this;
        }

        public IActivityLogger SwitchActivityAndLogFormat(string newActivityKey, string message, params object[] args)
        {
            return SwitchActivityAndLog(newActivityKey, string.Format(message, args));
        }

        public IActivityLogger LogError(Exception ex, bool terminateActivity = false)
        {
            Log($"Exception. Terminate? [{terminateActivity}]>: {ex.Message}\r\n{ex.StackTrace}");

            if (terminateActivity)
            {
                // TODO: Implement
            }

            return this;
        }

        public string FinishExtendedActivity()
        {
            LogFormat("Extended activity finished");

            var flushedLogs = _logs.ToString();

            _logs.Clear();

            return flushedLogs;
        }
    }
}
