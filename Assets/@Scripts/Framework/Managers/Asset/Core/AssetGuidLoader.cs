using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Scripts.Framework.Managers.Asset.Core
{
    public class AssetGuidLoader
    {
        #region Field

        private TextAsset _jsonText;
        private Dictionary<string, string> _assetGuidMap;

        #endregion



        #region Constructor

        public AssetGuidLoader(TextAsset jsonText)
        {
            _jsonText = jsonText;
        
            if (jsonText != null)
            {
                _assetGuidMap = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonText.text);
                Debugger.Log("Addressable Asset GUIDs loaded successfully!");
            }
            else
            {
                Debugger.LogError("AddressableAssetGuids.json not found!");
            }
        }

        #endregion



        #region Get Utils

        public string GetGuidForPath(string assetPath)
        {
            if (_assetGuidMap != null && _assetGuidMap.TryGetValue(assetPath, out var guid))
            {
                return guid;
            }

            Debug.LogWarning($"GUID for asset path '{assetPath}' not found.");
            return null;
        }

        #endregion
    }
}