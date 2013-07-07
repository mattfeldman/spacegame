using UnityEngine;
using System.Collections;

public class PlayerManagerServer : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.DestroyPlayerObjects(player);
	}
}
