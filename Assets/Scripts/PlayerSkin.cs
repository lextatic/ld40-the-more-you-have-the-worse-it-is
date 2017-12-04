using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkin : MonoBehaviour {

	[SerializeField]
	private CharacterController myCharacterController;

	public void PlayStepSound()
	{
		myCharacterController.PlayStep();
	}
}
