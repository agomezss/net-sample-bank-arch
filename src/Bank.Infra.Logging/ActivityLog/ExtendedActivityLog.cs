using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Infra.Logging
{
    internal sealed class ExtendedActivityLog
    {
        public Dictionary<DateTime, string> Messages { get; set; }
        public string ProcessKey { get; set; }
        public string ActivityKey { get; set; }

        public ExtendedActivityLog(string processKey, string activityKey, string startingMessage = null)
        {
            ProcessKey = processKey;
            ActivityKey = activityKey;
            Messages = new Dictionary<DateTime, string>();

            if (startingMessage != null)
            {
                Log(startingMessage);
            }
        }

        public void Log(string message)
        {
            DateTime nowDate = DateTime.Now;

            while (Messages.ContainsKey(nowDate))
            {
                nowDate = nowDate.AddMilliseconds(1);
            }

            Messages.Add(nowDate, $"{ActivityKey}> {message}");
        }

        public void LogAndSwitchActivity(string activityKey, string message)
        {
            Log(message);
            ActivityKey = activityKey;
            Log("Activity started");
        }

        public void SwitchActivityAndLog(string activityKey, string message = "Activity started")
        {
            ActivityKey = activityKey;
            Log(message);
        }

        public void Clear()
        {
            Messages.Clear();
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            foreach (DateTime keyDate in Messages.Keys)
            {
                builder.AppendFormat("{0}>{1}>{2}{3}",
                  keyDate.ToString("yyyy-MM-dd-HH-mm-ss-mmm"),
                  ProcessKey,
                  Messages[keyDate],
                  Environment.NewLine);
            }

            return builder.ToString();
        }
    }
}
