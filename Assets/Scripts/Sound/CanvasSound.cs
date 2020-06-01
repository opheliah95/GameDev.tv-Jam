using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CanvasSound : MonoBehaviour
{
	AudioSource audioSource;
	bool initialized;

	void Awake()
	{
		audioSource = GetComponent<AudioSource>();

	}

	void OnEnable()
	{
		audioSource.Play();
	}
}
