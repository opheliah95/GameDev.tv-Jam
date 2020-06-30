using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityConstants;

/// Case data
[CreateAssetMenu(fileName = "CaseData", menuName = "ScriptableObjects/Case Data")]
public class CaseData : ScriptableObject
{
    [Tooltip("Readable title for the case")]
    public string title = "Case Title";

    [Tooltip("Array of navigatable real world scene data. Make sure that at least one of them is unlocked")]
    public SceneData[] realSceneDataArray;
    
    [Tooltip("Array of navigatable reconstruction scene data. Make sure that at least one of them is unlocked")]
    public SceneData[] reconstructionSceneDataArray;
    
    [Tooltip("Actual scene where victim died, must be found by player")]
    public SceneData deathLocationSceneData;
    
    [Tooltip("Clues that can be found by player")]
    public ClueData[] clueDataArray;
    
    [Tooltip("Weapon used to kill the victim, must be found by player. Should also be in clueDataArray.")]
    public ClueData murderWeaponClueData;
}
