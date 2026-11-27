using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public static class PortfolioWebBuild
{
    [MenuItem("Tools/Portfolio/Build Web Demo")]
    public static void Build()
    {
        const string fallbackDirectory = "Build/PortfolioWeb";
        string output = Environment.GetEnvironmentVariable("PORTFOLIO_WEB_OUTPUT");
        if (string.IsNullOrWhiteSpace(output))
        {
            output = fallbackDirectory;
        }

        var scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .OrderByDescending(path => path == "Assets/Scenes/Dashboard.unity")
            .ToArray();
        if (scenes.Length == 0)
        {
            throw new BuildFailedException("Enable at least one scene in Build Settings.");
        }

        Directory.CreateDirectory(output);
        var originalCompression = PlayerSettings.WebGL.compressionFormat;
        bool originalFallback = PlayerSettings.WebGL.decompressionFallback;
        bool originalDataCaching = PlayerSettings.WebGL.dataCaching;
        try
        {
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
            PlayerSettings.WebGL.decompressionFallback = true;
            // The portfolio's script-only iframe cannot access the browser cache.
            PlayerSettings.WebGL.dataCaching = false;
            var report = BuildPipeline.BuildPlayer(scenes, output, BuildTarget.WebGL, BuildOptions.None);
            if (report.summary.result != BuildResult.Succeeded)
            {
                throw new BuildFailedException($"WebGL build ended with {report.summary.result}.");
            }
        }
        finally
        {
            PlayerSettings.WebGL.compressionFormat = originalCompression;
            PlayerSettings.WebGL.decompressionFallback = originalFallback;
            PlayerSettings.WebGL.dataCaching = originalDataCaching;
        }
    }
}
