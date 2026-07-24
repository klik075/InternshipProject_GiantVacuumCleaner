using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Framework.Utility
{
    public static class Util
    {
        #region Component
        public static T GetAddComponent<T>(this GameObject obj) where T : Component => obj.GetComponent<T>() ?? obj.AddComponent<T>();
        #endregion

        #region int To string
        public static string FormatMoney(int value)
        {
            if (value >= 1000000) return (value / 1000000.0).ToString("0.#") + "M";
            if (value >= 1000)  return (value / 1000.0).ToString("0.#") + "k";
            return value.ToString();
        }
        #endregion
        
        #region Sprite To Int
        private static readonly Dictionary<char, Sprite> NumSprites = new();
        private static void LoadNumSprite()
        {
            if (NumSprites.Count >0) return;
            for (int i = 0; i < 10; i++)
            {
                char charKey = (char)('0' + i);
                NumSprites[charKey] = Managers.Asset.Core.AMS.GetAsset<Sprite>(charKey.ToString());
            }
        }
        private static Sprite NumValue(char num) => NumSprites.GetValueOrDefault(num);
        private static bool IsPreviousNonZero(string numStr, int index)
        {
            for (int j = 0; j < index; j++) if (numStr[j] != '0') return true;
            return false;
        }
        private static void ConvertIntToSprite(string format, int num, ref Image[] numArray)
        {
            string numStr = num.ToString(format);
            for (int i = 0; i < numArray.Length; i++)
            {
                if (i < numStr.Length)
                {
                    Sprite sprite = NumValue(numStr[i]);
                    numArray[i].sprite = sprite;
                    numArray[i].gameObject.SetActive(IsPreviousNonZero(numStr, i) || numStr[i] != '0' || i == numStr.Length - 1);
                }
                else numArray[i].gameObject.SetActive(false);
            }
        }
        public static void ConvertNum(string format, int num, ref Image[] numArray)
        {
            LoadNumSprite();
            ConvertIntToSprite(format, num, ref numArray);
        }
        #endregion
    }
}