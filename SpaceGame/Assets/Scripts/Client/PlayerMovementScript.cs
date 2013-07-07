using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
	public float MinX;
	public float MaxX;

	public BaseWeapon Weapon;
	public ParticleSystem LeftJetParticleSystem;
	public ParticleSystem RightJetParticleSystem;

	private NetworkPlayer? _owner;

    // Use this for initialization
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
	    if (_owner.HasValue && _owner.Value == Network.player)
	    {
		    var rotation = Input.GetAxis("Horizontal");
		    var thrust = Input.GetAxis("Vertical");
		    networkView.RPC ("Rotate", RPCMode.Server, rotation);
		    networkView.RPC ("Move", RPCMode.Server, thrust);

			rigidbody.AddTorque(Vector3.up * rotation);
			rigidbody.AddRelativeForce(Vector3.forward * thrust * 10, ForceMode.Force);
	    }
	    else
	    {
		    this.enabled = false;
	    }
    }

	private void Fire()
	{
		var firedBullet = Weapon.Fire(transform.position, transform.rotation);
		if (firedBullet != null)
		{
			Physics.IgnoreCollision(firedBullet.collider, this.collider);
		}
	}

	[RPC]
	void SetOwner(NetworkPlayer player)
	{
		_owner = player;
		
		if (Network.player == _owner.Value)
		{
			enabled = true;
			Camera.main.GetComponent<CameraScript>().Target = this.gameObject;
		}
	}
}
