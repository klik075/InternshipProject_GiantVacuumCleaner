using System.Collections.Generic;
using DG.Tweening;
using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Modules.UIModules.UI_Elements_Controller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Framework.Utility
{
    public static class ConvertEx
    {
        #region Sprite To Int

        private static readonly Dictionary<char, Sprite> _numSprites = new();

        public static void ConvertNum(string format, int num, ref Image[] numArray)
        {
            LoadNumSprite();
            ConvertIntToSprite(format, num, ref numArray);
        }
        
        private static void LoadNumSprite()
        {
            if (_numSprites.Count >0) return;
            for (int i = 0; i < 10; i++)
            {
                char charKey = (char)('0' + i);
                _numSprites[charKey] = AMS.GetAsset<Sprite>($"level_{i}.sprite");
            }
        }

        private static Sprite NumValue(char num) => _numSprites.GetValueOrDefault(num);

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
        #endregion
        
        private static string FormatMileage(this TextMeshProUGUI text)
        {
            int parser = int.Parse(text.text);
            if (parser >= 1000000) return (parser / 1000000.0).ToString("0.#") + "M";
            if (parser >= 1000)  return (parser / 1000.0).ToString("0.#") + "k";
            return parser.ToString();
        }
        
        public static Ease ConvertEase(EaseAction.Ease ease, EaseAction.Direction direction)
        {
            return ease switch
            {
                EaseAction.Ease.Quad => direction switch
                {
                    EaseAction.Direction.In => Ease.InQuad,
                    EaseAction.Direction.Out => Ease.OutQuad,
                    _ => Ease.InOutQuad
                },
                EaseAction.Ease.Cubic => direction switch
                {
                    EaseAction.Direction.In => Ease.InCubic,
                    EaseAction.Direction.Out => Ease.OutCubic,
                    _ => Ease.InOutCubic
                },
                EaseAction.Ease.Quart => direction switch
                {
                    EaseAction.Direction.In => Ease.InQuart,
                    EaseAction.Direction.Out => Ease.OutQuart,
                    _ => Ease.InOutQuart
                },
                EaseAction.Ease.Quint => direction switch
                {
                    EaseAction.Direction.In => Ease.InQuint,
                    EaseAction.Direction.Out => Ease.OutQuint,
                    _ => Ease.InOutQuint
                },
                EaseAction.Ease.Sine => direction switch
                {
                    EaseAction.Direction.In => Ease.InSine,
                    EaseAction.Direction.Out => Ease.OutSine,
                    _ => Ease.InOutSine
                },
                EaseAction.Ease.Back => direction switch
                {
                    EaseAction.Direction.In => Ease.InBack,
                    EaseAction.Direction.Out => Ease.OutBack,
                    _ => Ease.InOutBack
                },
                EaseAction.Ease.Circ => direction switch
                {
                    EaseAction.Direction.In => Ease.InCirc,
                    EaseAction.Direction.Out => Ease.OutCirc,
                    _ => Ease.InOutCirc
                },
                EaseAction.Ease.Bounce => direction switch
                {
                    EaseAction.Direction.In => Ease.InBounce,
                    EaseAction.Direction.Out => Ease.OutBounce,
                    _ => Ease.InOutBounce
                },
                EaseAction.Ease.Elastic => direction switch
                {
                    EaseAction.Direction.In => Ease.InElastic,
                    EaseAction.Direction.Out => Ease.OutElastic,
                    _ => Ease.InOutElastic
                },
                _ => Ease.Linear
            };
        }
    }
}