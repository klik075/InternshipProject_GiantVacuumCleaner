using System;
using Scripts.Framework.Modules.SecurityPlayerPrefs;

namespace Scripts.Helper.TimeModules.Handles
{
    public class SinceAppLaunch : ITimeHandler
    {
        private const string SinceAppLaunchKey = "AppLauchDate";

        public SinceAppLaunch()
        {
            if (!SecurityModule.HasKey(SinceAppLaunchKey))
            {
                SecurityModule.SetString(SinceAppLaunchKey, DateTime.Now.ToString("o"));
            }
        }
        
        public bool IsSince(string checkKey, int checkValue, bool isUpdate = false)
        {
            string savedDate = SecurityModule.GetString(SinceAppLaunchKey, string.Empty);
            if (!DateTime.TryParse(savedDate, out DateTime launchTime)) return false;
            DateTime currentDate = DateTime.Now;
            TimeSpan difference = currentDate.Date - launchTime.Date;
            return difference.Days >= checkValue;
        }
    }
}