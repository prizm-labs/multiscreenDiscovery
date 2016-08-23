using System;
using System.Threading;
using UnityEngine;
using System.Collections;
using WebSocketSharp;
using WebSocketSharp.Server;

public class WebsocketServer : MonoBehaviour {
    public static WebsocketServer instance;
	public WebSocketSessionManager playerSessions;

	public string wsMessage = "";
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
		if (wsMessage != "") {
			Debug.LogError (wsMessage);
			wsMessage = "";
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
		Debug.LogError ("OPEN");
	}

	//data we receive from players
	protected override void OnMessage(MessageEventArgs e)
	{
		WebsocketServer.instance.wsMessage = e.Data;
	}

	protected override void OnClose(CloseEventArgs e)
	{
		
	}
}

