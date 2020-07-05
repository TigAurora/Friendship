using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace TheBattle
{
    public class MainMenuNetwork : MonoBehaviourPunCallbacks
    {
        [Header("Components")]
        public MainMenuNetworkComponents components;

        //Unity methods container
        #region //Unity
        private void Awake()
        {
            components.menuManager = GetComponent<MainMenuManager>();

            //If connect == false
            if (!PhotonNetwork.IsConnected)
                ConnectToPhoton(); //Connect to photon servers
        }
        #endregion

        //Connect to photon servers
        void ConnectToPhoton()
        {
            //connect to photon using setting file in project
            PhotonNetwork.ConnectUsingSettings();

            //Disable automatic player scene transition
            PhotonNetwork.AutomaticallySyncScene = false;
        }

        //Create room method
        public void CreateRoom(string roomName, bool isVisible, int scoreToWin)
        {
            //Creating custom room options
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 2; //Set max players in room
            roomOptions.IsVisible = isVisible; //Make visible in room list       

            PhotonNetwork.CreateRoom(roomName, roomOptions, null); //Create room in photon
        }

        //Join room method
        public void JoinToRoom(string roomName)
        {
            PhotonNetwork.JoinRoom(roomName); //Join room in photon by name
        }

        //Leave current room method
        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom(false);
        }

        //Start game method
        public void StartGame()
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false; //Close room
                PhotonNetwork.LoadLevel("CharacterCreator"); //Load scene
            }
            else
                Debug.Log("Need 1 more player.");
        }

        //Events container
        #region //Events

        //Calls when connected to master
        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby(); //If connected to master, make connect to lobby
        }

        //Calls when connected to lobby
        public override void OnJoinedLobby()
        {
            PhotonNetwork.NickName = "Player" + UnityEngine.Random.Range(100, 99999); //Set random name
            components.menuManager.OnConnectedToLobby(); //Call menu event to draw UI
        }

        //Calls when anyone create or close room
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            //Call menu manager to update UI list of rooms
            components.menuManager.UpdateCachedRoomList(roomList);
            components.menuManager.GetRoomListView();
        }

        //Calls when YOU join in room
        public override void OnJoinedRoom()
        {
            //Call menu manager to draw UI
            components.menuManager.OnJoinedToRoom(PhotonNetwork.PlayerList);
            components.menuManager.RoomMasterCheck(); //Check room master
        }

        //Calls when another player entered in room
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            //Call menu to update UI
            components.menuManager.GetPlayerList(PhotonNetwork.PlayerList);
        }

        //Calls when another player leave room
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            //Call menu to update UI
            components.menuManager.GetPlayerList(PhotonNetwork.PlayerList);
            components.menuManager.RoomMasterCheck();
        }
        #endregion

    }

    //Components container
    [Serializable]
    public struct MainMenuNetworkComponents
    {
        [HideInInspector] public MainMenuManager menuManager;
    }
}