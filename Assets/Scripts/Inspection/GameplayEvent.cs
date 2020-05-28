using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayEvent : MonoBehaviour
{
    /// Event triggered on execution end
    public event Action end;

    public abstract void Execute();

    /// Notify current event as ended.
    /// Make sure to call this at the end of every child GameplayEvent, even if it's just at the end of Execute.
    protected void End()
    {
        end?.Invoke();
    }
}
