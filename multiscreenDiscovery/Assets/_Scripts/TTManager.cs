using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TTManager : MonoBehaviour {
	public GameStates gs = new GameStates();
	public List<GameStates.PlayerDescriptor> ListOfPlayerDocks = new List<GameStates.PlayerDescriptor>();

	void OnEnable(){
		WebsocketServer.instance.updatePlayerSeatOnConnect += SendPlayerDockStatus;
		WebsocketServer.instance.PlayerDescriptorData += PlayerDescriptorFunction;
		WebsocketServer.instance.ZoneDescriptorData += ZoneDescriptorFunction;
		WebsocketServer.instance.PieceDescriptorData += PieceDescriptorFunction;
	}

	void OnDisable(){
		WebsocketServer.instance.updatePlayerSeatOnConnect -= SendPlayerDockStatus;
		WebsocketServer.instance.PlayerDescriptorData -= PlayerDescriptorFunction;
		WebsocketServer.instance.ZoneDescriptorData -= ZoneDescriptorFunction;
		WebsocketServer.instance.PieceDescriptorData -= PieceDescriptorFunction;
	}

	void Start(){//sample player docks data
		ListOfPlayerDocks.Add (new GameStates.PlayerDescriptor ("John", true, 0));
		ListOfPlayerDocks.Add (new GameStates.PlayerDescriptor ("danny", false, 0));
		ListOfPlayerDocks.Add (new GameStates.PlayerDescriptor ("Jimmy", false, 0));
		SendPlayerDockStatus ();
	}

	public void SendPlayerDockStatus(){
		JSONObject PlayerDocks = new JSONObject ();
		PlayerDocks.AddField ("DataType", "PlayerDescriptor");
		for (int i = 0; i < ListOfPlayerDocks.Count; i++) {
			PlayerDocks.Add (new JSONObject(JsonUtility.ToJson (ListOfPlayerDocks [i])));
		}
		Debug.LogError (PlayerDocks);
		WebsocketServer.instance.sendPlayerUpdate (PlayerDocks);
	}

	void PlayerDescriptorFunction(object data){
		
	}
	void ZoneDescriptorFunction(object data){
		
	}
	void PieceDescriptorFunction(object data){
		
	}


	

}
