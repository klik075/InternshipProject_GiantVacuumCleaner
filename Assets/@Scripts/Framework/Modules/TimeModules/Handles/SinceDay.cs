
using System;
using Scripts.Framework.Modules.SecurityPlayerPrefs;

namespace Scripts.Helper.TimeModules.Handles
{
    public class SinceDay: ITimeHandler
    {
        public bool IsSince(string checkKey, int value, bool isUpdate)
        {
            
            if (!SecurityModule.HasKey(checkKey))
            {
                SecurityModule.SetString(checkKey, DateTime.Now.ToString("O"));
                return true;
            }

            string saveDate = SecurityModule.GetString(checkKey, string.Empty);
            if (!DateTime.TryParse(saveDate, out DateTime lastDay)) return true;
            DateTime currentDay = DateTime.Now;
            TimeSpan difference = currentDay.Date - lastDay.Date;
            bool isSince = difference.Days >= value;
            if (isSince && isUpdate) SecurityModule.SetString(checkKey, DateTime.Now.ToString("O"));
            return isSince;
        }
    }
}