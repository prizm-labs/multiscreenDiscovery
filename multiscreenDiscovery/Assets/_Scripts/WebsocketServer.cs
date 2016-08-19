using System;
using System.Threading;
using UnityEngine;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebsocketServer : MonoBehaviour {
    public static WebsocketServer instance;
	public WebSocketSessionManager playerSessions;

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
	public void sendPlayerUpdate()
	{  
		//sample JSONObject
		var update = new JSONObject(delegate (JSONObject request){
				request.AddField("topic", "gamestate");
				request.AddField("winningBid", "wow");
				request.AddField("startingBid", "whatthe");
				request.AddField("winningPlayer", "me");
			}).ToString();

		playerSessions.Broadcast(update);        

	}

	//send commands via messaging
	public void sendPlayerUpdate(string msg){
		playerSessions.Broadcast(msg);     
	}
}



public class PlayerService : WebSocketService{
	//when player login, send them update of gamestate
	protected override void OnOpen()
	{
		WebsocketServer.instance.sendPlayerUpdate();
	}

	//data we receive from players
	protected override void OnMessage(MessageEventArgs e)
	{
		WebsocketServer.instance.playermsg = e.Data;

	}

	protected override void OnClose(CloseEventArgs e)
	{
		
	}
}

