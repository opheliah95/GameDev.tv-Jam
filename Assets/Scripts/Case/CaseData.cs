using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityConstants;

/// Case data
[CreateAssetMenu(fileName = "CaseData", menuName = "ScriptableObjects/Case Data")]
public class CaseData : ScriptableObject
{
    [System.Serializable]
    public struct SceneData
    {
        [Tooltip("Scene ID")]
        public ScenesEnum sceneEnum;

        [Tooltip("Readable scene name")]
        public string name;

        [Tooltip("Is the scene unlocked on case start?")]
        public bool unlockedOnStart;
    }
    
    [Tooltip("Readable title for the case")]
    public string title = "Case Title";

    [Tooltip("Array of navigatable real world scene data. Make sure that at least one of them is unlocked")]
    public SceneData[] realSceneDataArray;
    
    [Tooltip("Array of navigatable reconstruction scene data. Make sure that at least one of them is unlocked")]
    public SceneData[] reconstructionSceneDataArray;
}
