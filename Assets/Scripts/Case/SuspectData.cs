using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityConstants;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "SuspectData", menuName = "ScriptableObjects/Suspect Data")]
public class SuspectData : ScriptableObject
{
    [Tooltip("Identifier name. Must be unique.")]
    public string stringID;

    [Tooltip("Suspect photo")]
    public Texture2D picture;

    [Tooltip("Readable name")]
    public string suspectName;
    
    [Tooltip("Suspect bio, testimony, etc.")]
    public string description;
}