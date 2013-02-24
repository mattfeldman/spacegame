using UnityEngine;
using System.Collections;

public class BaseWeapon : MonoBehaviour
{
	public GameObject BulletPrefab;

	protected float CooldownTime;
	protected float CooldownLeft;

	// Use this for initialization
	public void Start ()
	{
		this.CooldownTime = 1f;
		this.CooldownLeft = -1f;
	}

	public void Update()
	{
		Debug.Log("update " + this.CooldownLeft);
		if (this.CooldownLeft > 0)
		{
			Debug.Log(Time.deltaTime);
			this.CooldownLeft -= Time.deltaTime;
		}
	}

	public GameObject Fire(Vector3 position)
	{
		Debug.Log("fire: " + this.CooldownLeft);
		if (this.CooldownLeft <= 0)
		{
			this.CooldownLeft = this.CooldownTime;
			var firedBullet = (GameObject)Instantiate(BulletPrefab, position, Quaternion.identity);
			return firedBullet;
		}

		return null;
	}
}
