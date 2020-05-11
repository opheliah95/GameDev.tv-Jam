using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]

public class Dialogue : ScriptableObject
{
    [Tooltip("Speaker Name")]
    public string speakerName;

    [TextArea(3, 10)]
    public string[] sentences;

    public Sprite characterImage;
}
