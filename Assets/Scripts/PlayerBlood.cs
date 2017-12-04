using System.Collections;
using UnityEngine;

public class PlayerBlood : MonoBehaviour
{
	private Rigidbody myRigidbody;

	// Use this for initialization
	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody>();

		myRigidbody.AddForce(Random.insideUnitSphere * 3f, ForceMode.VelocityChange);

		StartCoroutine(DelayedDestroy());
	}

	IEnumerator DelayedDestroy()
	{
		yield return new WaitForSeconds(2f);
		Destroy(this.gameObject);
	}
}
