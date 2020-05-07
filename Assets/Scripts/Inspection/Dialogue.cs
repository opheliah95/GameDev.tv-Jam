using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogue", order = 1)]

public class Dialogue : ScriptableObject
{
    public string name;

    [TextArea(3, 10)]
    public string[] sentences;

    public Sprite characterImage;


}
