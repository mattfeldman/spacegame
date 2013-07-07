using UnityEngine;
using System.Collections;

public class PlayerAnimationClient : MonoBehaviour 
{
	public ParticleSystem LeftJetParticleSystem;
	public ParticleSystem RightJetParticleSystem;
	
	private float _previousRotationVelocity;

	void Start () 
	{
		_previousRotationVelocity = 0;
	}
	
	void Update () 
	{
		// to avoid using too much bandwidth sending animation information to all the clients
		// we leave it up to the clients to determine if an animation is needed based on the player's status
		// We may want to revisit this approach later, depending on how well it works.
		var angularVelocity = rigidbody.angularVelocity.y;
		
		if(angularVelocity < 0 && angularVelocity < _previousRotationVelocity)
		{
			FireRightJet();	
		}
		
		if(angularVelocity > 0 && angularVelocity > _previousRotationVelocity)
		{
			FireLeftJet();	
		}
		
		_previousRotationVelocity = angularVelocity;
	}
	
	void FireRightJet()
	{
		RightJetParticleSystem.Emit(5);	
	}

	void FireLeftJet()
	{
		LeftJetParticleSystem.Emit(5);	
	}
}
