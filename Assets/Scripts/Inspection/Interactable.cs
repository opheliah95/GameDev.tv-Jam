using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Dialogue sequence played on interaction")]
    public List<Dialogue> dialogues;

    public void Interact()
    {
        DialogueManager.Instance.startDialogue(dialogues);
    }

}

