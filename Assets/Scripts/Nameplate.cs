using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nameplate : MonoBehaviour
{
	[SerializeField]
	private Transform nameplateTarget;

	private IHealth healthComponent;

	[SerializeField]
	private SpriteRenderer healthbarSprite;

	private void Start()
	{
		healthComponent = nameplateTarget.GetComponent<IHealth>();
	}

	private void LateUpdate()
	{
		transform.LookAt(transform.position + Vector3.forward);
		transform.Rotate(Vector3.right, 45f, Space.Self);

		float healthRatio = (float)healthComponent.Health / healthComponent.MaxHealth;

		healthbarSprite.size = new Vector2(healthRatio, 0.1f);

		healthbarSprite.color = Color.Lerp(Color.red, Color.green, healthRatio);

		//if (healthRatio <= 0.1)
		//{
		//	healthbarSprite.color
		//}
		//else if (healthRatio <= 0.4)
		//{
		//	healthbarSprite.color = Color.
		//}
	}
}
