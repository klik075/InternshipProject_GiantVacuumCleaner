using TMPro;
using UnityEngine;

namespace Scripts.Framework.Modules.DeviceModules.LocalizeModule
{
    public class LocalizeComponent : MonoBehaviour
    {
        [SerializeField] private LocalKey localKey;
        private TextMeshProUGUI _tmp;
        private DeviceManager _dm;
        private async void Awake()
        {
            DeviceConfig.ChangeLocalConfig += Localization;
            _dm = await CentralProcessor.I.GetSingleton<DeviceManager>();
            _tmp = GetComponent<TextMeshProUGUI>();
            Localization();
        }

        private void Localization()
        {
            _tmp.text = _dm.GetLocalString(localKey);
        }
    }
}