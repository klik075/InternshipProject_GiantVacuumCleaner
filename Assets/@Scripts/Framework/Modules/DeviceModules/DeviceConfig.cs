using System;
using Scripts.Framework.Modules.SecurityPlayerPrefs;

namespace Scripts.Framework.Modules.DeviceModules
{
    public enum Language{ ChineseSimplified = 0, ChineseTraditional = 1, English = 2, Italian = 3, Japanese = 4, Korean = 5, Russian = 6, Spanish = 7 }
    
    public static class DeviceConfig
    {
        public const string BGM_CONFIG = "BGM_CONFIG";
        public const string SFX_CONFIG = "SFX_CONFIG";
        public const string VIBE_CONFIG = "VIBE_CONFIG";
        public const string LOCAL_CONFIG = "LOCAL_CONFIG";
        public static event Action<string, bool> ChangedSoundConfig = delegate { };
        public static event Action ChangeLocalConfig = delegate { };
        public static bool GetDeviceConfig(string key, bool defaultValue) => SecurityModule.GetBool(key,defaultValue);
        public static void SetDeviceConfig(string key, bool boolean)
        {
            SecurityModule.SetBool(key,boolean);
            ChangedSoundConfig.Invoke(key, boolean);
        }

        public static Language GetLanguage()
        {
            int config = SecurityModule.GetInt(LOCAL_CONFIG, 2);
            if (Enum.IsDefined(typeof(Language), config)) return (Language)config;
            return Language.English;
        }
        public static void SetLanguage(Language language)
        {
            SecurityModule.SetInt(LOCAL_CONFIG, (int)language);
            ChangeLocalConfig.Invoke();
        }
    }
}