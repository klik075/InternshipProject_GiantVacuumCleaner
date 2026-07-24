using System.Collections.Generic;
using UnityEditor.AddressableAssets;
using UnityEngine.AddressableAssets;

namespace Scripts.Framework.Managers.Asset.Editor
{
    public static class AssetLabelUtility
    {
        public static List<AssetLabelReference> GetAllAssetLabels()
        {
            var assetLabelReferences = new List<AssetLabelReference>();
            var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;

            if (addressableSettings == null)
            {
                Debugger.LogError("AddressableAssetSetting is null.");
                return assetLabelReferences;
            }

            foreach (string label in addressableSettings.GetLabels())
            {
                var assetLabelReference = new AssetLabelReference();
                assetLabelReference.labelString = label;
                assetLabelReferences.Add(assetLabelReference);
            }

            return assetLabelReferences;
        }
    }
}