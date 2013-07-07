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

	private int _frameCounter;
	private float _timeCounter;
	private float _fps;

	// Use this for initialization
	void Start ()
	{
		_menuState = MenuState.Root;
		_server = "127.0.0.1";
		_serverPort = "25001";
		_clientPort = "25001";

		_timeCounter = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		_timeCounter -= Time.deltaTime;
		_frameCounter++;

		if (_timeCounter < 0)
		{
			_fps = _frameCounter;
			_frameCounter = 0;
			_timeCounter = 1;
		}

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
				RunningInformation();
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
	}

	private void RunningInformation()
	{
		if(Network.connections.Length > 0)
		{
			GUI.Label(new Rect(0, 0, 200, 20), String.Format("Ping: {0} FPS: {1}", Network.GetAveragePing(Network.connections[0]), _fps));
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
