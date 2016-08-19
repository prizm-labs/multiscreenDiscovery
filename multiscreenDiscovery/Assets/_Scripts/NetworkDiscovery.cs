﻿using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public class NetworkDiscovery : MonoBehaviour{

	public static NetworkDiscovery instance;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else
			Destroy (this);
	}

	public string RoomName = "";

	const int kMaxBroadcastMsgSize = 1024;
	// config data
	[SerializeField]
	public int m_BroadcastPort = 47777;

	[SerializeField]
	public int m_BroadcastKey = 1000;

	[SerializeField]
	public int m_BroadcastVersion = 1;

	[SerializeField]
	public int m_BroadcastSubVersion = 1;

	[SerializeField]
	public string m_BroadcastData = "HELLO";

	[SerializeField]
	public bool m_ShowGUI = true;

	[SerializeField]
	public int m_OffsetX;

	[SerializeField]
	public int m_OffsetY;

	// runtime data
	public int hostId = -1;
	public bool running = false;

	bool m_IsServer = false;
	bool m_IsClient = false;

	byte[] msgOutBuffer = null;
	HostTopology defaultTopology;

	public bool isServer { get { return m_IsServer; } set { m_IsServer = value; } }
	public bool isClient { get { return m_IsClient; } set { m_IsClient= value; } }

	static byte[] StringToBytes(string str)
	{
		byte[] bytes = new byte[str.Length * sizeof(char)];
		System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
		return bytes;
	}


	static string BytesToString(byte[] bytes)
	{
		char[] chars = new char[bytes.Length / sizeof(char)];
		System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
		return new string(chars);
	}

	public void Initialize()
	{
		
		if (m_BroadcastData.Length >= kMaxBroadcastMsgSize)
		{
			Debug.LogError("NetworkDiscovery Initialize - data too large. max is " + kMaxBroadcastMsgSize);
		}
		if (!NetworkTransport.IsStarted)
		{
			NetworkTransport.Init();
		}

		if (NetworkManager.singleton!= null)
		{
			m_BroadcastData = "NetworkManager:"+NetworkManager.singleton.networkAddress + ":" + NetworkManager.singleton.networkPort;
		}

		RoomName = GetRoomData ();
		msgOutBuffer = StringToBytes(RoomName);


		ConnectionConfig cc = new ConnectionConfig();

		cc.AddChannel(QosType.Unreliable);

		defaultTopology = new HostTopology(cc, 1);

		if (m_IsServer)
			StartAsServer();
		
		if (m_IsClient)
			StartAsClient();
		
	}

	// listen for broadcasts
	public void StartAsClient()
	{
		if (hostId != -1 || running)
		{
			Debug.LogWarning("NetworkDiscovery StartAsClient already started");
		}

		hostId = NetworkTransport.AddHost(defaultTopology, m_BroadcastPort);
		if (hostId == -1)
		{
			Debug.LogError("NetworkDiscovery StartAsClient - addHost failed");
		}

		byte error;
		NetworkTransport.SetBroadcastCredentials(hostId, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, out error);

		running = true;
		m_IsClient = true;
		Debug.Log("StartAsClient Discovery listening");
	}

	// perform actual broadcasts
	public void StartAsServer()
	{
		if (hostId != -1 || running)
		{
			Debug.LogWarning("NetworkDiscovery StartAsServer already started");
		}

		hostId = NetworkTransport.AddHost(defaultTopology, 0);
		if (hostId == -1)
		{
			Debug.LogError("NetworkDiscovery StartAsServer - addHost failed");
		}

		byte err;

		if (!NetworkTransport.StartBroadcastDiscovery(hostId, m_BroadcastPort, m_BroadcastKey, m_BroadcastVersion, m_BroadcastSubVersion, msgOutBuffer, msgOutBuffer.Length, 1000, out err))
		{
			Debug.LogError("NetworkDiscovery StartBroadcast failed err: " + err);
		}

		running = true;
		m_IsServer = true;
		Debug.Log("StartAsServer Discovery broadcasting");
	}

	public void StopBroadcast()
	{
		if (hostId == -1)
		{
			Debug.LogError("NetworkDiscovery StopBroadcast not initialized");
			return;
		}

		if (!running)
		{
			Debug.LogWarning("NetworkDiscovery StopBroadcast not started");
			return;
		}

		if (m_IsServer)
		{
			NetworkTransport.StopBroadcastDiscovery();
		}

		NetworkTransport.RemoveHost(hostId);
		hostId = -1;
		running = false;
		m_IsServer = false;
		m_IsClient = false;
		Debug.Log("Stopped Discovery broadcasting");
	}

	void Update()
	{
		if (hostId == -1)
			return;

		if (m_IsServer)
			return;

		int connectionId;
		int channelId;
		int receivedSize;
		byte error;
		NetworkEventType networkEvent = NetworkEventType.DataEvent;

		do
		{

			byte[] msgInBuffer = new byte[kMaxBroadcastMsgSize];

			networkEvent = NetworkTransport.ReceiveFromHost(hostId, out connectionId, out channelId, msgInBuffer, kMaxBroadcastMsgSize, out receivedSize, out error);

			if (networkEvent == NetworkEventType.BroadcastEvent)
			{
				NetworkTransport.GetBroadcastConnectionMessage(hostId, msgInBuffer, kMaxBroadcastMsgSize, out receivedSize, out error);

				string senderAddr;
				int senderPort;
				NetworkTransport.GetBroadcastConnectionInfo(hostId, out senderAddr, out senderPort, out error);

				OnReceivedBroadcast(senderAddr, BytesToString(msgInBuffer));
			}
		} while (networkEvent != NetworkEventType.Nothing);

	}

	//every client devices will receive updated broadcasts of all hosts
	public virtual void OnReceivedBroadcast(string fromAddress, string data){

		Debug.Log ("Got broadcast from [" + fromAddress + "] " + data);
		string HostIp;
		var address = fromAddress.Split (':');
		HostIp = address [3];
		Debug.LogError (HostIp);

		//if game is disconnected and is in our available list of games, remove it from that list
		if (data == "disconnected" && LoginManager.instance.AvailableGames.ContainsKey (HostIp)) {
			if (LoginManager.instance.AvailableGames.ContainsKey (HostIp)) {
				LoginManager.instance.RemoveGame (HostIp);
			}
		}

		//if game is not labeled disconnected and we don't have it on our available games list, add the game to the list
		else if (!LoginManager.instance.AvailableGames.ContainsKey (HostIp)) {
			string roomName = data;
			GameToJoin newGame = new GameToJoin (HostIp, roomName);
			LoginManager.instance.AddGame (newGame);
		} else {
			LoginManager.instance.AvailableGames [HostIp].roomName = data;
			LoginManager.instance.DisplayGames ();
		}

	}

	public string GetRoomData(){
		string roomName = gameObject.GetComponent<Lexic.NameGenerator> ().GetNextRandomName ();
	
		return roomName;
	}

	public int AssignRandomRoomNumber(){
		return UnityEngine.Random.Range (0, 99999);
	}



}
