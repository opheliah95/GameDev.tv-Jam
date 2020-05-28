using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public List<Dialogue> dialogues;

	public void TriggerDialogue ()
	{
        DialogueManager.Instance.startDialogue(dialogues);
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
            DialogueManager.Instance.EndDialogue();
            FirstPersonController.isTalking = false;
        }
    }
    
}
