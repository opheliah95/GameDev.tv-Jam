using System.Collections;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    /// footstep sound
    public SoundData footstep;
    AudioSource audioSource;
    IEnumerator footstepCoroutine;
   

    [Tooltip("This varaiable controls the length of the sound")]
    public float soundLength = 0.8f;

    [Tooltip("This varaiable controls how fast the track should be playing, the bigger the number, the slower it is")]
    public float soundInterval = 3f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        footstepCoroutine = playSoundWithInterval(footstep);
    }

    private void Start()
    {
        StartCoroutine(footstepCoroutine);
    }

    // function to play player related sound
    protected void playSound(SoundData soundsToPlay)
    {
        //randomly generate a sound to play
        int index = Random.Range(0, soundsToPlay.sounds.Count - 1);
        audioSource.volume = Random.Range(0.8f, 1.1f);
        // slow the pitch down randomly
        audioSource.pitch = Random.Range(0.7f, 0.9f);
        audioSource.clip = soundsToPlay.sounds[index];
        // play the sound
        audioSource.Play();
    }


    /// <summary>
    /// This function plays sound till the time you specifies, i.e. soundLength
    /// Then it slowly fades out basing on soundInterval
    /// </summary>
    protected IEnumerator playSoundWithInterval(SoundData soundsToPlay)
    {
        while (true)
        {
            if (!audioSource.isPlaying && FirstPersonController.isPlayerMoving)
            {
                playSound(soundsToPlay);
                // now sound can gradually fades out after certain amount of time
                yield return new WaitForSeconds(soundLength);

                //Synchronous Coroutines
                yield return StartCoroutine(fadeOutSound(soundInterval));

            }
            else
            {
                if (!FirstPersonController.isPlayerMoving)
                {
                    StopCoroutine(fadeOutSound(soundInterval));
                }
                yield return null;
            }


        }

    }

    /// <summary>
    /// Fades the sound basing on fadeInterval
    /// </summary>
    /// <param name="fadeInterval">How fast the fade out will be, the small the number, the faster it will cut off</param>
    IEnumerator fadeOutSound(float fadeInterval = 2f)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0 && FirstPersonController.isPlayerMoving && audioSource.isPlaying)
        {
            audioSource.volume -= startVolume * Time.deltaTime / fadeInterval;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

}
