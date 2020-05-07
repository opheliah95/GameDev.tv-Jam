using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueUIController : MonoBehaviour
{
   

    // press any key to continue
    private void Update()
    {
        var action = new InputAction(type: InputActionType.PassThrough, binding: "*/<Button>");

        if (Keyboard.current.anyKey.wasPressedThisFrame && DialogueManager.sentenceEnd)
        {
            FindObjectOfType<DialogueManager>().DisplayNextSentence();
        }
    }

    
}
