using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Gameplay event to execute on Interact")]
    public GameplayEvent targetEvent;

    public void Interact()
    {
        targetEvent.Execute();
    }

}

