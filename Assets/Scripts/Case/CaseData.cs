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
        public ScenesEnum sceneID;

        [Tooltip("Readable scene name")]
        public string name;
    }
    
    [Tooltip("Readable title for the case")]
    public string title = "Case Title";

    [Tooltip("Array of navigatable scene data. The first one is the default one and unlocked, others start locked.")]
    public SceneData[] sceneDataArray;
}
