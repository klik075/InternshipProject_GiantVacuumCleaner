using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Editor.Builder
{
    public class BuildPreProcess : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.bundleVersion = BuildConfiguration.BuildVersion;
            if (report.summary.platform == BuildTarget.Android)
            {
                PlayerSettings.Android.keystoreName = Application.dataPath + "/../" + $"{BuildConfiguration.PackageName}.keystore";
                PlayerSettings.Android.keystorePass = "ActFit0304!";
                PlayerSettings.Android.keyaliasName = BuildConfiguration.PackageName.ToLower();
                PlayerSettings.Android.keyaliasPass = "ActFit0304!";
                PlayerSettings.Android.minifyRelease = true;
                PlayerSettings.Android.bundleVersionCode = BuildConfiguration.AosBundle;
            }
            else if (report.summary.platform == BuildTarget.iOS)
            {
                PlayerSettings.iOS.buildNumber = BuildConfiguration.IOSBundle;
            }
        }
    }
}