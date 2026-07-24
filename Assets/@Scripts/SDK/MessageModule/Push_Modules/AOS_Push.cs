#if UNITY_ANDROID
using System;
using System.Collections;
using Scripts.SDK.MessageModule.Interface;
using Unity.Notifications.Android;
using UnityEngine.Android;

namespace Scripts.SDK.MessageModule.Push_Modules
{
    public class AOS_Push : IPushMessage
    {
        
         // TODO : Require Unity Package
         // TODO : Unity Registry "Mobile Notification"
         // TODO : ProjectSetting > Mobile Notification > "Android" > Register Notification Icons [small (icon_0), large(icon_1)]
        
        private PermissionCallbacks _permissionCallbacks;
        
        public IEnumerator RequestAuthorization()
        {
            if (_permissionCallbacks == null)
            {
                _permissionCallbacks = new PermissionCallbacks();
                _permissionCallbacks.PermissionGranted -= RegisterNotificationChannel;
                _permissionCallbacks.PermissionGranted += RegisterNotificationChannel;
                if (Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS")) yield break;
                Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS", _permissionCallbacks);
                yield return null;
            }
        }

        private void RegisterNotificationChannel(string permission)
        {
#if DEV
            Debugger.LogWarning("Push Permission Granted: " + permission);
#endif
            AndroidNotificationChannel channel = new AndroidNotificationChannel
            {
                Id = "PlayTime_Alarm",
                Name = "Play Time Alarm",
                Description = "Channel for Play Time notifications",
                Importance = Importance.Default,
                CanBypassDnd = true,
                CanShowBadge = true,
                EnableLights = true,
                EnableVibration = true,
                VibrationPattern = new long[] { 100, 200, 300 },
                LockScreenVisibility = LockScreenVisibility.Public
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }
        
        public void SendTimeIntervalNotification(string title, string body, int hours)
        {
            AndroidNotification notification = new AndroidNotification
            {
                Title = title,
                Text = body,
                FireTime = DateTime.Now.AddHours(hours),
                SmallIcon = "icon_0",
                LargeIcon = "icon_1"
            };
            AndroidNotificationCenter.SendNotification(notification, "PlayTime_Alarm");
        }

        public void SendCalendarNotification(string title, string body, int hour, int minute)
        {
            DateTime now = DateTime.Now;
            DateTime fireTime = new DateTime(now.Year, now.Month, now.Day, hour, minute, 0, DateTimeKind.Local);
            if (fireTime < now) fireTime = fireTime.AddDays(1);
            AndroidNotification notification = new AndroidNotification
            {
                Title = title,
                Text = body,
                FireTime = fireTime,
                RepeatInterval = TimeSpan.FromDays(1),
                SmallIcon = "icon_0",
                LargeIcon = "icon_1"
            };
            AndroidNotificationCenter.SendNotification(notification, "PlayTime_Alarm");
        }

        public void SendNotification(string title, string body, int fireTimeHours)
        {
            AndroidNotification notification = new AndroidNotification
            {
                Title = title,
                Text = body,
                FireTime = DateTime.Now.AddHours(fireTimeHours),
                SmallIcon = "icon_0",
                LargeIcon = "icon_1"
            };
            AndroidNotificationCenter.SendNotification(notification, "Notification!");
        }

        public void ScheduleNotifications()
        {
            // // Last Play To Request
            // SendTimeIntervalNotification(
            //     LocalizeManager.GetLocalString(StringTypes.NotificationTitle_24H),
            //     LocalizeManager.GetLocalString(StringTypes.NotificationBody_24H),
            //     24
            // );
            //
            // // Every Day To Request at 12:00
            // SendCalendarNotification(
            //     LocalizeManager.GetLocalString(StringTypes.NotificationTitle_12H),
            //     LocalizeManager.GetLocalString(StringTypes.NotificationBody_12H),
            //     12,
            //     0
            // );
            //
            // // Every Day To Request at 20:00
            // SendCalendarNotification(
            //     LocalizeManager.GetLocalString(StringTypes.NotificationTitle_20H),
            //     LocalizeManager.GetLocalString(StringTypes.NotificationBody_20H),
            //     20,
            //     0
            // );
        }
    }
}
#endif