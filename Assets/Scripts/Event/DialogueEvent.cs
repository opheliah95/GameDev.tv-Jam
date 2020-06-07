using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : GameplayEvent
{
    [Tooltip("Dialogue sequence played on interaction")]
    public List<Dialogue> dialogues;

    public SoundData soundData;
    protected override void Execute()
    {
        // play sound if there is any
        if (soundData != null)
            SoundManager.Instance.startPlaySound(soundData);
        // register callback on dialogue end so we can notify this event as ended
        DialogueManager.onDialogueEnded += OnDialogueEnded;
        
        DialogueManager.Instance.StartDialogue(dialogues);
    }

    private void OnDialogueEnded()
    {
        // unsubscribe now to avoid duplicate End signal on next dialogue (one-time event)
        DialogueManager.onDialogueEnded -= OnDialogueEnded;
        
        End();
    }
}
