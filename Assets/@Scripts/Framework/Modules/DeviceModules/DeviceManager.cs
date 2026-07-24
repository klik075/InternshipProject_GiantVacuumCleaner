using Scripts.Framework.Managers.Asset.Core;
using Scripts.Framework.Modules.DeviceModules.LocalizeModule;
using Scripts.Framework.Modules.DeviceModules.SoundModule;
using Scripts.Framework.Modules.DeviceModules.VibeModule;
using UnityEngine;

namespace Scripts.Framework.Modules.DeviceModules
{
    public class DeviceManager : MonoBehaviour
    {
        [SerializeField] public LocalizeSO[] localizeSoList;
        private SoundClipSO _soundSo;
        public static bool IsBGMMuted
        {
            get => DeviceConfig.GetDeviceConfig(DeviceConfig.BGM_CONFIG,false);
            set => DeviceConfig.SetDeviceConfig(DeviceConfig.BGM_CONFIG, value);
        }
        public static bool IsSfxMuted
        {
            get => DeviceConfig.GetDeviceConfig(DeviceConfig.SFX_CONFIG, false);
            set => DeviceConfig.SetDeviceConfig(DeviceConfig.SFX_CONFIG, value);
        }
        public static bool IsVibed
        {
            get => DeviceConfig.GetDeviceConfig(DeviceConfig.VIBE_CONFIG, true);
            set => DeviceConfig.SetDeviceConfig(DeviceConfig.VIBE_CONFIG, value);
        }
        public static Language CurrentLanguage
        {
            get => DeviceConfig.GetLanguage();
            set => DeviceConfig.SetLanguage(value);
        }

        private void Awake()
        {
            _soundSo = AMS.GetAsset<SoundClipSO>("ClipSO");
        }

        public SoundController AttachSoundController(GameObject parent) => parent.AddComponent<SoundController>();
        public VibeController AttachVibeController(GameObject parent) => parent.AddComponent<VibeController>();
        public AudioClip GetClip(SoundClipName clipName) => _soundSo.GetClip(clipName);
        public string GetLocalString(LocalKey key) => localizeSoList[(int)CurrentLanguage].GetLocalString(key);
    }
}