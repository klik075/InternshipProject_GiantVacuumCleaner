using System.Collections;

namespace Scripts.SDK.MessageModule.Interface
{
    public interface IPushMessage
    {
        IEnumerator RequestAuthorization();
        void SendTimeIntervalNotification(string title, string body, int hours);
        void SendCalendarNotification(string title, string body, int hour, int minute);
        void SendNotification(string title, string text, int fireTimeHours);
        void ScheduleNotifications();
    }
}