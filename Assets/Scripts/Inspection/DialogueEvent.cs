using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : GameplayEvent
{
    [Tooltip("Dialogue sequence played on interaction")]
    public List<Dialogue> dialogues;

    public override void Execute()
    {
        DialogueManager.Instance.startDialogue(dialogues);
    }
}
