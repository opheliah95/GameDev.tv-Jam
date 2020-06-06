using UnityConstants;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UnlockSceneEvent : GameplayEvent
{
    [Tooltip("Scene to unlock")]
    public ScenesEnum sceneEnum;

    [Tooltip("Type of scene to unlock")]
    public CaseSceneType sceneType;

    // a list of sound to play when unlock objects
    public SoundData unlockSound;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    protected override void Execute()
    {
        playSound();
        CaseManager.Instance.CurrentCaseProgress.UnlockScene(sceneType, sceneEnum);
        End();
    }

    protected void playSound()
    {
        //stop any audio that is currently playing
        audioSource.Stop();

        //randomly generate a sound to play
        int index = Random.Range(0, unlockSound.sounds.Count - 1);
        audioSource.clip = unlockSound.sounds[index];

        // play the sound
        audioSource.Play();
        
    }
}
