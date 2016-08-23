using UnityEngine;
using System.Collections;

public class HHManager : MonoBehaviour {
	public static HHManager instance;

	void Awake(){
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else
			Destroy (gameObject);
	}

	void Update(){
		if (Input.GetKeyDown ("q")) {
			WebsocketClient.instance.SendData ("HELLOTT!");
		}
	}
	void OnEnable(){
	}
	void OnDisable(){
	}


}
