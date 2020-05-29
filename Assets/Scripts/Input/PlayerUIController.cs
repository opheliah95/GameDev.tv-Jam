using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIController : MonoBehaviour
{
    [Tooltip("In Game Menu")]
    public InGameMenu inGameMenu;
    
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
        inGameMenu.Toggle();
    }
}
