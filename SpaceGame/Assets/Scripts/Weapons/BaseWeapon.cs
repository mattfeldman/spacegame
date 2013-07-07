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
		if (this.CooldownLeft > 0)
		{
			this.CooldownLeft -= Time.deltaTime;
		}
	}

	public GameObject Fire(Vector3 position, Quaternion rotation)
	{
		if (this.CooldownLeft <= 0)
		{
			this.CooldownLeft = 1;
			var firedBullet = (GameObject)Network.Instantiate(BulletPrefab, position, rotation, 0);
			return firedBullet;
		}

		return null;
	}
}
