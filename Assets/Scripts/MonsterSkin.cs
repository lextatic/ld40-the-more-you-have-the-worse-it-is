using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSkin : MonoBehaviour
{

	[SerializeField]
	private Enemy myEnemyController;

	public void PlayStepSound()
	{
		myEnemyController.PlayMoveSound();
	}
}
