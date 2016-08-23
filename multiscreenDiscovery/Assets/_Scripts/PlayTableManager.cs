using UnityEngine;
using System.Collections;
using TouchScript;
using TouchScript.Gestures;


public class PlayTableManager : MonoBehaviour {
	public bool hosting = true;
	// Use this for initialization
	void Awake () {
		PlayTableBootStrap ();
	}

	void PlayTableBootStrap(){
		GameObject mainMenu = Instantiate (Resources.Load ("PlayTablePrefabs/MainMenu"))as GameObject;
		mainMenu.AddComponent<LoginManager> ();
		mainMenu.GetComponent<LoginManager> ().Hosting = hosting;
		gameObject.AddComponent<NetworkDiscovery> ();
		gameObject.AddComponent<Lexic.NameGenerator> ();
		gameObject.AddComponent<TouchScript.InputSources.StandardInput> ();
		gameObject.AddComponent<TouchScript.Behaviors.TouchScriptInputModule> ();
		gameObject.AddComponent<WebSocketManager> ();
	}
}
