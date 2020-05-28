using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayEventSequence : GameplayEvent
{
    /* Event references (often placed on same game object) */
    
    [Tooltip("Sequence of events to execute")]
    public List<GameplayEvent> eventSequence;
    
    /* State */
    
    /// Current event index
    int m_CurrentEventIndex;
    
    void Awake () {
        // reverse reference this for each event, to allow access later
        // to notify this of advancing sequence on event end
        foreach (var gameplayEvent in eventSequence) {
            gameplayEvent.RegisterMasterSequence(this);
        }
    }

    public override void Execute()
    {
        m_CurrentEventIndex = 0;
        ExecuteEventAtCurrentIndex();
    }
    
    /// Continue sequence, called from slave event on end
    public void Continue () {
        ++m_CurrentEventIndex;
        ExecuteEventAtCurrentIndex();
    }

    private void ExecuteEventAtCurrentIndex()
    {
        if (m_CurrentEventIndex < eventSequence.Count) {
            eventSequence[m_CurrentEventIndex].Execute();
        } else {
            Debug.LogFormat(this, "Event sequence {0} over after {1} events", this, eventSequence.Count);
        }
    }
}
