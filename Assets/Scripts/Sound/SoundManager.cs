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
        Debug.LogFormat("{0} sound is playing...",soundsToPlay.name);
        AudioSource audioSource = GetComponent<AudioSource>();
        //stop any audio that is currently playing
        audioSource.Stop();

        //randomly generate a sound to play
        int index = Random.Range(0, soundsToPlay.sounds.Count - 1);
        audioSource.clip = soundsToPlay.sounds[index];

        // play the sound
        audioSource.Play();

    }

}
