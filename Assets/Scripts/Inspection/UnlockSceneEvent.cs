using System.Collections;
using System.Collections.Generic;
using UnityConstants;
using UnityEngine;

public class UnlockSceneEvent : GameplayEvent
{
    [Tooltip("Scene to unlock")]
    public ScenesEnum sceneEnum;

    [Tooltip("Type of scene to unlock")]
    public CaseSceneType sceneType;

    public override void Execute()
    {
        CaseManager.Instance.UnlockScene(sceneType, sceneEnum);
        End();
    }
}
