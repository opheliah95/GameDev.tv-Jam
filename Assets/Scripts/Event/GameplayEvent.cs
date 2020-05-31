using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameplayEvent : MonoBehaviour
{
    /// Event triggered on execution end
    public event Action end;

    /// Has this event been started as a master event?
    private bool m_IsExecutedAsMaster = false;

    /// Execute and notify manager that a new master event started (e.g. to prevent further player interactions)
    public void ExecuteAsMaster()
    {
        m_IsExecutedAsMaster = true;
        GameplayEventManager.Instance.NotifyMasterEventStarted();
        Execute();
    }
    
    /// Execute without notifying manager
    public void ExecuteAsSlave()
    {
        Execute();
    }
    
    /// Core execute callback
    protected abstract void Execute();

    /// Notify current event as ended.
    /// Make sure to call this at the end of every child GameplayEvent, even if it's just at the end of Execute.
    protected void End()
    {
        // if this event was started as master, notify its end as master as well (e.g. to resume player interactions)
        if (m_IsExecutedAsMaster)
        {
            GameplayEventManager.Instance.NotifyMasterEventEnded();
        }
        
        // notify observers of this specific event
        end?.Invoke();
    }
}
