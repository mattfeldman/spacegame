using UnityEngine;
using System.Collections;

public class PlayerManagerClient : MonoBehaviour
{
	public GameObject PlayerPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnConnectedToServer()
	{
		if (Network.isClient)
		{
			var player = Network.Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, 0);

			var camera = Camera.main;
			camera.GetComponent<CameraScript>().Target = (GameObject) player;
		}
		else
		{
			this.enabled = false;
		}
	}
}
