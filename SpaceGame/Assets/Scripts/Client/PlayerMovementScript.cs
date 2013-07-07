using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
	public float MinX;
	public float MaxX;

	public BaseWeapon Weapon;

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
			
			if(Input.GetButton("Jump"))
			{
				networkView.RPC ("Fire", RPCMode.Server);
			}
			
			// attempt to predict movement by applying input locally
			rigidbody.AddTorque(Vector3.up * rotation);
			rigidbody.AddRelativeForce(Vector3.forward * thrust * 10, ForceMode.Force);
	    }
	    else
	    {
		    this.enabled = false;
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
