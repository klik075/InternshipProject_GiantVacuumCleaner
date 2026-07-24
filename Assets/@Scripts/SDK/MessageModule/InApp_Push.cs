#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif
using Scripts.SDK.MessageModule.Push_Modules;

namespace Scripts.SDK.MessageModule
{
    public class InApp_Push : IndividualSingletonPersist<InApp_Push>
    {
        //TODO : Require Unity Package
        //TODO : Unity Registry "Mobile Notification"
        
#if UNITY_ANDROID
        private AOS_Push _aos;
#elif UNITY_IOS
        private iOS_Push _ios;
#endif

        private void Awake() => InitializeNotification();
        private void InitializeNotification()
        {
#if UNITY_ANDROID
            _aos = new AOS_Push();
            AOSREquest();
#elif UNITY_IOS
            _ios = new iOS_Push();
            IOSRequest();
#endif
        }
      
#if UNITY_ANDROID
        private void AOSREquest() => StartCoroutine(_aos.RequestAuthorization());
#elif UNITY_IOS
        private void IOSRequest() => StartCoroutine(_ios.RequestAuthorization());
#endif
        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) return;
#if UNITY_ANDROID
                AndroidNotificationCenter.CancelAllNotifications();
                _aos?.ScheduleNotifications();
#elif UNITY_IOS
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            _ios?.ScheduleNotifications();
#endif
        }
        
        
#if Firebase_CloudMessage
        //TODO : If use to "Cloud Message", Build Setting Symbol Install => "Firebase_CloudMessage"
            public void InitializeMessaging()
            {
                    FirebaseMessaging.TokenReceived += OnTokenReceived;
                    FirebaseMessaging.MessageReceived += OnMessageReceived;
            }
        
            private void OnTokenReceived(object sender, TokenReceivedEventArgs token) 
            {
                    Debugger.Log("Received Registration Token: " + token.Token);
            }

            private void OnMessageReceived(object sender, MessageReceivedEventArgs e)
            {
                    OnFirebaseMessaging(sender, e);
#if Dev
            Debugger.Log($"Push Message Title : {e.Message.Notification.Title}");
            Debugger.Log($"Push Message Title : {e.Message.Notification.Body}");
#endif
            }

            public void OnFirebaseMessaging(object sender, MessageReceivedEventArgs e)
            {
                    InitializeNotification();
                    string title = e.Message.Notification.Title;
                    string body = e.Message.Notification.Body;
#if UNITY_ANDROID
            _aos?.SendNotification(title, body, 0);
#elif UNITY_IOS
                    _ios?.SendNotification(title, body,0);
#endif
            }
#endif
    }
}