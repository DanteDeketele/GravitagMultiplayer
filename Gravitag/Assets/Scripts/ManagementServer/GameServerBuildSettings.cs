#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "GameServerBuildSettings", menuName = "Custom/GameServerBuildSettings")]
public class GameServerBuildSettings : ScriptableObject
{
    public SceneAsset[] serverScenes;
}
#endif