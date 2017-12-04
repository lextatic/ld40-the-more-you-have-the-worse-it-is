using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
	private bool takingDamage;
	private float damageTime = 0.5f;
	private bool shakeRight = false;
	private int maxHealth = 10;
	private int health = 6;
	private float maxSpeed = 7f;
	private float minSpeed = 1f;
	private bool attacking = false;
	private float attackChargeTime = 0.5f;
	
	private float speed;

	private Animator animator;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private GameObject[] skins;

	[SerializeField]
	private Blood bloodPrefab;

	Transform player;
	Rigidbody myRigidbody;

	[SerializeField]
	private Fade attackFade;

	[SerializeField]
	private Color attackColor;

	[Header("SFX")]
	[SerializeField]
	AudioClip[] collectBlood;

	[SerializeField]
	AudioClip[] takeDamageSound;

	[SerializeField]
	AudioClip[] deathSound;

	[SerializeField]
	AudioClip[] attackPrep;

	[SerializeField]
	AudioClip[] attack;

	[SerializeField]
	AudioClip[] move;

	public int MaxHealth
	{
		get
		{
			return maxHealth;
		}
	}

	public int Health
	{
		get
		{
			return health;
		}
	}

	private void Start()
	{
		GameObject playerObj = GameObject.FindWithTag("Player");
		if (!playerObj) return;
		player = playerObj.transform;
		myRigidbody = GetComponent<Rigidbody>();
		SetSpeed();
	}

	public void SetHealth(int health)
	{
		this.health = health;
		SetSpeed();
	}

	private void SetSpeed()
	{
		float healthRatio = (float)health / maxHealth;
		speed = maxSpeed - ((maxSpeed - minSpeed) * healthRatio);
		foreach (GameObject obj in skins)
		{
			obj.SetActive(false);
		}
		GameObject currentSkin;
		if (healthRatio < 0.33f)
		{
			currentSkin = skins[0];
		}
		else if (healthRatio < 0.66f)
		{
			currentSkin = skins[1];
		}
		else
		{
			currentSkin = skins[2];
		}
		currentSkin.SetActive(true);
		animator = currentSkin.GetComponent<Animator>();
		animator.speed = speed;
	}

	private void Update()
	{
		if (!player) return;

		if (!takingDamage)
		{
			transform.LookAt(player);
		}
		else
		{
			if(shakeRight)
			{
				transform.Translate(transform.right * 0.1f);
			}
			else
			{
				transform.Translate(transform.right * -0.1f);
			}
			shakeRight = !shakeRight;
		}

		if(Vector3.Distance(transform.position, player.position) < 1f && !attacking && !takingDamage)
		{
			attacking = true;
			StartCoroutine(Attack());
			animator.SetInteger("AnimState", 1);
			PlayRandomSound(attackPrep);
		}
	}

	void FixedUpdate()
	{
		Vector3 direction = Vector3.zero;

		if (!takingDamage && !attacking && player)
		{
			direction = transform.forward * speed;
		}

		Vector3 v = myRigidbody.velocity;
		Vector3 velocityChange = (direction - v);
		velocityChange.y = 0;

		myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}

	public void TakeDamage()
	{
		GetComponent<SphereCollider>().isTrigger = true;
		takingDamage = true;

		int damageTaken = 5;
		if(health < 5)
		{
			damageTaken = health;
		}

		health -= damageTaken;

		for(int i = 0; i < damageTaken; i++)
		{
			Blood blood = Instantiate(bloodPrefab, transform.position + transform.up + (Random.insideUnitSphere * 1), transform.rotation);
			blood.SetTarget(player);
		}

		SetSpeed();

		animator.SetInteger("AnimState", 3);

		if (health <= 0)
		{
			PlayRandomSound(deathSound);
			audioSource.gameObject.transform.parent = null;
			Destroy(audioSource.gameObject, 2f);
			GetComponent<Collider>().enabled = false;
		}
		else
		{
			PlayRandomSound(takeDamageSound);
		}

		StartCoroutine(EndTakeDamageCooldown());
	}

	public void GetOneHealth()
	{
		if (health < 10 && health > 0)
		{
			health++;
			SetSpeed();
			PlayRandomSound(collectBlood);
		}
	}

	IEnumerator EndTakeDamageCooldown()
	{
		yield return new WaitForSeconds(damageTime);
		takingDamage = false;
		GetComponent<SphereCollider>().isTrigger = false;

		if(health <= 0)
		{
			Destroy(this.gameObject);
			Beam.monsterKills++;
			Debug.Log("MonstersKilled: " + Beam.monsterKills);
		}
		else
		{
			SetSpeed();
		}

		animator.SetInteger("AnimState", 0);
	}

	IEnumerator Attack()
	{
		yield return new WaitForSeconds(attackChargeTime * ((float)health / maxHealth));

		Collider[] hits = Physics.OverlapBox(transform.position + (transform.up * 0.5f) + (transform.forward * 0.77f), new Vector3(.25f, .25f, .25f), transform.rotation);

		foreach(Collider hit in hits)
		{
			if(hit.CompareTag("Player"))
			{
				hit.GetComponent<CharacterController>().TakeDamage(transform, 5);
			}
		}

		animator.SetInteger("AnimState", 2);

		PlayRandomSound(attack);

		attackFade.FadeFrom(attackColor);

		yield return new WaitForSeconds(attackChargeTime * ((float)health / maxHealth));

		attacking = false;

		animator.SetInteger("AnimState", 0);
	}

	public void PlayMoveSound()
	{
		PlayRandomSound(move);
	}

	private void PlayRandomSound(AudioClip[] clips)
	{
		audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
	}
}
