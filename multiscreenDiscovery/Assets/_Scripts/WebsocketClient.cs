using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class WebsocketClient : MonoBehaviour {
	public static WebsocketClient instance;

	public delegate void DataReceiveDelegate(object data);
	public DataReceiveDelegate PieceDescriptorData;
	public DataReceiveDelegate PlayerDescriptorData;
	public DataReceiveDelegate ZoneDescriptorData;

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
			JSONObject d = new JSONObject(wsMessage);
			Debug.LogError(d[0].ToString());
			switch(d[0].ToString()){
			case("\"PlayerDescriptor\""):
				Debug.LogError("INSidie switch");
				PlayerDescriptorData(wsMessage);
				break;
			case("\"ZoneDescriptor\""):
				ZoneDescriptorData(wsMessage);
				break;
			case("\"PieceDescriptor\""):
				PieceDescriptorData(wsMessage);
				break;
			default:
				break;

			}
			wsMessage = "";
		}
	}

	public void BeginConnection(string ipAddress, string name){
		ip = ipAddress;
		sessionName = name;
		ws = new WebSocket("ws://"+ip+":4649/"+websocketService);

		var ver = Application.unityVersion;
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
