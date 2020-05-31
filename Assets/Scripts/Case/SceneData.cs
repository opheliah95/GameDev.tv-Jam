using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityConstants;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SceneData", menuName = "ScriptableObjects/Scene Data")]
public class SceneData : ScriptableObject
{
    [Tooltip("Scene ID")]
    public ScenesEnum sceneEnum;

    [Tooltip("Readable scene name")]
    public string sceneName;

    [Tooltip("Is the scene unlocked on case start?")]
    public bool unlockedOnStart;
}