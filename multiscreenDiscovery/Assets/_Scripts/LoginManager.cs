using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {	
	public enum Seats{red=0,blue,green,yellow,orange,black,white,teal};
	public static LoginManager instance;
	public Dictionary<string, GameToJoin> AvailableGames = new Dictionary<string, GameToJoin>();
	public AudioSource s;

	void Awake(){
		if (instance == null) {
			instance = this;
		} else
			Destroy (this);
	}

	void Start(){
		NetworkDiscovery.instance.StopBroadcast ();
		CreateHostGameButton ();
		CreateJoinGameButton ();
	}

	public void HostGame(){
		NetworkDiscovery.instance.Initialize ();
		NetworkDiscovery.instance.StartAsServer ();
	}

	public void JoinGame(){
		NetworkDiscovery.instance.Initialize ();
		NetworkDiscovery.instance.StartAsClient ();
		DisplayGames ();
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
			Debug.LogError (child.name);
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
		btn.GetComponent<Button> ().onClick.AddListener (() => HostGame ());

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

	public void CreateHostGameButton(){
		Transform PanelConnection = transform.FindChild ("Pnl_Connection");
		GameObject btn = Instantiate (Resources.Load ("HostButton/Btn_Host")) as GameObject;
		btn.transform.SetParent (PanelConnection);
		btn.GetComponent<Button> ().onClick.AddListener (() => HostGame ());
		btn.GetComponent<Button> ().onClick.AddListener (() => PanelConnection.gameObject.SetActive(false));
		btn.GetComponent<Button> ().onClick.AddListener (() => SetGameName());
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

	public void CreateJoinGameButton(){
		Transform PanelNetworkGames = transform.FindChild ("Pnl_NetworkGames");
		Transform PanelConnection = transform.FindChild ("Pnl_Connection");
		Debug.LogError (PanelConnection.name);
		GameObject btn = Instantiate (Resources.Load ("JoinButton/Btn_Join")) as GameObject;
		btn.transform.SetParent (PanelConnection);
		btn.GetComponent<Button> ().onClick.AddListener (() => JoinGame ());
		btn.GetComponent<Button> ().onClick.AddListener (() => PanelConnection.gameObject.SetActive(false));
		btn.GetComponent<Button> ().onClick.AddListener (() => PanelNetworkGames.gameObject.SetActive(true));
		btn.GetComponent<Button> ().onClick.AddListener (() => DisplayGames());
		btn.GetComponent<Button> ().onClick.AddListener (() => CreateRefreshButton());
	}



}
