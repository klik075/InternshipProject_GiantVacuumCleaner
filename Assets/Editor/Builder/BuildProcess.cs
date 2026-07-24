using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;
#if UNITY_IOS

#endif

namespace Editor.Builder
{
    public class BuildProcess : MonoBehaviour
    {
        [MenuItem("Build/Build APK")]
        public static void AndroidAPKBuild()
        {
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;
            EditorUserBuildSettings.buildAppBundle = false;
            AndroidBuildProcess(false);
        }
    
        [MenuItem("Build/Build App Bundle")]
        public static void AndroidBuildProcess()
        {
            EditorUserBuildSettings.androidCreateSymbols = AndroidCreateSymbols.Public;
            EditorUserBuildSettings.buildAppBundle = true;
            AndroidBuildProcess(true);
        }

        private static void AndroidBuildProcess(bool aab)
        {
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = new[] { EditorBuildSettings.scenes[0].path };
            buildPlayerOptions.locationPathName = $"Assets/../build/{BuildConfiguration.PackageName}_{BuildConfiguration.BuildVersion}({BuildConfiguration.AosBundle})." + $"{(aab ? "aab" : "apk")}";
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            if (summary.result == BuildResult.Succeeded) Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
            if (summary.result == BuildResult.Failed) Debug.Log("Build failed");
        }
    }
}
