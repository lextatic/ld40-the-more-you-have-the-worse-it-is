using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
	[SerializeField]
	GameObject[] tiggerComponents;

	bool triggered = false;

	public IEnumerator OnTriggerEnter(Collider other)
	{
		yield return null;

		if(!triggered && other.CompareTag("Player"))
		{
			foreach(GameObject obj in tiggerComponents)
			{
				obj.SetActive(true);
			}
			triggered = true;
		}
	}
}
