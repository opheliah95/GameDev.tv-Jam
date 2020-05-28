using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayEvent : MonoBehaviour
{
    /// EventTrigger executing this event
    protected GameplayEventSequence masterSequence = null;

    /// Set master event sequence. Should be called on EventSequence init.
    /// If this event is not part of a sequence, it remains null.
    public void RegisterMasterSequence(GameplayEventSequence eventSequence) {
        masterSequence = eventSequence;
    }

    public abstract void Execute();
}
