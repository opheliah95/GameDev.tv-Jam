using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    [Tooltip("In Game Menu")]
    public InGameMenu inGameMenu;
    
    private void OnSubmit(InputValue value)
    {
        if (value.isPressed)
        {
            if (DialogueManager.sentenceEnd)
            {
                // BAD, make DialogueManager a singleton or save reference on Start
                FindObjectOfType<DialogueManager>().DisplayNextSentence();
            }
        }
    }

    private void OnToggleInGameMenu()
    {
        inGameMenu.Toggle();
    }
    
    private void OnShowOptions()
    {
        
    }
}
