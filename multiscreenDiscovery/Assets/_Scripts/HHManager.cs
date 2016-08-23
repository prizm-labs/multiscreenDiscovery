using UnityEngine;
using System.Collections;

public class HHManager : MonoBehaviour {
	public static HHManager instance;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else
			Destroy (gameObject);
	}

	void OnEnable(){
		WebsocketClient.instance.PlayerDescriptorData += PlayerDescriptorFunction;
		WebsocketClient.instance.ZoneDescriptorData += ZoneDescriptorFunction;
		WebsocketClient.instance.PieceDescriptorData += PieceDescriptorFunction;
	}
	void OnDisable(){
		WebsocketClient.instance.PlayerDescriptorData -= PlayerDescriptorFunction;
		WebsocketClient.instance.ZoneDescriptorData -= ZoneDescriptorFunction;
		WebsocketClient.instance.PieceDescriptorData -= PieceDescriptorFunction;

	}

	public void PlayerDescriptorFunction(object data){	
		Debug.LogError (gameObject.name);
		LoginManager.instance.DisplaySeats (new JSONObject(data.ToString()));
		Debug.LogError ("PlayerDescriptor function " + data);
	}	

	public void ZoneDescriptorFunction(object data){	
		Debug.LogError ("ZoneDescriptor function " + data);

	}

	public void PieceDescriptorFunction(object data){	
		Debug.LogError ("PieceDescriptor function " + data);

	}


}
