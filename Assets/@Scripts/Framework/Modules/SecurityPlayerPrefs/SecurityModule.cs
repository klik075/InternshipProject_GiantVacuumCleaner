
using UnityEngine;

namespace Scripts.Framework.Modules.SecurityPlayerPrefs
{
    public sealed class SecurityModule : SecurityPlayerPref
    {
        #region Secruity Keys
        private static void SetSecurityValue(string key, string value)
        {
            string hideKey = MakeHash(key + SaltForKey);
            string encryptValue = Encrypt(value + MakeHash(value));
            PlayerPrefs.SetString(hideKey, encryptValue);
        }

        private static string GetSecurityValue(string key)
        {
            string hideKey = MakeHash(key + SaltForKey);
            string encryptValue = PlayerPrefs.GetString(hideKey);
            if (string.IsNullOrEmpty(encryptValue)) return string.Empty;
            string valueAndHash = Decrypt(encryptValue);
            if (HashLen > valueAndHash.Length) return string.Empty;
            string savedValue = valueAndHash[..^HashLen];
            string savedHash = valueAndHash[^HashLen..];
            return MakeHash(savedValue) != savedHash ? string.Empty : savedValue;
        }
        #endregion
        
        #region Save & Delete
        public static bool HasKey(string key)
        {
            string hideKey = MakeHash(key + SaltForKey);
            return PlayerPrefs.HasKey(hideKey);
        }
        public static void DeleteKey(string key) => PlayerPrefs.DeleteKey(MakeHash(key + SaltForKey));
        public static void DeleteAll() => PlayerPrefs.DeleteAll();
        public static void Save() => PlayerPrefs.Save();
        #endregion



        #region Set & Get

        public static void SetInt(string key, int value) => SetSecurityValue(key, value.ToString());
        public static void SetLong(string key, long value) => SetSecurityValue(key, value.ToString());
        public static void SetFloat(string key, float value) => SetSecurityValue(key, value.ToString());
        public static void SetBool(string key, bool value) => SetSecurityValue(key, value.ToString()); 
        public static void SetString(string key, string value) => SetSecurityValue(key, value);
        
        public static int GetInt(string key, int defaultValue)
        {
            string originalValue = GetSecurityValue(key);
            if (string.IsNullOrEmpty(originalValue)) return defaultValue;
            return false == int.TryParse(originalValue, out int result) ? defaultValue : result;
        }

        public static long GetLong(string key, long defaultValue)
        {
            string originalValue = GetSecurityValue(key);
            if (string.IsNullOrEmpty(originalValue)) return defaultValue;
            return false == long.TryParse(originalValue, out long result) ? defaultValue : result;
        }

        public static float GetFloat(string key, float defaultValue)
        {
            string originalValue = GetSecurityValue(key);
            if (string.IsNullOrEmpty(originalValue)) return defaultValue;
            return false == float.TryParse(originalValue, out float result) ? defaultValue : result;
        }

        public static string GetString(string key, string defaultValue)
        {
            string originalValue = GetSecurityValue(key);
            return string.IsNullOrEmpty(originalValue) ? defaultValue : originalValue;
        }

        public static bool GetBool(string key, bool defaultValue)
        {
            string originalValue = GetSecurityValue(key);
            if (string.IsNullOrEmpty(originalValue)) return defaultValue;
            return bool.TryParse(originalValue, out bool result) ? result : defaultValue;
        }
        #endregion
    }
}
