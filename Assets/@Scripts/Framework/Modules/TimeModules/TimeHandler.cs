using System.Collections.Generic;
using Scripts.Helper.TimeModules;
using Scripts.Helper.TimeModules.Handles;

namespace Scripts.Framework.Modules.TimeModules
{
    public enum DateType { SinceAppLaunch, SinceDay, SinceHours, SinceMinute, SinceSecond}

    public class TimeHandler
    {
        private readonly Dictionary<DateType, ITimeHandler> _handleStrategy = new()
        {
            { DateType.SinceAppLaunch , new SinceAppLaunch()},
            { DateType.SinceDay , new SinceDay()},
            { DateType.SinceHours , new SinceHours()},
            { DateType.SinceMinute, new SinceMinute()},
            { DateType.SinceSecond , new SinceSeconds()}
        };

        private readonly ITimeHandler _handler;
        
        public TimeHandler(DateType dateType)
        {
            if (_handleStrategy.TryGetValue(dateType, out ITimeHandler strategy)) _handler = strategy;
        }

        public bool IsSince(string checkKey, int checkValue, bool isUpdate)
        {
            return _handler is not null && _handler.IsSince(checkKey,checkValue, isUpdate);
        }
    }
}