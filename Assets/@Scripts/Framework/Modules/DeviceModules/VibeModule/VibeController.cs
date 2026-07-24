using UnityEngine;

namespace Scripts.Framework.Modules.DeviceModules.VibeModule
{
    public enum IOSVibrationPattern { Light, Medium, Heavy }

    public class VibeController : DeviceController
    {
        protected override void Init()
        {
            base.Init();
        }
        
#if UNITY_ANDROID
        public void OnVibe(long milliseconds = 500)
        {
            if (!DeviceManager.IsVibed) return;
            VibrateAndroid(milliseconds);
        }
        
        private void VibrateAndroid(long milliseconds)
        {
            using AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using AndroidJavaClass vibrationHelper = new AndroidJavaClass("com.actionfit.nativemodules.vibecontrol.VibrationHelper");
            vibrationHelper.CallStatic("Vibrate", currentActivity, milliseconds);
        }
        
#elif UNITY_IOS
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void iOSVibrate(string pattern);

        public void OnVibe(IOSVibrationPattern pattern = IOSVibrationPattern.Light)
        {
            if (!DeviceManager.IsVibed) return;
            string patternStr = pattern.ToString().ToLower();
            iOSVibrate(patternStr);
        }
#endif
    }
}