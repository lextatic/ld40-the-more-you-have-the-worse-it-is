using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{
	[SerializeField]
	AudioSource musicSource;

	[SerializeField]
	AudioClip music;

	// Use this for initialization
	void Start ()
	{
		musicSource.Stop();
		musicSource.clip = music;
		musicSource.Play();
	}
}
