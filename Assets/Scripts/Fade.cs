using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
	[SerializeField]
	private new MeshRenderer renderer;
	Color currentColor;
	Color targetColor;

	[SerializeField]
	private bool destroy = true;

	public void FadeFrom(Color color)
	{
		currentColor = color;
		renderer.material.SetColor("_TintColor", currentColor);
		targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
	}

	private void Start()
	{
		currentColor = renderer.material.GetColor("_TintColor");
		targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, 0f);
		if (destroy)
		{
			StartCoroutine(DestroyLate());
		}
	}

	private void Update()
	{
		currentColor = Color.Lerp(currentColor, targetColor, .1f);
		renderer.material.SetColor("_TintColor", currentColor);
	}

	IEnumerator DestroyLate()
	{
		yield return new WaitForSeconds(1f);
		Destroy(transform.parent.gameObject);
	}
}
