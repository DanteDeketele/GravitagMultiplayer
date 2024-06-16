#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System;

public class GameServerBuildWindow : EditorWindow
{
    private string buildPath = "C:\\GravitagMultiplayer\\GravitagManagementServer\\GravitagManagementServer\\GameServer\\Build.exe";
    private GameServerBuildSettings buildSettings;

    [MenuItem("Build/Build Game Server")]
    public static void ShowWindow()
    {
        GetWindow<GameServerBuildWindow>("Build Game Server");
    }

    void OnEnable()
    {
        buildSettings = Resources.Load<GameServerBuildSettings>("GameServerBuildSettings");
        if (buildSettings == null)
        {
            Debug.LogError("GameServerBuildSettings not found! Create one via Assets > Create > Custom > GameServerBuildSettings.");
        }
    }

    void OnGUI()
    {
        GUILayout.Label("Build Game Server", EditorStyles.boldLabel);

        GUILayout.Space(10);

        GUILayout.Label("Build Path (Relative to Project)", EditorStyles.label);
        buildPath = EditorGUILayout.TextField(buildPath);

        GUILayout.Space(10);

        if (buildSettings != null)
        {
            GUILayout.Label("Server Scenes", EditorStyles.label);
            for (int i = 0; i < buildSettings.serverScenes.Length; i++)
            {
                buildSettings.serverScenes[i] = (SceneAsset)EditorGUILayout.ObjectField($"Scene {i + 1}", buildSettings.serverScenes[i], typeof(SceneAsset), false);
            }
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Build"))
        {
            BuildGameServer();
        }
    }

    void BuildGameServer()
    {
        if (buildSettings == null || buildSettings.serverScenes.Length == 0)
        {
            Debug.LogError("No server scenes selected! Add scenes to GameServerBuildSettings.");
            return;
        }

        string buildFullPath = buildPath;
        buildFullPath = Path.GetFullPath(buildFullPath);

        // Check if a build already exists
        if (File.Exists(buildFullPath))
        {
            if (!EditorUtility.DisplayDialog("Build Exists",
                "A build already exists at the specified location. Do you want to overwrite it?",
                "Yes", "No"))
            {
                return;
            }
        }

        string[] scenes = new string[buildSettings.serverScenes.Length];
        for (int i = 0; i < buildSettings.serverScenes.Length; i++)
        {
            scenes[i] = AssetDatabase.GetAssetPath(buildSettings.serverScenes[i]);
        }

        // Ensure the build directory exists
        if (!Directory.Exists(Path.GetDirectoryName(buildFullPath)))
        {
            if (!EditorUtility.DisplayDialog("Build Exists",
                "The specified build directory does not exist. Do you want to create it?\n" +
                "Folder: " + Path.GetDirectoryName(buildFullPath),
                "Yes", "No"))
            {
                return;
            }
            else
            {
                Directory.CreateDirectory(Path.GetDirectoryName(buildFullPath));
            }
        }

        // Add the define for GAMESERVER
        string previousDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        string newDefines = previousDefines + ";GAMESERVER";
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, newDefines);

        // Build player options
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = buildFullPath,
            target = EditorUserBuildSettings.activeBuildTarget, // Use the currently selected build target in Unity
            options = BuildOptions.EnableHeadlessMode
        };

        // Build the player
        BuildPipeline.BuildPlayer(buildPlayerOptions);

        // Remove the define after the build is completed
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, previousDefines);

        Debug.Log("Dedicated server build completed.");
    }

}
#endif