using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
	public float MinX;
	public float MaxX;

	public BaseWeapon Weapon;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
	    rigidbody.transform.position += Vector3.right * Input.GetAxis("Horizontal") * Time.deltaTime * 10;

		if (rigidbody.transform.position.x < MinX)
		{
			transform.position = new Vector3(MinX, transform.position.y, transform.position.z);
		}
		if (transform.position.x > MaxX)
		{
			transform.position = new Vector3(MaxX, transform.position.y, transform.position.z);
		}

		if (Input.GetButton("Jump"))
		{
			Fire();
		}
    }

	private void Fire()
	{
		var firedBullet = Weapon.Fire(transform.position);
		if (firedBullet != null)
		{
			Physics.IgnoreCollision(firedBullet.collider, this.collider);
		}
	}
}
