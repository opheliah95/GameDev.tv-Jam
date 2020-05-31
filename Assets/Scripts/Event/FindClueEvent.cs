using UnityConstants;
using UnityEngine;

public class FindClueEvent : GameplayEvent
{
    [Tooltip("Clue string ID")]
    public string clueStringID;

    protected override void Execute()
    {
        CaseManager.Instance.CurrentCaseProgress.FindClue(clueStringID);
        End();
    }
}
