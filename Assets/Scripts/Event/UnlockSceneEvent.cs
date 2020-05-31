using UnityConstants;
using UnityEngine;

public class UnlockSceneEvent : GameplayEvent
{
    [Tooltip("Scene to unlock")]
    public ScenesEnum sceneEnum;

    [Tooltip("Type of scene to unlock")]
    public CaseSceneType sceneType;

    protected override void Execute()
    {
        CaseManager.Instance.CurrentCaseProgress.UnlockScene(sceneType, sceneEnum);
        End();
    }
}
