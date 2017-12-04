using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBeam
	: MonoBehaviour {

	[SerializeField]
	private new MeshRenderer renderer;
	Color currentColor;
	Color targetColor;

	[SerializeField]
	Color Color1;
	[SerializeField]
	Color Color2;

	bool color1 = false;

	[SerializeField]
	private float time = 2f;

	[SerializeField]
	private AudioClip beamOff;

	private void Start()
	{
		currentColor = Color1;
		targetColor = Color2;
		
		StartCoroutine(ChangeColor());
	}
	
	private void Update()
	{
		currentColor = Color.Lerp(currentColor, targetColor, time * Time.deltaTime);
		renderer.material.SetColor("_TintColor", currentColor);
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
