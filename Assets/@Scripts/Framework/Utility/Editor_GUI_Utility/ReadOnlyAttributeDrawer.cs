
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

/// <summary>
/// 실제 읽기 불능 에디터 처리를 하는 프로퍼티 드로어
/// </summary>
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;    // Disable the property field
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;     // Re-enable GUI (optional)
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif