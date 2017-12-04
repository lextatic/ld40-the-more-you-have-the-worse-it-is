using UnityEngine;

public class Blood : MonoBehaviour
{
	Transform target;

	//void Start ()
	//{
	//	player = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
	//}

	public void SetTarget(Transform target)
	{
		this.target = target;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (target)
		{
			transform.Translate(((target.position + target.up) - transform.position).normalized * 8f * Time.deltaTime, Space.World);
		}
		else
		{
			Destroy(this.gameObject);
		}
	}

	private void OnTriggerEnter(Collider col)
	{
		if(col.transform == target)
		{
			Destroy(this.gameObject);

			col.GetComponent<IHealth>().GetOneHealth();

		}
	}

}
