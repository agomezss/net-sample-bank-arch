using Newtonsoft.Json;
using System;

namespace Bank.Core.ViewModels
{
    public class MobilePhoneActivityRequest : BaseRequest
    {
        public MobilePhoneActivityRequest()
        {
        }

        private string _phoneUid;

        public new string PhoneUid
        {
            get
            {
                return _phoneUid;
            }

            set
            {
                _phoneUid = value;

                if (ActivityLog != null)
                {
                    ActivityLog.PhoneUid = value;
                }
            }
        }

        private DateTime _phoneLocalTime;

        public new DateTime PhoneLocalTime
        {
            get
            {
                return _phoneLocalTime;
            }

            set
            {
                _phoneLocalTime = value;

                if (ActivityLog != null)
                {
                    ActivityLog.PhoneLocalTime = value;
                }
            }
        }

        public ActivityLogBaseRequest ActivityLog { get; set; }

        public void RegisterActivity(string category, object activityData = null)
        {
            ActivityLog = new ActivityLogBaseRequest
            {
                PhoneUid = PhoneUid,
                PhoneLocalTime = PhoneLocalTime,
                Category = category
            };

            if (activityData != null)
            {
                ActivityLog.ActivityJsonData = JsonConvert.SerializeObject(activityData);
            }
        }
    }
}
