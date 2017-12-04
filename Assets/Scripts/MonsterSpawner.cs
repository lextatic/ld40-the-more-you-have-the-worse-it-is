using System.Collections;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
	[SerializeField]
	private Enemy MonsterPrefab;

	[SerializeField]
	private int monsterStartingLife = 5;

	[SerializeField]
	private int spawnCharges = 1;

	[SerializeField]
	private float spawnDelay = 5f;

	void Start ()
	{
		if(spawnCharges > 0)
		{
			StartCoroutine(SpawnMonster());
		}
	}

	IEnumerator SpawnMonster()
	{
		Enemy newMonster = Instantiate(MonsterPrefab, transform.position, transform.rotation);
		newMonster.SetHealth(monsterStartingLife);
		spawnCharges--;

		yield return new WaitForSeconds(spawnDelay);

		if (spawnCharges > 0)
		{
			StartCoroutine(SpawnMonster());
		}
	}
}
