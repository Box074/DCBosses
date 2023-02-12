using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class CreateAssetBundles
{
    public static Dictionary<string, string> scenes = new Dictionary<string, string>()
    {
        ["queenscene"] = "Assets/Scenes/Queen/QueenScene.unity"
    };
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles/Windows";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        var build = new AssetBundleBuild();
        build.assetBundleName = "queenscene";
        build.assetBundleVariant = "";
        build.addressableNames = new string[1];
        build.assetNames = new string[] {
            "Assets/Scenes/Queen/QueenScene.unity"
        };
        BuildPipeline.BuildAssetBundles(assetBundleDirectory,
                                        new AssetBundleBuild[] { build },
                                        BuildAssetBundleOptions.ChunkBasedCompression,
                                        BuildTarget.StandaloneWindows);
    }
    [MenuItem("Assets/Build All AssetBundles")]
    static void BuildAllAssetBundles()
    {
        string assetBundleDirectory = "Assets/AssetBundles";
        if (!Directory.Exists(assetBundleDirectory))
        {
            Directory.CreateDirectory(assetBundleDirectory);
        }
        var builds = new List<AssetBundleBuild>();
        foreach (var v in scenes)
        {
            var build = new AssetBundleBuild();
            build.assetBundleName = v.Key;
            build.addressableNames = new string[1];
            build.assetNames = new string[] {
                v.Value
            };
            builds.Add(build);
        }
        var platforms = new List<(BuildTarget, string)>()
        {
            (BuildTarget.StandaloneLinux64, "Linux"),
            (BuildTarget.StandaloneOSX, "OSX"),
            (BuildTarget.StandaloneWindows, "Windows")
        };
        foreach (var v in platforms)
        {
            var dir = Path.Combine(assetBundleDirectory, v.Item2);
            Directory.CreateDirectory(dir);
            BuildPipeline.BuildAssetBundles(dir,
                                        builds.ToArray(),
                                        BuildAssetBundleOptions.ChunkBasedCompression,
                                        v.Item1);
        }
    }
}