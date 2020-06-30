using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityConstants;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ClueData", menuName = "ScriptableObjects/Clue Data")]
public class ClueData : ScriptableObject
{
    [Tooltip("Identifier name. Must be unique.")]
    public string stringID;

    [Tooltip("Picture")]
    public Texture2D picture;

    [Tooltip("Readable name")]
    public string clueName;
    
    [Tooltip("Clue description")]
    public string description;
}