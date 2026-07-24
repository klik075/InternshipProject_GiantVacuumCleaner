using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Scripts.Framework.Utility.Loader;
using UnityEngine;

namespace Scripts.Framework.Modules.DeviceModules.LocalizeModule
{
    [CreateAssetMenu(fileName = "LocalizeSO", menuName = "DataContainer/Localize")]
    public class LocalizeSO : DescriptionSO, IJsonSO
    {
        [SerializeField] private TextAsset localizeData;
        [SerializeField] private string soName;
        [SerializeField] private SerializedDictionary<LocalKey,string> stringDic = new(); 
        
        public string SoName() => soName;
        private void OnEnable() => ConvertData();
        public string GetLocalString(LocalKey key) => stringDic.GetValueOrDefault(key);
        private void ConvertData()
        {
            if (localizeData == null) return; 
            JsonLoader loader = new JsonLoader();
            stringDic = loader.GetJson<SerializedDictionary<LocalKey, string>>(localizeData);
        }
    }
}