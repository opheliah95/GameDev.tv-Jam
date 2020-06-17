using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    /* Input Action callbacks */
    
    private void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            if (DialogueManager.sentenceEnd)
            {
                DialogueManager.Instance.DisplayNextSentence();
            }
        }
    }

    private void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            if (DialogueManager.sentenceEnd)
            {
                DialogueManager.Instance.DisplayNextSentence();
            }
        }
    }

    private void OnToggleInGameMenu(InputValue value)
    {
        Debug.Assert(value.isPressed, "OnToggleInGameMenu called with value.isPressed: false, " +
                                      "action should only be set for Press, not Release.");
        
        // see FirstPersonController.OnMasterEventStarted comment, we prefer checking flag than switching action map
        if (!GameplayEventManager.Instance.IsEventPlaying())
        {
            InGameMenu.Instance.Toggle();
        }
    }
}
