using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public List<Dialogue> dialogues;

	public void TriggerDialogue ()
	{
        FindObjectOfType<DialogueManager>().startDialogue(dialogues);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !FirstPersonController.isTalking)
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            FindObjectOfType<DialogueManager>().EndDialogue();
            FirstPersonController.isTalking = false;
        }
    }
    
}
