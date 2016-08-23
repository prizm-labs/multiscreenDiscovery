using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class WebsocketClient : MonoBehaviour {
	public static WebsocketClient instance;

	public string ip;
	public string sessionName;
	public string websocketService = "players";

	public WebSocket ws;
	public string wsMessage = "";

	void Awake(){
		if (instance == null) {
			instance = this;
		} else
			Destroy (gameObject);
	}

	void Update() {
		if (wsMessage != "") {
			Debug.LogError (wsMessage);
			wsMessage = "";
			}
	}

	public void BeginConnection(string ipAddress, string name){
		ip = ipAddress;
		sessionName = name;
		ws = new WebSocket("ws://"+ip+":4649/"+websocketService);

		ws.OnOpen += (sender, e) =>
		{
		};

		ws.OnMessage += (sender, e) =>
		{
			wsMessage = e.Data;
		};

		ws.OnError += (sender, e) =>
		{
			Debug.LogError(e.Message);
		};        

		ws.OnClose += (sender, e) => {
			Debug.LogError("Closing");
			ws.Send(String.Format("Player disconnected"));
		};
		
		ws.Connect();
	}

	public void SendData(object data){
		ws.Send (data.ToString ());
	}

	public void CancelConnection(){
		ws.Close ();
	}

}
