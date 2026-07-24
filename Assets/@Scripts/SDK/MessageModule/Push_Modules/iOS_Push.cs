#if UNITY_IOS
using System;
using System.Collections;
using Scripts.SDK.MessageModule.Interface;
using Unity.Notifications.iOS;

namespace Scripts.SDK.MessageModule.Push_Modules
{
    public class iOS_Push : IPushMessage
    {
        public IEnumerator RequestAuthorization()
        {
            using AuthorizationRequest request = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);
            while (!request.IsFinished) yield return null;
        }

        public void SendTimeIntervalNotification(string title, string body, int hours)
        {
            iOSNotificationTimeIntervalTrigger timeTrigger = new iOSNotificationTimeIntervalTrigger()
            {
                TimeInterval = new TimeSpan(hours, 0, 0),
                Repeats = false,
            };

            iOSNotification notification = new iOSNotification
            {
                Identifier = "PlayTime_Alarm_" + Guid.NewGuid(),
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge,
                CategoryIdentifier = "EveryTime",
                ThreadIdentifier = "thread1",
                Trigger = timeTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void SendCalendarNotification(string title, string body, int hour, int minute)
        {
            iOSNotificationCalendarTrigger calendarTrigger = new iOSNotificationCalendarTrigger
            {
                Hour = hour,
                Minute = minute,
                Repeats = true,
            };

            iOSNotification notification = new iOSNotification
            {
                Identifier = "PlayTime_Alarm_" + Guid.NewGuid(),
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge,
                CategoryIdentifier = "EveryTime",
                ThreadIdentifier = "thread1",
                Trigger = calendarTrigger,
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void SendNotification(string title, string body, int fireTimeHours)
        {
            iOSNotification notification = new iOSNotification
            {
                Identifier = "PlayTime_Alarm_" + Guid.NewGuid(),
                Title = title,
                Body = body,
                ShowInForeground = true,
                ForegroundPresentationOption = PresentationOption.Alert | PresentationOption.Badge,
                CategoryIdentifier = "EveryTime",
                ThreadIdentifier = "thread1",
            };
            iOSNotificationCenter.ScheduleNotification(notification);
        }

        public void ScheduleNotifications()
        {
            // // Last Play To Request 24H
            // SendTimeIntervalNotification(
            //     LocalizeManager.GetLocalString(StringTypes.NotificationTitle_24H),
            //     LocalizeManager.GetLocalString(StringTypes.NotificationBody_24H),
            //     24
            //     );
            //
            // // Every Day To Request at 12:00
            // SendCalendarNotification(
            //     LocalizeManager.GetLocalString(StringTypes.NotificationTitle_12H),
            //     LocalizeManager.GetLocalString(StringTypes.NotificationBody_12H),
            //     12,
            //     0
            //     );
            //
            // // Every Day To Request at 20:00
            // SendCalendarNotification(
            //     LocalizeManager.GetLocalString(StringTypes.NotificationTitle_20H),
            //     LocalizeManager.GetLocalString(StringTypes.NotificationBody_20H),
            //     20,
            //     0
            //     );
        }
    }
}
#endif