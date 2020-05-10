using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : MonoBehaviour
{
    public List<Dialogue> dialogues;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().startDialogue(dialogues);
    }
}
