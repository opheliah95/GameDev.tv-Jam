using UnityConstants;
using UnityEngine;

public class UnlockSceneEvent : GameplayEvent
{
    [Tooltip("Scene to unlock")]
    public ScenesEnum sceneEnum;

    [Tooltip("Type of scene to unlock")]
    public CaseSceneType sceneType;

    [Tooltip("Play a sound out of the sound data group")]
    public SoundData soundsToPlay;
    protected override void Execute()
    {
        SoundManager.Instance.startPlaySound(soundsToPlay);
        CaseManager.Instance.CurrentCaseProgress.UnlockScene(sceneType, sceneEnum);
        End();
    }
}
