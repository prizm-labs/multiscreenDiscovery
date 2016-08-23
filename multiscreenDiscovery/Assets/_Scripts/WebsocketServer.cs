using System;
using System.Threading;
using UnityEngine;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebsocketServer : MonoBehaviour {
    public static WebsocketServer instance;
	public WebSocketSessionManager playerSessions;

	public delegate void DataReceiveDelegate(object data);
	public DataReceiveDelegate PieceDescriptorData;
	public DataReceiveDelegate PlayerDescriptorData;
	public DataReceiveDelegate ZoneDescriptorData;

	public delegate void PlayerSeatDelegate();

	public PlayerSeatDelegate updatePlayerSeatOnConnect;



	public string playermsg = "";
	public static int port = 4649;

	WebSocketServer wssv = new WebSocketServer(port);

    void Awake() {
        if (instance == null)
        {
			DontDestroyOnLoad (gameObject);
            instance = this;
        }
        else
            Destroy(gameObject);
    }

	void Start () {
		String PlayerServicePath = "players";
		wssv.AddWebSocketService<PlayerService>("/"+ PlayerServicePath);

		wssv.Start();
		playerSessions = wssv.WebSocketServices["/" + PlayerServicePath].Sessions;
    }


    void Update () {		
		if (playermsg != "") {
			Debug.LogError (playermsg);
			JSONObject jo = new JSONObject (playermsg);
			switch (jo ["DataType"].str) {

			case("PlayerDescriptor"):
				PlayerDescriptorData (jo.ToString ());
				break;
			default:
				break;
			}
			//var obj = new JSONObject(playermsg);
			/*
			for (int i = 0; i < obj.Count; i++)
			{
				if (obj.keys[i] != null)
				{
					switch ((string)obj.keys[i])
					{
					case "topic":
						{
							Debug.LogError ("Inside topic");
							String topic = (String)obj.list [i].str;

							switch (topic) {
							case "refresh":
								Debug.LogError (topic);
								WebsocketServer.instance.sendPlayerUpdate ();
								break;

							default:
								break;
							}

							//do stuff with (JSONObject)obj.list[i];

							break;
						}
					case "key2":
						//do stuff with (JSONObject)obj.list[i];

						break;
					}
				}
			}
*/
			playermsg = "";
		}
	}

	//send game state via JSONObject
	public void sendPlayerUpdate(object data)
	{  
		playerSessions.Broadcast(data.ToString());
	}


}



public class PlayerService : WebSocketService{
	
	//when player login, send them update of gamestate
	protected override void OnOpen()
	{
		WebsocketServer.instance.updatePlayerSeatOnConnect();
	}

	//data we receive from players
	protected override void OnMessage(MessageEventArgs e)
	{

		Debug.LogError("Received Message from server " + e.Data);

		switch(e.Data[0].ToString()){
		case("PlayerDescriptor"):
			WebsocketServer.instance.PlayerDescriptorData(e.Data);
			break;
		case("ZoneDescriptor"):
			WebsocketServer.instance.ZoneDescriptorData(e.Data);
			break;
		case("PieceDescriptor"):
			WebsocketServer.instance.PieceDescriptorData(e.Data);
			break;
		default:
			break;

		}
		WebsocketServer.instance.playermsg = e.Data;

	}

	protected override void OnClose(CloseEventArgs e)
	{
		
	}
}

