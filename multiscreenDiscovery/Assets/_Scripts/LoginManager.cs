using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {	
	public enum Seats{red=0,blue,green,yellow,orange,black,white,teal};
	public static LoginManager instance;
	public Dictionary<string, GameToJoin> AvailableGames = new Dictionary<string, GameToJoin>();

	public bool Hosting = true;
	void Awake(){
		if (instance == null) {
			instance = this;
		} else
			Destroy (this);
	}

	void Start(){
		if (Hosting) {
			CreateMainMenuButton ("Start", "HostGame");
			CreateMainMenuButton ("Load", "LoadGame");
			CreateMainMenuButton ("Tutorial", "TutorialMode");
		} else
			JoinGame ();

		gameObject.BroadcastMessage ("functionName");
	}

	public void HostGame(){
		NetworkDiscovery.instance.Initialize ();
		NetworkDiscovery.instance.StartAsServer ();
		Transform PanelConnection = transform.FindChild ("Pnl_Connection");
		PanelConnection.gameObject.SetActive(false);
		SetGameName();
		WebSocketManager.instance.StartAsServer ();
	}

	public void JoinGame(){
		NetworkDiscovery.instance.Initialize ();
		NetworkDiscovery.instance.StartAsClient ();
		DisplayGames ();
		Transform PanelNetworkGames = transform.FindChild ("Pnl_NetworkGames");
		Transform PanelConnection = transform.FindChild ("Pnl_Connection");
		PanelConnection.gameObject.SetActive(false);
		PanelNetworkGames.gameObject.SetActive(true);
		CreateRefreshButton();
	}

	public void RemoveGame(string ip){
		AvailableGames.Remove (ip);
		DisplayGames ();
	}

	public void AddGame(GameToJoin game){
		AvailableGames.Add (game.LocalIp, game);
		DisplayGames ();		
	}

	public void DisplayGames(){
		Transform PanelListGames = transform.FindChild ("Pnl_NetworkGames/Pnl_ListGames");		

		foreach (Transform child in PanelListGames) {
			Destroy (child.gameObject);
		}
		foreach(GameToJoin game in AvailableGames.Values){
			CreateGamesButton (game);
		}

	}

	public void RefreshGames(){
		AvailableGames.Clear ();
		DisplayGames ();
	}

	void CreateGamesButton(GameToJoin game){
		Transform PanelButton = transform.FindChild ("Pnl_NetworkGames/Pnl_ListGames");
		GameObject btn = Instantiate (Resources.Load ("RoomButton/Btn_Room")) as GameObject;
		btn.transform.SetParent (PanelButton);
		btn.transform.FindChild ("Text").GetComponent<Text> ().text = game.roomName + "\n" + game.LocalIp;
		btn.GetComponent<Button> ().onClick.AddListener (() => Debug.LogError(game.LocalIp));
		btn.GetComponent<Button> ().onClick.AddListener (() => WebSocketManager.instance.StartAsClient (game.LocalIp, game.roomName));

		/*
		GameObject gamebtn = Instantiate (buttonPrefab) as GameObject;
		BtnScrpt_JoinGame gamebtnScript = gamebtn.GetComponent<BtnScrpt_JoinGame> ();
		gamebtn.transform.SetParent(DisplayAvailableGamesPanelSub.transform);
		gamebtnScript.MyData = game;
		gamebtnScript.DisplayRoomInfo ();

		gamebtnScript.SetButtonAction ();
		gamebtnScript.MyButtonScript.onClick.AddListener(() => DisplayAvailableGamesPanel.SetActive (false));
		gamebtnScript.MyButtonScript.onClick.AddListener (() => DisplayLoginPanel.SetActive (false));
		Debug.LogError ("Disabling");*/
	}

	public void DisplaySeats(){/*
		foreach (Transform child in DisplayAvailableGamesPanel.transform) {
			Destroy (child.gameObject);
		}*/
	}		

	/*

public void DisplayGames(){
	Debug.LogError ("All available games");
	GameObject PanelOfGames;
	foreach (GameToJoin content in AvailableGames.Values) {
		PanelOfGames = GameObject.Find ("Canvas_Login/Panel/Panel");
		GameObject newButton = Instantiate (buttonPrefab) as GameObject;
		newButton.transform.SetParent (PanelOfGames.transform);
		newButton.GetComponent<Button>().onClick.AddListener(() => AddRoomButton(content));
		newButton.tag = content.LocalIp;
		}
	}
*/


	private void AddRoomButton(GameToJoin gameAvailable){
		Debug.LogError (gameAvailable.LocalIp);
	}

	public void CreateMainMenuButton(string buttonName, string msg){
		Transform PanelConnection = transform.FindChild ("Pnl_Connection");
		GameObject btn = Instantiate (Resources.Load ("MainMenuButton/Btn_MainMenu")) as GameObject;
		btn.transform.SetParent (PanelConnection);

		btn.transform.FindChild ("Text").GetComponent<Text> ().text = buttonName;
		btn.GetComponent<Button> ().onClick.AddListener (() => gameObject.BroadcastMessage(msg));
	}

	public void SetGameName(){
		transform.FindChild ("Pnl_GameInfo").gameObject.SetActive (true);
		Text TextGameInfo = transform.Find ("Pnl_GameInfo/Txt_RoomName").GetComponent<Text>();
		TextGameInfo.text = NetworkDiscovery.instance.RoomName;
	}

	public void CreateRefreshButton(){
		Transform PanelNetworkGames = transform.FindChild ("Pnl_NetworkGames/Pnl_RefreshPanel");
		GameObject btn = Instantiate (Resources.Load ("RefreshGamesButton/Btn_Refresh")) as GameObject;
		btn.transform.SetParent (PanelNetworkGames);
		btn.GetComponent<Button> ().onClick.AddListener (() => RefreshGames ());
	}


}
