using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Editor.StateMachine.Templates
{
	public static class ScriptTemplates
	{
		private static readonly string Path = "Assets/Editor/StateMachine//Templates";

		[MenuItem("Assets/Create/State Machine/Action Script", false, 0)]
		public static void CreateActionScript() =>
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<DoCreateStateMachineScriptAsset>(),
				"NewActionSO.cs",
				(Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image,
				$"{Path}/StateAction.txt");

		[MenuItem("Assets/Create/State Machine/Condition Script", false, 0)]
		public static void CreateConditionScript() =>
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0,
				ScriptableObject.CreateInstance<DoCreateStateMachineScriptAsset>(),
				"NewConditionSO.cs",
				(Texture2D)EditorGUIUtility.IconContent("cs Script Icon").image,
				$"{Path}/StateCondition.txt");

		/// <summary>
		/// # 비공개 클래스
		///   - 에디터 상에서 액션 및 컨디션 스크립트를 만들기 위한 클래스
		///   - 해당 텍스트 파일을 기반으로 기본 스크립트를 구성함
		///     - Action : StateAction.txt
		///     - Condition : StateCondition.txt
		/// </summary>
		private class DoCreateStateMachineScriptAsset : EndNameEditAction
		{
			public override void Action(int instanceId, string pathName, string resourceFile)
			{
				string text = File.ReadAllText(resourceFile);

				string fileName = System.IO.Path.GetFileName(pathName);
				{
					string newName = fileName.Replace(" ", "");
					if (!newName.Contains("SO"))
						newName = newName.Insert(fileName.Length - 3, "SO");

					pathName = pathName.Replace(fileName, newName);
					fileName = newName;
				}

				string fileNameWithoutExtension = fileName.Substring(0, fileName.Length - 3);
				text = text.Replace("#SCRIPTNAME#", fileNameWithoutExtension);

				string runtimeName = fileNameWithoutExtension.Replace("SO", "");
				text = text.Replace("#RUNTIMENAME#", runtimeName);

				for (int i = runtimeName.Length - 1; i > 0; i--)
					if (char.IsUpper(runtimeName[i]) && char.IsLower(runtimeName[i - 1]))
						runtimeName = runtimeName.Insert(i, " ");

				text = text.Replace("#RUNTIMENAME_WITH_SPACES#", runtimeName);

				string fullPath = System.IO.Path.GetFullPath(pathName);
				var encoding = new UTF8Encoding(true);
				File.WriteAllText(fullPath, text, encoding);
				AssetDatabase.ImportAsset(pathName);
				ProjectWindowUtil.ShowCreatedAsset(AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object)));
			}
		}
	}
}
