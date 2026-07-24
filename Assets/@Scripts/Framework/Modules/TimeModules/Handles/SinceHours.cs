using System;
using Scripts.Framework.Modules.SecurityPlayerPrefs;

namespace Scripts.Helper.TimeModules.Handles
{
    public class SinceHours : ITimeHandler
    {
        public bool IsSince(string checkKey, int value, bool isUpdate)
        {
            if (!SecurityModule.HasKey(checkKey))
            {
                SecurityModule.SetString(checkKey, DateTime.Now.ToString("O"));
                return true;
            }

            string savedDate = SecurityModule.GetString(checkKey, string.Empty);
            if (!DateTime.TryParse(savedDate, out DateTime lastDateTime)) return true;
            DateTime currentDateTime = DateTime.Now;
            TimeSpan difference = currentDateTime - lastDateTime;
            bool isSince = difference.TotalHours >= value;
            if (isSince && isUpdate) SecurityModule.SetString(checkKey, DateTime.Now.ToString("O"));
            return isSince;
        }
    }
}