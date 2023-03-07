using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;

public class Launcher : MonoBehaviourPunCallbacks
{
   public static Launcher Instance;
   [SerializeField]  TMP_InputField roomNameInputField;
   [SerializeField]  TMP_Text errorText;
   [SerializeField]  TMP_Text roomNameText;
   [SerializeField] Transform roomListContent;
   [SerializeField]  GameObject roomListItemPrefab;
   [SerializeField]  Transform playerListContent;
   [SerializeField] GameObject playerListItemPrefab;
   [SerializeField]  GameObject startGameButton;
   [SerializeField]  GameObject start1;
   [SerializeField]  GameObject start2;
   [SerializeField]  GameObject start3;
   [SerializeField]  GameObject start4;
   [SerializeField] GameObject RoomManager;

   void Awake()
   {

      Instance = this;
   }
   void Start()
   {
      
      PhotonNetwork.ConnectUsingSettings();   
              
   }

   public override void OnConnectedToMaster()
   {
      PhotonNetwork.JoinLobby();
      Debug.Log("Connected to master");
      PhotonNetwork.AutomaticallySyncScene = true;
   }

   public override void OnJoinedLobby()
   {
      MenuManager.Instance.OpenMenu("title");
      Debug.Log("joined lobby");
      PhotonNetwork.NickName = "Player" + Random.Range(0, 1000).ToString("0000");
   }

   public void CreateRoom()
   {
      if (string.IsNullOrEmpty(roomNameInputField.text))
      {
         return;
      }
      PhotonNetwork.CreateRoom(roomNameInputField.text);
      MenuManager.Instance.OpenMenu("loading");
   }

   public override void OnJoinedRoom()
   {
      
      MenuManager.Instance.OpenMenu("room");
      roomNameText.text = PhotonNetwork.CurrentRoom.Name;

      Player[] players = PhotonNetwork.PlayerList;

      foreach(Transform child in playerListContent)
      {
         Destroy(child.gameObject);
      }

      for(int i = 0; i < players.Count(); i++)
      {
         Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
      }
      startGameButton.SetActive(PhotonNetwork.IsMasterClient);
      start1.SetActive(PhotonNetwork.IsMasterClient);
      start2.SetActive(PhotonNetwork.IsMasterClient);
      start3.SetActive(PhotonNetwork.IsMasterClient);
      start4.SetActive(PhotonNetwork.IsMasterClient);
      
   }

   public override void OnMasterClientSwitched(Player newMasterClient)
   {
      startGameButton.SetActive(PhotonNetwork.IsMasterClient);
      start1.SetActive(PhotonNetwork.IsMasterClient);
      start2.SetActive(PhotonNetwork.IsMasterClient);
      start3.SetActive(PhotonNetwork.IsMasterClient);
      start4.SetActive(PhotonNetwork.IsMasterClient);
   }

   public override void OnCreateRoomFailed(short returnCode, string message)
   {
      errorText.text = "Room Creation Failed " + message;
      MenuManager.Instance.OpenMenu("error");
   }

   public void LeaveRoom()
   {
      PhotonNetwork.LeaveRoom();
      MenuManager.Instance.OpenMenu("loading");
   }

   public void JoinRoom(RoomInfo info)
   {
      PhotonNetwork.JoinRoom(info.Name);
      MenuManager.Instance.OpenMenu("loading");
      
     
   }
   public override void OnLeftRoom()
   {
      MenuManager.Instance.OpenMenu("title");
   }

   public override void OnRoomListUpdate(List<RoomInfo> roomList)
   {
      foreach(Transform trans in roomListContent)
      {
         Destroy(trans.gameObject);
      }

      for(int i = 0; i < roomList.Count; i++)
      {
         if(roomList[i].RemovedFromList)
            continue;
         Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
      }
   }

   public override void OnPlayerEnteredRoom(Player newPlayer)
   {
      Instantiate(playerListItemPrefab,playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
   }

   public void StartGame()
   {
      
      PhotonNetwork.LoadLevel(4);
   }

   public void StartGame1()
   {
      
      PhotonNetwork.LoadLevel(5);
   }

   public void StartGame2()
   {
      
      PhotonNetwork.LoadLevel(6);
      
   }

   public void StartGame3()
   {
      
      PhotonNetwork.LoadLevel(7);
      
   }

   public void StartGame4()
   {
      
      PhotonNetwork.LoadLevel(8);
      
   }
   public override void OnDisconnected(DisconnectCause cause)
   {
      base.OnDisconnected(cause);
        
   }

   public void Disconnect()
   {
        
        
      PhotonNetwork.Disconnect();
      OnDisconnected(DisconnectCause.DisconnectByClientLogic);
   }

   public void YouhouMenuback()
   {
      SceneManager.LoadScene(0);
      PhotonNetwork.Disconnect();
      Destroy(RoomManager);

   }
}
