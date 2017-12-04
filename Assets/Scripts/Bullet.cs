using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField]
	private GameObject playerBlood;

	[SerializeField]
	private Blood pickupBlood;

	private AudioSource audioSource;

	[Header("SFX")]
	[SerializeField]
	AudioClip[] hitWall;

	private float lifetime = 2f;

	private bool waitingDestroy = false;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();

		StartCoroutine(Decay());
	}

	private IEnumerator Decay()
	{
		yield return new WaitForSeconds(lifetime);

		if(!waitingDestroy)
		{
			StartCoroutine(LateDestroy());
			transform.GetChild(0).gameObject.SetActive(false);
			waitingDestroy = true;

			for (int i = 0; i < 5; i++)
			{
				Instantiate(playerBlood, transform.position + (Random.insideUnitSphere * 0.2f), transform.rotation);
			}
		}
	}

	void Update ()
	{
		if (!waitingDestroy)
		{
			transform.Translate(transform.forward * 10f * Time.deltaTime, Space.World);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!waitingDestroy && !other.CompareTag("Player") && !other.CompareTag("Item") && !other.CompareTag("Trigger"))
		{
			StartCoroutine(LateDestroy());
			transform.GetChild(0).gameObject.SetActive(false);
			waitingDestroy = true;

			if (other.CompareTag("Enemy"))
			{
				//other.GetComponent<IHealth>().GetOneHealth();
				IHealth monsterHealth = other.GetComponent<IHealth>();
				if (monsterHealth.Health < monsterHealth.MaxHealth)
				{
					Blood blood = Instantiate(pickupBlood, transform.position + transform.up + (Random.insideUnitSphere * 1), transform.rotation);
					blood.SetTarget(other.transform);
				}
				else
				{
					Instantiate(playerBlood, transform.position + (Random.insideUnitSphere * 0.2f), transform.rotation);
					PlayRandomSound(hitWall);
				}
			}
			else
			{
				for (int i = 0; i < 5; i++)
				{
					Instantiate(playerBlood, transform.position + (Random.insideUnitSphere * 0.2f), transform.rotation);
				}
				PlayRandomSound(hitWall);
			}
		}
	}

	IEnumerator LateDestroy()
	{
		yield return new WaitForSeconds(0.5f);
		Destroy(this.gameObject);
	}

	private void PlayRandomSound(AudioClip[] clips)
	{
		audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
	}
}
