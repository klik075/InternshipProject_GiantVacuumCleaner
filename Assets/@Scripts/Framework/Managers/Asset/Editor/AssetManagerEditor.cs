#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Scripts.Framework.Managers.Asset.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Scripts.Framework.Managers.Asset.Editor
{
    [CustomEditor(typeof(AssetManager))]
    public class AssetManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            AssetManager assetManager = (AssetManager)target;

            // Create a custom style for the button
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fixedHeight = 40;
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.fontSize = 14;

            // Draw default inspector excluding the Labels field
            DrawPropertiesExcluding(serializedObject, "_labels");

            // Get all asset labels as AssetLabelReferences
            List<AssetLabelReference> allAssetLabelReferences = AssetLabelUtility.GetAllAssetLabels();
            bool labelsMatch = ListsMatch(assetManager.Labels, allAssetLabelReferences);

            // Determine button label and color
            string buttonText;
            Color buttonColor;
            if (labelsMatch)
            {
                buttonText = "Ready to AssetLabels";
                buttonColor = new Color(0.53f, 0.81f, 0.98f); // Sky blue color
            }
            else
            {
                buttonText = "Push the button, Setup AssetLabels";
                buttonColor = new Color(1f, 0.27f, 0.27f); // Red color
            }

            // Set the background color to the determined color
            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = buttonColor;

            if (GUILayout.Button(buttonText, buttonStyle))
            {
                // Assign all asset labels to AssetLabelReferences in AssetManager
                assetManager.Labels = allAssetLabelReferences;

                // Mark the object as dirty to save changes
                EditorUtility.SetDirty(assetManager);
            }

            // Reset the background color to the original color
            GUI.backgroundColor = originalColor;

            // Handle serialization of LabelReferences based on SerializeAssetLabels
            if (assetManager.SerializeAssetLabels)
            {
                SerializedProperty labelsProperty = serializedObject.FindProperty("_labels");
                EditorGUILayout.PropertyField(labelsProperty, true);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private bool ListsMatch(List<AssetLabelReference> list1, List<AssetLabelReference> list2)
        {
            if (list1 == null || list2 == null)
                return false;

            if (list1.Count != list2.Count)
                return false;

            list1.Sort((a, b) 
                => string.Compare(a.labelString, b.labelString, StringComparison.Ordinal));
            list2.Sort((a, b) 
                => string.Compare(a.labelString, b.labelString, StringComparison.Ordinal));

            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i].labelString != list2[i].labelString)
                    return false;
            }

            return true;
        }
    }
}
#endif
