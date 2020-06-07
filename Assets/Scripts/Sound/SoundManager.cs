using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CommonsPattern;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : SingletonManager<SoundManager>
{

    public void startPlaySound(SoundData soundsToPlay)
    {
        playSound(soundsToPlay);
    }

    protected void playSound(SoundData soundsToPlay)
    {
       
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.LogFormat("{0} sound has stoped", GetComponent<AudioSource>().clip);

        }

        //randomly generate a sound to play
        int index = Random.Range(0, soundsToPlay.sounds.Count - 1);
        audioSource.clip = soundsToPlay.sounds[index];
        Debug.LogFormat("{0} sound is playing...", audioSource.clip);
        // play the sound
        audioSource.Play();

    }

}
