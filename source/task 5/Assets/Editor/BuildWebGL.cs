using System;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;

// Command-line WebGL build:
// Unity -batchmode -quit -projectPath "source/task 5" -executeMethod BuildWebGL.Build -buildOutput <dir>
// <dir>/index.html, <dir>/Build and <dir>/TemplateData replace the site's copies in the repo root
public static class BuildWebGL
{
    public static void Build()
    {
        string[] args = Environment.GetCommandLineArgs();
        int outputIndex = Array.IndexOf(args, "-buildOutput");
        string output = outputIndex >= 0 && outputIndex + 1 < args.Length ? args[outputIndex + 1] : "webgl";

        // GitHub Pages can't send Content-Encoding headers for pre-compressed files, so the loader decompresses them itself
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
        PlayerSettings.WebGL.decompressionFallback = true;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray(),
            locationPathName = output,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };
        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
        {
            EditorApplication.Exit(1);
        }
    }
}
