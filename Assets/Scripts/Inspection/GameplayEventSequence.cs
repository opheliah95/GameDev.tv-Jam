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
    
    public override void Execute()
    {
        m_CurrentEventIndex = 0;
        TryExecuteEventAtCurrentIndex();
    }
    
    /// Continue sequence, called from slave event on end
    public void Continue () {
        ++m_CurrentEventIndex;
        TryExecuteEventAtCurrentIndex();
    }

    private void TryExecuteEventAtCurrentIndex()
    {
        if (m_CurrentEventIndex < eventSequence.Count) {
            GameplayEvent gameplayEvent = eventSequence[m_CurrentEventIndex];
            // we will continue sequence as soon as this event ends
            // register end callback now in case Execute immediately ends
            gameplayEvent.end += OnEventEnd;
            // actually execute the next event
            gameplayEvent.Execute();
        } else {
            Debug.LogFormat(this, "Event sequence {0} over after {1} events", this, eventSequence.Count);
        }
    }

    // Callback for individual event ending in the sequence
    private void OnEventEnd()
    {
        // unsubscribe now to avoid duplicate End signal on this event if this sequence is replayed (one-time event)
        GameplayEvent gameplayEvent = eventSequence[m_CurrentEventIndex];
        gameplayEvent.end -= OnEventEnd;
        
        Continue();
    }
}
