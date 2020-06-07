using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEvent : GameplayEvent
{
    [Tooltip("Sound Data associated with the events")]
    public SoundData soundsToPlay;

    protected override void Execute()
    {
        SoundManager.Instance.startPlaySound(soundsToPlay);
        End();
    }
}
