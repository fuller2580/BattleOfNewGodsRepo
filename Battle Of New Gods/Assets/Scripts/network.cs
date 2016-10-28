using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;


public class network : MonoBehaviour {

	public GameObject standByCam;
	
	bool inRoom = false;
	bool inOptions = false;
	bool spawned = false;
	bool EvilSheepMember = false;
	bool chatBox = false;
	bool connectMsg = true;
	bool checkingRooms = false;
	
	string message;
	List<string> chatMessages;
	int maxChatMessages = 4;
	int msgShowCount = 0;
	bool showMsgs = false;
	
	PhotonView photonView;
	PhotonView playerPhoton;

	public GameObject SelectClass;
	
	Vector2 scrollPosition;
	
	string roomName = "Evil Sheep Boss Fight";
	
	string formNick = "";
	string formPassword = "";
	string RformNick = ""; 
	string RformPassword = "";
	string Remail = "";
	string TRformPassword = "";
	public GUISkin guiSkin;
	System.Boolean LoadOut;
	string LoadOutText = "";
	string formText = "";
	
	bool backOn;
	string regError = "";
	
	string URL = "http://evilsheepgaming.com/esg00001.php"; 
	bool DoLogin = true;
	bool loggedIn = false;
	
	// Use this for initialization
	void Start () {
		PhotonNetwork.player.name = PlayerPrefs.GetString("Username"," Player");
		photonView = GetComponent<PhotonView>();
		message = String.Empty;
		PhotonNetwork.automaticallySyncScene = true;
		DontDestroyOnLoad (transform.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}
	
	void Connect(){
		PhotonNetwork.ConnectUsingSettings("Bong v.001");
		//PhotonNetwork.ConnectToBestCloudServer("mobaz v0.001");
		//PhotonNetwork.ConnectToRegion(CloudRegionCode.us,"Bong v.001");
	
	
	}
	
	void chat(){
		if(chatBox){
	
		}
		else chatBox = true;
	}
	
	void OnGUI(){
		if(!inRoom)GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		if(PhotonNetwork.connected == false && inRoom == false && inOptions == false) {
			if(!loggedIn){
				if(LoadOut){
					GUI.skin = guiSkin;
					GUI.Box (new Rect(Screen.width/2 - 150, Screen.height/2 - 30, 300, 60), LoadOutText);
					GUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();   
					if (backOn && GUI.Button (new Rect(Screen.width/2 - 50, Screen.height/2 - 8, 100, 16), "Back") ){ 
						DoLogin = true;
						backOn = false;
						LoadOutText = "";
						LoadOut = false;
					}
					GUILayout.EndHorizontal();
				}
				else GUI.Window (0, new Rect (Screen.width/2 - 250, Screen.height/2 - 145, 500, 290),Login, "");
			}
			else{
				GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
	
				if(!checkingRooms){
					PlayerPrefs.SetString("Username", PhotonNetwork.player.name);
					Connect ();
					checkingRooms = true;
				}
	
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
		if(PhotonNetwork.connected == true && inRoom == false){
			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height) );
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
	
			if(checkingRooms){
				GUILayout.BeginHorizontal();
				GUILayout.Label("Room Name: ");
				roomName = GUILayout.TextField(roomName);
				GUILayout.Label("Max Players: ");
				byte roomMax = 3;
	
				if(GUILayout.Button("Create Room",GUILayout.Width(150))){
					RoomOptions roomOptions = new RoomOptions();
					roomOptions.IsVisible = true;
					roomOptions.MaxPlayers = roomMax;
					roomOptions.IsOpen = true;
					PhotonNetwork.CreateRoom(roomName,roomOptions, TypedLobby.Default);
				}
				GUILayout.EndHorizontal();
				GUILayout.Box("Rooms List: ");
				scrollPosition = GUILayout.BeginScrollView(scrollPosition, false ,true, GUILayout.Width(400), GUILayout.Height(300));
	
				foreach( RoomInfo game in PhotonNetwork.GetRoomList()) // Each RoomInfo "game" in the amount of games created "rooms" display the fallowing.
				{
					GUILayout.BeginHorizontal();
					if(game.playerCount == game.maxPlayers)GUI.color = Color.red;
					else GUI.color = Color.green;
					GUILayout.Box(game.name + " " + game.playerCount + "/" + game.maxPlayers); //Thus we are in a for loop of games rooms display the game.name provide assigned above, playercount, and max players provided. EX 2/20
					GUI.color = Color.white;
	
					if (GUILayout.Button("Join Room",GUILayout.Width(150))){
	
						PhotonNetwork.JoinRoom(game.name); // Next to each room there is a button to join the listed game.name in the current loop.
					}
					GUILayout.EndHorizontal();
				}
	
				GUILayout.EndScrollView();
			}
	
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndArea();
	
		}
		if(PhotonNetwork.connected == true && inRoom == true) {
			if(connectMsg){
				AddChatMessage(PhotonNetwork.player.name + " has connected.");
				connectMsg = false;
			}
			GUILayout.BeginArea( new Rect(0, 0, Screen.width, Screen.height));
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			if(showMsgs){
				foreach(string msg in chatMessages) {
					GUILayout.Label(msg);
				}
			}
			if(chatBox){
				if (Event.current.Equals (Event.KeyboardEvent ("return"))){
					message = PhotonNetwork.player.name + ": " + message;
					GetComponent<PhotonView>().RPC ("AddChatMessage_RPC", PhotonTargets.AllBuffered, message);
					message = String.Empty;
					chatBox = false;
					StartCoroutine(chatEnd());
	
				}	
				GUI.SetNextControlName("chatWindow");
				message = GUILayout.TextField(message);
				GUI.FocusControl("chatWindow");
	
			}
			GUILayout.EndVertical();
			GUILayout.EndArea();
	
	
			if(spawned == false){
				GUILayout.BeginArea( new Rect(0, 0, Screen.width/10, Screen.height/10) );
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
	
					for(int i = 0; i < PhotonNetwork.playerList.Length; i++){
						GUILayout.Label(PhotonNetwork.playerList[i].name);
					}
	
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
	
				GUILayout.BeginArea( new Rect(0, 0, Screen.width, (Screen.height/4)*3) );
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginVertical();
				GUILayout.FlexibleSpace();
				if(!PhotonNetwork.isMasterClient){
					if(PhotonNetwork.playerList.Length < 3)GUILayout.Label("Waiting for Players");
					else GUILayout.Label("Waiting for Host");
				}
				else{
					if(PhotonNetwork.playerList.Length > 3)GUILayout.Label("Waiting for Players");
					else {
						if(GUILayout.Button("Start Game",GUILayout.Width(150))){
							photonView.RPC("startGame",PhotonTargets.All);	
						}
					}
				}
				//GUILayout.Label(GUI.tooltip,GUILayout.Width(100), GUILayout.Height(200));
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.EndArea();
			}
		}
	}
	IEnumerator chatEnd(){
		yield return new WaitForSeconds(0.5f);
		chatBox = false;
	}
	
	void AddChatMessage(string m) {
		//GetComponent<PhotonView>().RPC ("AddChatMessage_RPC", PhotonTargets.AllBuffered, m);
	}
	
	[PunRPC]
	void AddChatMessage_RPC(string m) {
		while(chatMessages.Count >= maxChatMessages) {
			chatMessages.RemoveAt(0);
		}
		chatMessages.Add(m);
		StartCoroutine(showMessages());
	}
	
	[PunRPC]
	void startGame(){
		spawned = true;
		SelectClass.SetActive(true);
	}
	
	IEnumerator showMessages(){
		showMsgs = true;
		msgShowCount++;
		yield return new WaitForSeconds(10);
		msgShowCount--;
		if(msgShowCount <= 0){
			msgShowCount = 0;
			showMsgs = false;
		}
	}
	
	void OnJoinedLobby(){
		//PhotonNetwork.JoinRandomRoom();
	}
	
	void OnPhotonRandomJoinFailed(){
		//print("OnPhotonRandomJoinFailed");
		PhotonNetwork.CreateRoom(null);
	}
	
	void OnJoinedRoom(){
		//print("OnJoinedRoom");
		inRoom = true;
		//SpawnPlayer();
	}
	
	void OnFailedToConnectToPhoton(){
		Debug.LogError("Failed to connect to: "+PhotonNetwork.ServerAddress);
	}
	
	void OnConnectionFail(){
		Debug.LogError("Failed to connect ");
	}
	
	void SpawnPlayer(){
		spawned = true;
	
	}
	
	void Login (int id){
		if(DoLogin){
			GUILayout.BeginVertical();
			GUI.skin = guiSkin;
	
			GUILayout.Box ("Login");
			GUILayout.Label(regError);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();     
			GUILayout.Label( "Username:" );
			formNick = GUILayout.TextField ( formNick , 15,  GUILayout.Width(345), GUILayout.Height(35));
			GUILayout.EndHorizontal();
			GUILayout.Space(10);
	
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace(); 
			GUILayout.Label( "Password:" );
			formPassword = GUILayout.PasswordField (formPassword , "*"[0],  GUILayout.Width(345), GUILayout.Height(35) ); //same as above, but for password
			GUILayout.EndHorizontal();
			GUILayout.Space(10);
			GUILayout.BeginHorizontal();
			GUILayout.Space(5);
			if ( GUILayout.Button ("Login" ) ){
				StartCoroutine(Action("Login"));
			}
			if ( GUILayout.Button ( "Sign Up" ) ){
				DoLogin = false;
				regError = "";
			}
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
	
		}
		else{
			GUI.skin = guiSkin;
	
			GUILayout.Box ("Register");
			GUILayout.Label(regError);
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace(); 
			GUILayout.Label("Username:" );
			RformNick = GUILayout.TextField (RformNick, 12,GUILayout.Width(300) ,GUILayout.Height(35)  );
	
			GUILayout.EndHorizontal();
	
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace(); 
			GUILayout.Label("Password:" );
			RformPassword = GUILayout.PasswordField (RformPassword , "*"[0],GUILayout.Width(300) , GUILayout.Height(35) );
			GUILayout.EndHorizontal();
	
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace(); 
			GUILayout.Label("Repeat Password:" );
	
			TRformPassword = GUILayout.PasswordField (TRformPassword , "*"[0],GUILayout.Width(300) , GUILayout.Height(35) );
			GUILayout.EndHorizontal();
	
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace(); 
			GUILayout.Label("Email:" );
			Remail = GUILayout.TextField (Remail, 75,GUILayout.Width(300) , GUILayout.Height(35) );
			GUILayout.EndHorizontal();
	
	
			if ( GUILayout.Button ("Finish" ) ){ 
				regError = "";
				if(TRformPassword == RformPassword){
					if(RformNick.Length > 2){
						if(RformPassword.Length > 6){
							if(validateEmail(Remail)){
								StartCoroutine(Action("Register"));
							}
							else regError = " Error: Email is not valid";
						}
						else regError = " Error: Password needs to be greater then 6 characters";
					}
					else regError = " Error: Username must be atleast 3 characters";
				}
				else regError = " Error: Passwords do not match";
	
			}
			if ( GUILayout.Button ("Back" ) ){ 
				DoLogin = true;
				regError = "";
			}
		}
	}
	
	IEnumerator Action(string Act) {
		WWWForm form = new WWWForm(); 
		string tempURL;
		if(Act =="Login"){
			LoadOutText = "Signing In...";
			form.AddField("User", formNick);
			form.AddField("Password", formPassword);
			form.AddField("Act",Act);
			LoadOut = true;
		}
		else{
			LoadOutText = "Creating Account...";
			form.AddField("User", RformNick);
			form.AddField("Password", RformPassword);
			form.AddField("Act",Act);
			form.AddField("Email", Remail);
		}
		if(formNick == "admin" && formPassword == "esg"){
			yield return new WaitForSeconds(2);
			LoadOutText = "Admin Sign in Successful";
			EvilSheepMember = true;
			PhotonNetwork.player.name = formNick;
			yield return new WaitForSeconds(1);
			loggedIn = true;
		}
		else{
			WWW w = new WWW(URL,form); 
			yield return w; 
			if (w.error != null) {
				print(w.error); 
				LoadOutText = "Error Processing Request. Refreshing...";
				yield return new WaitForSeconds(3);
				LoadOut = false;
				DoLogin = true;
			}
			else {
				if(w.text == "Correct"){
					yield return new WaitForSeconds(2);
					LoadOutText = "Sign in Successful";
					if(formNick == "Clid" || formNick == "clid" || formNick == "Chilladin" || formNick == "chilladin" || formNick == "Jeragon" || formNick == "jeragon" || formNick == "EighthWonder" || formNick == "eighthwonder") EvilSheepMember = true;
					PhotonNetwork.player.name = formNick;
					yield return new WaitForSeconds(1);
					loggedIn = true;
				}
				if(w.text == "Wrong"){
					yield return new WaitForSeconds(2);
					LoadOut = false;
					regError = " Error: Password Incorrect";
				}
				if(w.text == "No User"){
					yield return new WaitForSeconds(2);
					LoadOut = false;
					regError = " Error: No Registered User Found";
				}
				if(w.text == "ILLEGAL REQUEST"){
					LoadOutText = "Server Error";
					backOn = true;
				}
				if(w.text == "Registered"){
					yield return new WaitForSeconds(2);
					LoadOutText = "Account Created!";
					backOn = true;
				}
				if(w.text == "TAKEN"){
					yield return new WaitForSeconds(2);
					LoadOut = false;
					regError = " Error: Username already taken.";
				}
				if(w.text == "ETAKEN"){
					yield return new WaitForSeconds(2);
					LoadOut = false;
					regError = " Error: Email already in use.";
				}
				// formText = w.text; 
				w.Dispose(); 
			}
		}
		formNick = ""; 
		formPassword = "";
	}
	
	bool validateEmail(string address){
		string[] atCharacter;
		string[] dotCharacter;
		atCharacter = address.Split("@"[0]);
		if(atCharacter.Length == 2){
			dotCharacter = atCharacter[1].Split("."[0]);
			if(dotCharacter.Length >= 2){
				if(dotCharacter[dotCharacter.Length - 1].Length == 0){
					return false;
				}
				else return true;
			}
			else return false;
		}
		else return false;
	
	}
}
