using Newtonsoft.Json;
using Scripts.Framework.Managers.Asset.Core;
using UnityEngine;

namespace Scripts.Framework.Utility.Loader
{
    public class JsonLoader
    {
        public T GetJson<T>(TextAsset asset) => DeserializeJson<T>(asset.text);
        
        public T GetJson<T>(string key)
        {
            string data = AMS.GetAsset<TextAsset>(key).ToString();
            return DeserializeJson<T>(data);
        }
        private T DeserializeJson<T>(string data) => JsonConvert.DeserializeObject<T>(data);
    }
}