using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;

namespace UnityLib.CI
{
    public class BuildScript
    {
        static string[] SCENES = FindEnabledEditorScenes();

        [MenuItem("CI/Build MacOSX")]
        static void PerformMacOSXBuild()
        {
            SaveVersion();
            GenericBuild(SCENES, "Build/OSX/" + GetProjectName() + ".app", BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
        }

        [MenuItem("CI/Build MacOSX64")]
        static void PerformMacOSX64Build()
        {
            SaveVersion();
            GenericBuild(SCENES, "Build/OSX/" + GetProjectName() + ".app", BuildTarget.StandaloneOSXIntel64, BuildOptions.None);
        }

        [MenuItem("CI/Build Window")]
        static void PerformWindowBuild()
        {
            SaveVersion();
            GenericBuild(SCENES, "Build/Win64/" + GetProjectName() + ".exe", BuildTarget.StandaloneWindows64, BuildOptions.None);
        }

        [MenuItem("CI/Build Android")]
        static void PerformAndroidBuild()
        {
			var dir = "Build/Android/";
			if (!Directory.Exists(dir))
				Directory.CreateDirectory(dir);

            SaveVersion();
			GenericBuild(SCENES, dir + GetProjectName() + ".apk", BuildTarget.Android, BuildOptions.None);
        }

        [MenuItem("CI/Build IOS")]
        static void PerformIOSBuild()
        {
            SaveVersion();
            GenericBuild(SCENES, "Xcode/", BuildTarget.iOS, BuildOptions.None);
        }

        static void SaveVersion()
        {
            if (!Directory.Exists(Application.dataPath + "/Resources"))
                Directory.CreateDirectory(Application.dataPath + "/Resources");

            File.WriteAllText(Application.dataPath + "/Resources/buildInfo.txt", GetGitHash());
            AssetDatabase.Refresh();
        }

        static string GetGitHash()
        {
            Process p = new Process();
            p.StartInfo.FileName = "git";
            p.StartInfo.Arguments = "rev-parse --short HEAD";
            // Pipe the output to itself - we will catch this later
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;

            // Where the script lives
            p.StartInfo.WorkingDirectory = Application.dataPath;
            p.StartInfo.UseShellExecute = false;

            p.Start();
            // Read the output - this will show is a single entry in the console - you could get  fancy and make it log for each line - but thats not why we're here
            var output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();

            return output;
        }

        private static string[] FindEnabledEditorScenes()
        {
            List<string> EditorScenes = new List<string>();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (!scene.enabled) continue;
                EditorScenes.Add(scene.path);
            }
            return EditorScenes.ToArray();
        }

        static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
            string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);
            if (res.Length > 0)
            {
                throw new Exception("BuildPlayer failure: " + res);
            }
        }

        static string GetProjectName()
        {
            var dp = Application.dataPath;
            var s = dp.Split("/"[0]);
            return s[s.Length - 2];
        }
    }
}
