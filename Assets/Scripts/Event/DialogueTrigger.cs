using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour {

	public List<Dialogue> dialogues;

	private void TriggerDialogue ()
	{
        Debug.LogWarning("TriggerDialogue is obsolete, use Trigger + DialogueEvent to make sure " +
                         "Player controls are updated properly during event sequence.");
        DialogueManager.Instance.StartDialogue(dialogues);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !FirstPersonController.isTalking)
        {
            TriggerDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DialogueManager.Instance.EndDialogue();
            FirstPersonController.isTalking = false;
        }
    }
    
}
