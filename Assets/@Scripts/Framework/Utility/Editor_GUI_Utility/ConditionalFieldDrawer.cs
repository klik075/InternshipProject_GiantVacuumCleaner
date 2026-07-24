#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalFieldAttribute))]
public class ConditionalFieldDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;

        bool shouldShow = ShouldShowProperty(property, conditional);

        if (shouldShow)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalFieldAttribute conditional = (ConditionalFieldAttribute)attribute;

        bool shouldShow = ShouldShowProperty(property, conditional);

        if (shouldShow)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }

        return 0f;
    }

    private bool ShouldShowProperty(SerializedProperty property, ConditionalFieldAttribute conditional)
    {
        foreach (var conditionField in conditional.ConditionFields)
        {
            SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionField);

            if (conditionProperty == null)
            {
                Debug.LogWarning($"Cannot find property with name '{conditionField}' in object '{property.serializedObject.targetObject}'.");
                return false;
            }

            bool shouldShow = false;

            switch (conditionProperty.propertyType)
            {
                case SerializedPropertyType.Boolean:
                    shouldShow = conditional.ConditionValues.Contains(conditionProperty.boolValue ? 1 : 0);
                    break;
                case SerializedPropertyType.Enum:
                    shouldShow = conditional.ConditionValues.Contains(conditionProperty.enumValueIndex);
                    break;
                default:
                    Debug.LogWarning($"ConditionalField does not support '{conditionProperty.propertyType}' yet.");
                    break;
            }

            if (!shouldShow)
            {
                return false;
            }
        }

        return true;
    }
}
#endif