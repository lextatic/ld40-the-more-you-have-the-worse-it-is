using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
	[SerializeField]
	private GameObject finalGate;

	[SerializeField]
	GameObject[] tiggerComponents;

	[SerializeField]
	Blood bloodPrefab;

	[SerializeField]
	private bool isSuperMeat = false;

	void Update()
	{
		transform.Rotate(0, 25 * Time.deltaTime, 0);
	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.CompareTag("Player"))
		{
			finalGate.SetActive(false);

			foreach (GameObject obj in tiggerComponents)
			{
				obj.SetActive(true);
			}

			if(isSuperMeat)
			{
				for (int i = 0; i < 10; i++)
				{
					Blood blood = Instantiate(bloodPrefab, transform.position + transform.up + (Random.insideUnitSphere * 1), transform.rotation);
					blood.SetTarget(other.transform);
				}
			}

			Destroy(this.gameObject);
		}
	}
}
