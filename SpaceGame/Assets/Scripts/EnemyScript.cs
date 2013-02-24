using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour {

	public EnemyManager EnemyManager { get; set; }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.position += EnemyManager.MoveVector * Time.deltaTime * EnemyManager.Speed;

		if (this.transform.position.x > 15 || this.transform.position.x < -15)
		{
			this.EnemyManager.EndReached();
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		foreach (var point in collision.contacts)
		{
			if (point.otherCollider.gameObject.layer == 8)
			{
				Destroy(this.gameObject);
				Destroy(point.otherCollider.gameObject);
			}
		}
	}
}
