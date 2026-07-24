using Scripts.Framework.Modules.UIModules.UI_Elements_Controller;
using UnityEditor;
using UnityEngine;

namespace Scripts.Framework.Modules.UIModules.Editor
{
    [CustomPropertyDrawer(typeof(ContentAction))]
    public class ContentActionDrawer  : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUI.GetPropertyHeight(property.FindPropertyRelative("actionType"))
                           + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("direction"))
                           + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("ease"))
                           + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("duration"))
                           + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("blendBeforeAction"))
                           + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("blendAfterAction"))
                           + EditorGUI.GetPropertyHeight(property.FindPropertyRelative("loop"));

            if (property.FindPropertyRelative("loop").boolValue) height += EditorGUI.GetPropertyHeight(property.FindPropertyRelative("loopAction"));
            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty actionType = property.FindPropertyRelative("actionType");
            SerializedProperty direction = property.FindPropertyRelative("direction");
            SerializedProperty ease = property.FindPropertyRelative("ease");
            SerializedProperty duration = property.FindPropertyRelative("duration");
            SerializedProperty blendBeforeAction = property.FindPropertyRelative("blendBeforeAction");
            SerializedProperty blendAfterAction = property.FindPropertyRelative("blendAfterAction");
            SerializedProperty loop = property.FindPropertyRelative("loop");
            SerializedProperty loopAction = property.FindPropertyRelative("loopAction");

            Rect currentPosition = position;
            currentPosition.height = EditorGUI.GetPropertyHeight(actionType);
            EditorGUI.PropertyField(currentPosition, actionType);

            currentPosition.y += currentPosition.height;
            currentPosition.height = EditorGUI.GetPropertyHeight(direction);
            EditorGUI.PropertyField(currentPosition, direction);

            currentPosition.y += currentPosition.height;
            currentPosition.height = EditorGUI.GetPropertyHeight(ease);
            EditorGUI.PropertyField(currentPosition, ease);

            currentPosition.y += currentPosition.height;
            currentPosition.height = EditorGUI.GetPropertyHeight(duration);
            EditorGUI.PropertyField(currentPosition, duration);

            currentPosition.y += currentPosition.height;
            currentPosition.height = EditorGUI.GetPropertyHeight(blendBeforeAction);
            EditorGUI.PropertyField(currentPosition, blendBeforeAction);

            currentPosition.y += currentPosition.height;
            currentPosition.height = EditorGUI.GetPropertyHeight(blendAfterAction);
            EditorGUI.PropertyField(currentPosition, blendAfterAction);

            currentPosition.y += currentPosition.height;
            currentPosition.height = EditorGUI.GetPropertyHeight(loop);
            EditorGUI.PropertyField(currentPosition, loop);

            if (!loop.boolValue) return;
            currentPosition.y += currentPosition.height;
            currentPosition.height = EditorGUI.GetPropertyHeight(loopAction);
            EditorGUI.PropertyField(currentPosition, loopAction);
        }
    }
}