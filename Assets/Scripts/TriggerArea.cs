using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
	[SerializeField]
	GameObject[] tiggerComponents;

	bool triggered = false;

	public void OnTriggerEnter(Collider other)
	{
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
