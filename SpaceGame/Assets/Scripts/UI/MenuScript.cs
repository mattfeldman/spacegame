using System;
using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour
{

	private enum MenuState
	{
		Root,
		Server,
		Client,
		Running
	};

	private MenuState _menuState;
	private String _server;
	private String _serverPort;
	private String _clientPort;

	// Use this for initialization
	void Start ()
	{
		_menuState = MenuState.Root;
		_server = "127.0.0.1";
		_serverPort = "25001";
		_clientPort = "25001";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void OnGUI()
	{
		switch (_menuState)
		{
			case MenuState.Root:
				RootMenu();
				break;
			case MenuState.Server:
				ServerMenu();
				break;
			case MenuState.Client:
				ClientMenu();
				break;
			case MenuState.Running:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
	}

	private void RootMenu()
	{
		if (GUI.Button(new Rect(0, 0, 200, 20), "Create Server"))
		{
			_menuState = MenuState.Server;
		}
		if (GUI.Button(new Rect(0, 25, 200, 20), "Connect to a server"))
		{
			_menuState = MenuState.Client;
		}
	}

	private void ServerMenu()
	{
		GUI.Label(new Rect(0, 0, 100, 20), "Port: ");
		_serverPort = GUI.TextField(new Rect(100, 0, 200, 20), _serverPort);

		if (GUI.Button(new Rect(0, 25, 200, 20), "Create Server"))
		{
			if (Network.InitializeServer(32, Convert.ToInt32(_serverPort), false) == NetworkConnectionError.NoError)
			{
				_menuState = MenuState.Running;
			}
		}

		if (GUI.Button(new Rect(0, 50, 200, 20), "Back to main menu"))
		{
			_menuState = MenuState.Root;
		}
	}

	private void ClientMenu()
	{
		GUI.Label(new Rect(0, 0, 100, 20), "Server to connect to: ");
		_server = GUI.TextField(new Rect(100, 0, 200, 20), _server);

		GUI.Label(new Rect(0, 25, 100, 20), "Port: ");
		_clientPort = GUI.TextField(new Rect(100, 25, 200, 20), _clientPort);

		if (GUI.Button(new Rect(0, 50, 200, 20), "Connect to Server"))
		{
			if (Network.Connect(_server, Convert.ToInt32(_clientPort)) == NetworkConnectionError.NoError)
			{
				_menuState = MenuState.Running;
			}
		}

		if (GUI.Button(new Rect(0, 75, 200, 20), "Back to main menu"))
		{
			_menuState = MenuState.Root;
		}
	}
}
