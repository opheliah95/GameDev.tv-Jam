using UnityConstants;
using UnityEngine;

public class FindClueEvent : GameplayEvent
{
    [Tooltip("Clue string ID")]
    public string clueStringID;

    [Tooltip("Sound Data associated with the events")]
    public SoundData soundsToPlay;

    protected override void Execute()
    {
        SoundManager.Instance.startPlaySound(soundsToPlay); // play sound associated with clues
        CaseManager.Instance.CurrentCaseProgress.FindClue(clueStringID);
        End();
    }
}
