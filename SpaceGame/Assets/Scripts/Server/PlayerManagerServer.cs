using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerManagerServer : MonoBehaviour
{
	public GameObject PlayerPrefab;

	private Dictionary<NetworkPlayer, GameObject> _playerLookup;
	private List<NetworkPlayer> _spawnRequests; 

	// Use this for initialization
	void Start ()
	{
		// There's nothing to do until the server is initialized
		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Network.isClient)
		{
			this.enabled = false;
		}

		foreach (var player in _spawnRequests.ToArray())
		{
			SpawnPlayer(player);
			_spawnRequests.Remove(player);
		}

	}

	void OnServerInitialized()
	{
		this.enabled = true;
		_playerLookup = new Dictionary<NetworkPlayer, GameObject>();
		_spawnRequests = new List<NetworkPlayer>();
	}

	void OnPlayerConnected(NetworkPlayer player)
	{
		_spawnRequests.Add(player);
	}

	void OnPlayerDisconnected(NetworkPlayer player)
	{
		Network.DestroyPlayerObjects(player);
		
		Network.Destroy(_playerLookup[player]);
		_playerLookup.Remove(player);
	}

	private void SpawnPlayer(NetworkPlayer player)
	{
		var playerObject = (GameObject)Network.Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity, 0);

		_playerLookup.Add(player, playerObject);

		var view = playerObject.GetComponent<NetworkView>();
		view.RPC("SetOwner", RPCMode.AllBuffered, player);
	}
}
