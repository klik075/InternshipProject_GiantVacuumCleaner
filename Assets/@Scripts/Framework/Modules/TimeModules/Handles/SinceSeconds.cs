using System;
using Scripts.Framework.Modules.SecurityPlayerPrefs;

namespace Scripts.Helper.TimeModules.Handles
{
    public class SinceSeconds : ITimeHandler
    {
        public bool IsSince(string checkKey, int value, bool isUpdate)
        {
            if (!SecurityModule.HasKey(checkKey))
            {
                SecurityModule.SetString(checkKey, DateTime.Now.ToString("O"));
                return true;
            }
            
            string savedDate = SecurityModule.GetString(checkKey, string.Empty);
            
            // if Throw version
            if (!DateTime.TryParse(savedDate, out DateTime lastDateTime)) { return true; }
            
            DateTime currentDateTime = DateTime.Now;
            TimeSpan difference = currentDateTime - lastDateTime;
            
            bool isSince = difference.TotalSeconds >= value;
            if (isSince && isUpdate)
            {
                SecurityModule.SetString(checkKey, DateTime.Now.ToString("O"));
            }
            return isSince;
        }
    }
}