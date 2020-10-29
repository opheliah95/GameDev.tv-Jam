using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonsPattern;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonManager<SoundManager>
{
    [SerializeField]
    AudioSource audioSource;

    public float playbackPercent = 10f;
    public void startPlaySound(SoundData soundsToPlay)
    {
        playSound(soundsToPlay);
    }

    protected void playSound(SoundData soundsToPlay)
    {
        audioSource = GetComponent<AudioSource>();

        // assign the next sound to subsequent audio source
        // if the sound is still playing or it hasn't gone through 1/10 of original length
        // if the sound has gone through 1/3 of original length, just overwrite it with a new sound to play
        if (audioSource.isPlaying && audioSource.time <= audioSource.clip.length/playbackPercent)
            audioSource = transform.GetChild(0).GetComponent<AudioSource>();


        //randomly generate a sound to play
        int index = Random.Range(0, soundsToPlay.sounds.Count - 1);
        audioSource.clip = soundsToPlay.sounds[index];
      
        // play the sound
        audioSource.Play();

    }

   
    

}
