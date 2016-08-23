using UnityEngine;
using System.Collections;

public class TTManager : MonoBehaviour {
	public static TTManager instance;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else
			Destroy (gameObject);
	}

	void Update(){
		if (Input.GetKeyDown ("q")) {
			WebsocketClient.instance.SendData ("HELLOHH!");
		}
	}
	void OnEnable(){
		
	}
	void OnDisable(){
		
	}

}
