using UnityEngine;
using System.Collections;

public class WebSocketManager : MonoBehaviour {
	public static WebSocketManager instance;


	void Awake(){
		if (instance == null) {
			instance = this;
		} else
			Destroy (gameObject);
	}

	public void StartAsServer(){
		gameObject.AddComponent<WebsocketServer> ();
	}
	public void StartAsClient(string ip, string sessionName){
		gameObject.AddComponent<WebsocketClient> ();
		GetComponent<WebsocketClient> ().ip = ip;
		GetComponent<WebsocketClient> ().sessionName = sessionName;
	}
}
