using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meat : MonoBehaviour
{
	[SerializeField]
	private Blood bloodPrefab;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	void Update ()
	{
		transform.Rotate(0, 25 * Time.deltaTime, 0);
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			for (int i = 0; i < 5; i++)
			{
				Blood blood = Instantiate(bloodPrefab, transform.position + transform.up + (Random.insideUnitSphere * 1), transform.rotation);
				blood.SetTarget(other.transform);
			}

			Destroy(this.gameObject);
		}
	}
}
