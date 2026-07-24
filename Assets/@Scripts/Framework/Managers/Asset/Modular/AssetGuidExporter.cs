
#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Scripts.Framework.Managers.Asset.Modular
{
    public class AddressableGuidExporter : MonoBehaviour
    {
        [MenuItem("Tools/Export Addressable Asset GUIDs")]
        public static void ExportAddressableAssetGuids()
        {
            ExportAddressableGuids();
        }

        private static void ExportAddressableGuids()
        {
            // Addressable Asset 설정을 가져옵니다.
            AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
            Dictionary<string, string> assetGuidMap = new Dictionary<string, string>();

            // 각 그룹을 순회하며 에셋 경로와 GUID를 추출합니다.
            foreach (AddressableAssetGroup group in settings.groups)
            {
                foreach (AddressableAssetEntry entry in group.entries)
                {
                    string path = AssetDatabase.GUIDToAssetPath(entry.guid);
                    if (path.StartsWith("Assets/")) assetGuidMap[path] = entry.guid;
                }
            }

            // JSON 파일로 저장합니다.
            string json = JsonConvert.SerializeObject(assetGuidMap, Formatting.Indented);
            File.WriteAllText("Assets/Data/AddressableAssetGuids.json", json);
            Debug.Log("Addressable Asset GUIDs exported successfully!");
        }
    }
}
#endif