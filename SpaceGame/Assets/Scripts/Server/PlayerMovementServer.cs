using UnityEngine;
using System.Collections;

public class PlayerMovementServer : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	[RPC]
	void Rotate(float movementAmount)
	{
		var rotation = movementAmount*Time.deltaTime*100;
		
		rigidbody.AddTorque(Vector3.up*rotation);
	}
	
	[RPC]
	void Move(float moveAmount)
	{
		var thrust = moveAmount; //> 0 ? Input.GetAxis("Vertical") : 0;
		rigidbody.AddRelativeForce(Vector3.forward*thrust*10, ForceMode.Force);
	}

	[RPC]
	void Fire()
	{
		var playerScript = GetComponent<PlayerMovementScript>();

		playerScript.Weapon.Fire(transform.position, transform.rotation);
	}
}
