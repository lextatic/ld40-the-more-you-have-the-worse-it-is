using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private Transform target;
	private Vector3 difference;

	private void Start()
	{
		difference = transform.position - target.position;
	}

	void LateUpdate ()
	{	
		if(target)
		transform.position = Vector3.Lerp(transform.position, target.position + difference, Time.deltaTime * 2f);
	}
}
