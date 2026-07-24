using _Scripts.Frameworks.Modules.StateMachine.Core.Modular;
using Editor.Core;
using UnityEditor;
using UnityEngine;

namespace Editor.AnimationParameter
{
    [CustomEditor(typeof(AnimationParameterActionSO)), CanEditMultipleObjects]
    public class AnimatorParameterActionSOEditor : CustomEditorBase
    {
        public override void OnInspectorGUI()
        {
            base.DrawNonEditableScriptReference<AnimationParameterActionSO>();

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("_description"));
            EditorGUILayout.Space();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("WhenToRun"));
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Animator Parameter", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("_parameterName"), new GUIContent("Name"));

            // Draws the appropriate value depending on the type of parameter this SO is going to change on the Animator
            SerializedProperty animParamValue = serializedObject.FindProperty("_parameterType");

            EditorGUILayout.PropertyField(animParamValue, new GUIContent("Type"));

            switch (animParamValue.intValue)
            {
                case (int)AnimationParameterActionSO.ParameterType.Bool:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("BoolValue"), new GUIContent("Desired value"));
                    break;
                case (int)AnimationParameterActionSO.ParameterType.Int:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("IntValue"), new GUIContent("Desired value"));
                    break;
                case (int)AnimationParameterActionSO.ParameterType.Float:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("FloatValue"), new GUIContent("Desired value"));
                    break;

            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}