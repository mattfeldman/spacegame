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
		    networkView.RPC ("Rotate", RPCMode.Server, Input.GetAxis("Horizontal"));
		    networkView.RPC ("Move", RPCMode.Server, Input.GetAxis("Vertical"));
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
		
		if(Network.isClient)
		{
			Destroy(rigidbody);
		}

		if (Network.player == _owner.Value)
		{
			enabled = true;
			Camera.main.GetComponent<CameraScript>().Target = this.gameObject;
		}
	}
}
