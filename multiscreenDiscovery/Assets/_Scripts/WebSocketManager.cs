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

	public void StartAsClient(){
		gameObject.AddComponent<WebsocketClient> ();
	}
}
