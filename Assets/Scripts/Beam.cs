using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour {

	[SerializeField]
	private new MeshRenderer renderer;
	Color currentColor;
	Color targetColor;

	[SerializeField]
	Color Color1;
	[SerializeField]
	Color Color2;


	bool color1 = false;

	public static int monsterKills = 0;

	[SerializeField]
	private bool roomOnly = false;

	private int startMonstersCount;

	[SerializeField]
	private int monstersToUnlock = 0;

	[SerializeField]
	private float time = 2f;

	[Header("SFX")]
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip beamOff;

	private void Start()
	{
		currentColor = Color1;
		targetColor = Color2;
		
		StartCoroutine(ChangeColor());

		if(roomOnly)
		{
			startMonstersCount = monsterKills;
		}
	}
	
	private void Update()
	{
		currentColor = Color.Lerp(currentColor, targetColor, time * Time.deltaTime);
		renderer.material.SetColor("_TintColor", currentColor);

		if(roomOnly)
		{
			if (monsterKills - startMonstersCount >= monstersToUnlock)
			{
				audioSource.PlayOneShot(beamOff);
				audioSource.transform.parent = null;
				gameObject.SetActive(false);
			}
		}
		else if(monsterKills >= monstersToUnlock)
		{
			audioSource.PlayOneShot(beamOff);
			audioSource.transform.parent = null;
			gameObject.SetActive(false);
		}
	}

	IEnumerator ChangeColor()
	{
		yield return new WaitForSeconds(time);
		
		if (color1)
		{
			targetColor = Color2;
		}
		else
		{
			targetColor = Color1;
		}
		color1 = !color1;

		StartCoroutine(ChangeColor());
	}
}
