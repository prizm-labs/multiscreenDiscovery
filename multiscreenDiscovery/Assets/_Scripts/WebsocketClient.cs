using UnityEngine;
using System;
using System.Collections;
using WebSocketSharp;

public class WebsocketClient : MonoBehaviour {
	public string ip;
	public string sessionName;
	public string websocketService = "players";
	// Use this for initialization
	void Start () {

        string res = null;

		var ws = new WebSocket("ws://"+ip+":4649/"+websocketService);


        var ver = Application.unityVersion;
        ws.OnOpen += (sender, e) =>
        {
            Debug.LogError("OnOpen");
            ws.Send(String.Format("Hello, Unity {0}!", ver));
        };

        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("OnMessage");
            res = e.Data;
            Debug.LogError(res);
        };

        ws.OnError += (sender, e) =>
        {
            Debug.LogError(e.Message);
        };
        

        ws.Connect();

    }
	
	// Update is called once per frame
	void Update () {

	}
}
