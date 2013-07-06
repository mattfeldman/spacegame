using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
	public float MinX;
	public float MaxX;

	public BaseWeapon Weapon;
	public ParticleSystem LeftJetParticleSystem;
	public ParticleSystem RightJetParticleSystem;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		var rotation =  Input.GetAxis("Horizontal") * Time.deltaTime * 100;
	    var thrust = Input.GetAxis("Vertical") ;//> 0 ? Input.GetAxis("Vertical") : 0;

		rigidbody.AddTorque(Vector3.up*rotation);
		rigidbody.AddRelativeForce(Vector3.forward*thrust*10,ForceMode.Force);


	    if (rotation > 0)
	    {
		    LeftJetParticleSystem.Emit(5);
	    }
	    if (rotation < 0)
	    {
		    RightJetParticleSystem.Emit(5);
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
