using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menucontroll : MonoBehaviour
{
 [SerializeField] private string VersionName="0.1";
[SerializeField] private GameObject UsernameMenu;
[SerializeField] private GameObject ConnectPanel;

[SerializeField] private InputField UsernameInput;
[SerializeField] private InputField CreateGameInput;
[SerializeField] private InputField JoinGameInput;

[SerializeField] private GameObject StartButton;
[SerializeField] private GameObject CreateFromStart;
[SerializeField] private GameObject CreateFromStart1;

public void Awake()
{
PhotonNetwork.ConnectUsingSettings(VersionName);
}

private void Start()
{
UsernameMenu.SetActive(true);
}


public void OnConnectedToMaster()
{
PhotonNetwork.JoinLobby(TypedLobby.Default);
Debug.Log("Connected");
}
public void ChangeUserNameInput()
{
if(UsernameInput.text.Length < 4)


{
StartButton.SetActive(true);
}
else
{
CreateFromStart.SetActive(false);
CreateFromStart1.SetActive(true);
}
}
public void SetUserName()
{
UsernameMenu.SetActive(false);
PhotonNetwork.playerName=UsernameInput.text;
   
}
public void CreateGame()
{
PhotonNetwork.CreateRoom(CreateGameInput.text,new RoomOptions() {maxPlayers=4},null);

}

public void JoinGame()
{
RoomOptions roomOptions=new RoomOptions();
roomOptions.maxPlayers=4;
PhotonNetwork.JoinOrCreateRoom(JoinGameInput.text,roomOptions,TypedLobby.Default);

}
private void OnJoinedRoom()
{
PhotonNetwork.LoadLevel("testmulty 1");
}

}
