using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {

	private int startMonstersCount;

	[SerializeField]
	private int monstersToUnlock = 0;

	[Header("SFX")]
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip victoryMusic;

	[SerializeField]
	private GameObject victoryPanel;

	private bool victory = false;

	private void Start()
	{
		startMonstersCount = Beam.monsterKills;
	}

	private void Update()
	{
		if (!victory && Beam.monsterKills - startMonstersCount >= monstersToUnlock)
		{
			audioSource.Stop();
			audioSource.clip = victoryMusic;
			audioSource.Play();
			victoryPanel.SetActive(true);
			victory = true;
		}
	}
}
