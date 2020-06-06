using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName ="ScriptableObject/Sound Data")]
public class SoundData : ScriptableObject
{
    public List<AudioClip> sounds;
}
