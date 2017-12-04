using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController : MonoBehaviour, IHealth
{
	private int maxHealth = 10;
	private int health = 5;
	private Rigidbody myRigidbody;
	private Vector3 mousePosition;
	private float maxSpeed = 7f;
	private float minSpeed = 1f;
	private bool charge = false;
	private bool chargeCooldown = false;
	private float chargeCooldownTime = 0.3f;
	private bool enemyHit = false;
	private bool takingDamage = false;
	private float takingDamageTime = 0.2f;

	private Animator animator;

	[SerializeField]
	private GameObject gameOverPanel;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private GameObject[] skins;

	[SerializeField]
	private GameObject dashTrailPrefab;
	private Vector3 dashStartPosition;

	[Header("SFX")]
	[SerializeField]
	AudioClip[] collectBlood;

	[SerializeField]
	AudioClip[] shot;

	[SerializeField]
	AudioClip[] slash;

	[SerializeField]
	AudioClip[] hit;

	[SerializeField]
	AudioClip[] death;

	[SerializeField]
	AudioClip[] step;

	private float speed;

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

	[Header("Others")]

	[SerializeField]
	private SpriteRenderer arrow;

	[SerializeField]
	private Bullet bulletPrefab;

	[SerializeField]
	private GameObject playerBlood;

	void Start ()
	{
		myRigidbody = GetComponent<Rigidbody>();
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
		if (healthRatio < 0.25f)
		{
			currentSkin = skins[0];
		}
		else if (healthRatio < 0.5f)
		{
			currentSkin = skins[1];
		}
		else if (healthRatio < 0.5f)
		{
			currentSkin = skins[2];
		}
		else
		{
			currentSkin = skins[3];
		}
		currentSkin.SetActive(true);
		animator = currentSkin.GetComponent<Animator>();
		animator.speed = speed;
	}

	void Update()
	{
		RaycastHit hitInfo;
		Ray rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (!chargeCooldown && !takingDamage && Physics.Raycast(rayCast, out hitInfo, Mathf.Infinity, 1 << LayerMask.NameToLayer("Floor")))
		{
			mousePosition = hitInfo.point;
			transform.LookAt(mousePosition);
		}

		if (!chargeCooldown && !takingDamage && Input.GetButtonUp("Fire1"))
		{
			dashStartPosition = transform.position;
			charge = true;
			chargeCooldown = true;

			animator.SetInteger("AnimState", 2);

			PlayRandomSound(slash);

			RaycastHit[] hits = Physics.BoxCastAll(transform.position + (transform.up * 0.5f), new Vector3(.5f, .5f, .01f), transform.forward, transform.rotation, Mathf.Min((mousePosition - transform.position).magnitude, speed));
			foreach(RaycastHit hit in hits)
			{
				if (hit.transform.CompareTag("Player"))
				{
					continue;
				}

				if (hit.transform.CompareTag("Enemy"))
				{
					hit.transform.GetComponent<Enemy>().TakeDamage();
					enemyHit = true;
					//health = Mathf.Min(maxHealth, health + 5);
				}
				else
				{
					if(hit.transform.CompareTag("Trigger") || hit.transform.CompareTag("Item"))
					{
						hit.transform.SendMessage("OnTriggerEnter", GetComponent<Collider>());
					}
					continue;
				}
			}

			StartCoroutine(FinishChargeCooldown());
		}

		if (!chargeCooldown && !takingDamage && Input.GetButtonDown("Fire2"))
		{
			if (health > 1)
			{
				Instantiate(bulletPrefab, transform.position + (transform.right * -0.25f) + (transform.up * 0.65f) + (transform.forward * 0.72f), transform.rotation);
				health--;
				SetSpeed();
				PlayRandomSound(shot);
			}
		}

		if(!chargeCooldown && !takingDamage && Input.GetButton("Fire1"))
		{
			arrow.size = new Vector2(0.7f, Mathf.Min((mousePosition - transform.position).magnitude, speed));
		}
		else
		{
			arrow.size = Vector2.zero;
		}
	}

	IEnumerator FinishChargeCooldown()
	{
		yield return new WaitForSeconds(chargeCooldownTime);
		chargeCooldown = false;
		animator.SetInteger("AnimState", 0);

		if (enemyHit)
		{
			SetSpeed();
			enemyHit = false;
		}
	}

	void FixedUpdate ()
	{
		Vector3 direction = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		direction = direction.normalized * speed;
		if (charge)
		{
			Vector3 difference = mousePosition - myRigidbody.position;
			if (difference.magnitude < speed)
			{
				direction = difference.normalized * difference.magnitude / Time.fixedDeltaTime;
			}
			else
			{
				direction = difference.normalized * speed / Time.fixedDeltaTime;
			}
			StartCoroutine(DrawOnNextFrame());
			charge = false;
		}
		else if(takingDamage)
		{
			direction = damageDirection;
		}
		else if (chargeCooldown)
		{
			direction = Vector3.zero;
		}

		if(!takingDamage && !chargeCooldown)
		{
			if(direction != Vector3.zero)
			{
				animator.SetInteger("AnimState", 1);
			}
			else
			{
				animator.SetInteger("AnimState", 0);
			}
		}

		//myRigidbody.MovePosition(myRigidbody.position + direction);
		//myRigidbody.velocity = Vector3.zero;
		
		var v = myRigidbody.velocity;
		var velocityChange = (direction - v);
		velocityChange.y = 0;

		myRigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
	}

	IEnumerator DrawOnNextFrame()
	{
		yield return new WaitForEndOfFrame();

		while (Vector3.Distance(dashStartPosition, transform.position) > 1.5f)
		{
			Instantiate(dashTrailPrefab, dashStartPosition, transform.rotation);
			dashStartPosition += transform.forward * 1.5f;
		}
	}

	Vector3 damageDirection;
	public void TakeDamage(Transform from, int damage)
	{
		damageDirection = (transform.position - from.position).normalized * 10f;

		health -= damage;
		
		takingDamage = true;
		StartCoroutine(FinishTakeDamage());

		animator.SetInteger("AnimState", 3);

		PlayRandomSound(hit);
	}

	public void GetOneHealth()
	{
		if (health < 10)
		{
			health++;
			SetSpeed();
			PlayRandomSound(collectBlood);
		}
	}

	IEnumerator FinishTakeDamage()
	{
		yield return new WaitForSeconds(takingDamageTime);

		if (health <= 0)
		{
			PlayRandomSound(death);
			audioSource.gameObject.transform.parent = null;
			Destroy(audioSource.gameObject, 2f);
			Destroy(this.gameObject);

			for (int i = 0; i < 15; i++)
			{
				Instantiate(playerBlood, transform.position + transform.up + (Random.insideUnitSphere * 0.5f), transform.rotation);
			}

			gameOverPanel.SetActive(true);
		}
		else
		{
			SetSpeed();
		}

		animator.SetInteger("AnimState", 0);
		takingDamage = false;
	}
	
	public void PlayStep()
	{
		PlayRandomSound(step);
	}

	private void PlayRandomSound(AudioClip[] clips)
	{
		audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
	}
}
