
using UnityEngine;
using UnityEditor;

public class GameBuilder : MonoBehaviour
{

    [MenuItem ("Build/iOS")]
    public static void BuildiOS ()
    {
        Build (BuildTarget.iOS);
    }

    [MenuItem ("Build/Android")]
    public static void BuildAndroid ()
    {
        Build (BuildTarget.Android);
    }

    [MenuItem ("Build/WebGL")]
    public static void BuildWebGL ()
    {
        Build (BuildTarget.WebGL);
    }

    [MenuItem ("Build/Windows (32-bit)")]
    public static void BuildWindows32 ()
    {
        Build (BuildTarget.StandaloneWindows);
    }

    [MenuItem ("Build/Windows (64-bit)")]
    public static void BuildWindows64 ()
    {
        Build (BuildTarget.StandaloneWindows64);
    }


    public static void Build (BuildTarget target)
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions ();

        buildPlayerOptions.scenes = new[] {
            "Assets/Scenes/Menus/Main.unity",
            "Assets/Scenes/Level/1.unity"
        };
        buildPlayerOptions.locationPathName = "Builds/" + target.ToString () + "/CyberAttack";

        if (target == BuildTarget.StandaloneWindows || target == BuildTarget.StandaloneWindows64) {
            buildPlayerOptions.locationPathName += ".exe";
        }
        buildPlayerOptions.target = target;
        buildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer (buildPlayerOptions);
    }
}
