using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CommonsPattern;

public class GameplayEventManager : SingletonManager<GameplayEventManager>
{
    // Events
    public static event Action onMasterEventStarted;
    public static event Action onMasterEventEnded;
    
    /// Number of active master events. Should be 0 or 1, but we prefer a count to a flag in case there are more.
    private int m_ActiveMasterEventsCount = 0;

    /// Return true iff some event is playing (equivalent to checking if a master event is playing)
    public bool IsEventPlaying()
    {
        return m_ActiveMasterEventsCount > 0;
    }
    
    /// Notify an event has started.
    public void NotifyMasterEventStarted()
    {
        // normally we have only one master gameplay event active at a time
        // but in case designer checked isMasterEvent for some slave events too,
        // we count all of them and only notify master event start if the first master event started
        if (m_ActiveMasterEventsCount++ == 0)
        {
            onMasterEventStarted?.Invoke();
        }
        else
        {
            Debug.LogWarningFormat(this,
                "There should be only one active master gameplay event at a time, but we know have {0}",
                m_ActiveMasterEventsCount);
        }
    }
    
    /// Notify an event has ended.
    public void NotifyMasterEventEnded()
    {
        // normally we have only one master gameplay event active at a time
        // but in case designer checked isMasterEvent for some slave events too,
        // we count all of them and only notify master event end if the last master event ended
        if (--m_ActiveMasterEventsCount == 0)
        {
            onMasterEventEnded?.Invoke();
        }
        else
        {
            Debug.LogWarningFormat(this,
                "There should be only one active master gameplay event at a time, but we just had {0}",
                m_ActiveMasterEventsCount + 1);
        }
    }
}
