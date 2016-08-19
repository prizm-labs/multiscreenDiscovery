using UnityEngine;
using System.Collections;

public class PlayTableManager : MonoBehaviour {
	public bool hosting = true;
	// Use this for initialization
	void Start () {
		PlayTableBootStrap ();
	}

	void PlayTableBootStrap(){
		GameObject mainMenu = Instantiate (Resources.Load ("PlayTablePrefabs/MainMenu"))as GameObject;
		mainMenu.GetComponent<LoginManager> ().Hosting = hosting;
		Instantiate (Resources.Load ("PlayTablePrefabs/NetworkDiscoveryManager"));
		Instantiate (Resources.Load ("PlayTablePrefabs/TouchScript"));
		Instantiate (Resources.Load ("PlayTablePrefabs/WebSocketManager"));
	}
}
