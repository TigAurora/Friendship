using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace TheBattle
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Components")]
        public MainMenuComponents components;

        Animator activeScreen; //Cache current active screen

        //Unity methods container
        #region //Unity
        private void Awake()
        {
            components.menuNetwork = GetComponent<MainMenuNetwork>();
            components.cachedRoomList = new Dictionary<string, RoomInfo>();

            activeScreen = components.connectingScreenAnimator;
        }

        void ChangeScreen(Animator next)
        {
            activeScreen.Play("UI_Menu_Hide");

            activeScreen = next;
            next.Play("UI_Menu_Show");
        }
        #endregion

        //Check InputField for need lenght
        bool CheckField(InputField field, int lenght)
        {
            if (field.text.Length >= lenght)
                return true;
            else
                return false;
        }

        //Get players in room
        public void GetPlayerList(Player[] players)
        {
            //Clean
            for (int i = 0; i < components.playersGameObjectsList.Count; i++)
                Destroy(components.playersGameObjectsList[i]); //Remove old
            components.playersGameObjectsList.Clear(); //Clear list 

            //Create new players
            for (int i = 0; i < players.Length; i++)
            {
                //Create object in scene
                GameObject player = Instantiate(components.playerGameObject, components.playersRoomListParent);

                //Get Text component in new object
                Text playerNickname = player.GetComponentInChildren<Text>();
                playerNickname.text = players[i].NickName; //set nickname

                //Add this object to list
                components.playersGameObjectsList.Add(player);
            }
        }

        //Get room list
        public void GetRoomListView()
        {
            //Clean
            for (int i = 0; i < components.roomGameObjectsList.Count; i++)
                Destroy(components.roomGameObjectsList[i]); //Remove old
            components.roomGameObjectsList.Clear(); //Clear list 

            foreach (RoomInfo roomInfo in components.cachedRoomList.Values)
            {
                //Create new object
                GameObject room = Instantiate(components.roomGameObject, components.roomListParent);

                //Get Text component in new object
                Text roomName = room.GetComponentInChildren<Text>();
                roomName.text = roomInfo.Name; //Set room name

                //Get Button component in new object
                Button joinBtn = room.GetComponentInChildren<Button>();

                //Add button click event
                joinBtn.onClick.AddListener(() => components.menuNetwork.JoinToRoom(roomInfo.Name)); //Event: Join room by roomInfo.Name 

                //Add this object to list
                components.roomGameObjectsList.Add(room);
            }
        }

        //Update room list 
        public void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (components.cachedRoomList.ContainsKey(info.Name))
                    {
                        components.cachedRoomList.Remove(info.Name);
                    }
                    continue;
                }
                // Update cached room info
                if (components.cachedRoomList.ContainsKey(info.Name))
                {
                    components.cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    components.cachedRoomList.Add(info.Name, info);
                }
            }
        }

        //Check master
        public void RoomMasterCheck()
        {
            if (PlayerNetwork.Instance.IsMaster())
                components.startGameBtn.SetActive(true);
            else
                components.startGameBtn.SetActive(false);
        }

        //Events container
        #region //Events

        //Calls when connected to lobby
        public void OnConnectedToLobby()
        {
            //Change screen to main mneu
            ChangeScreen(components.menuScreenAnimator);
        }

        //Calls when YOU join to room
        public void OnJoinedToRoom(Player[] players)
        {
            //Change screen to room
            ChangeScreen(components.roomScreenAnimator);

            GetPlayerList(players);
        }
        #endregion

        //UI Events container
        #region //Events UI

        //Calls when click play
        public void OnClick_Play()
        {
            //Check field
            if (CheckField(components.nicknameField, 3))
            {
                //if nickname lenght > 3 change screen
                ChangeScreen(components.gameScreenAnimator);
                PlayerNetwork.Instance.Set_NickName(components.nicknameField.text); //Set nickname in photon
            }
            else
            {
                //Write in InputField error
                components.nicknameField.text = "";
                components.nicknameField.placeholder.GetComponent<Text>().text = "Length should > 3";
            }
        }

        //Calls when click Create room button
        public void OnClick_CreateRoom()
        {
            if (CheckField(components.roomNameCreateField, 3))
            {
                int scoreToWin = 0;

                //Check score InputField
                if (CheckField(components.roomScoreToWinField, 1))
                    scoreToWin = int.Parse(components.roomScoreToWinField.text);

                //If score <= 0 error
                if (scoreToWin <= 0)
                {
                    Debug.Log("Score must be more 0");
                    return;
                }

                //Create room in photon
                components.menuNetwork.CreateRoom(components.roomNameCreateField.text, components.roomVisibleToggle.isOn, scoreToWin);
            }
            else
            {
                //Error
                components.roomNameCreateField.text = "";
                components.roomNameCreateField.placeholder.GetComponent<Text>().text = "Name must be more 3";
            }
        }

        //Calls when select character
        public void OnClick_SelectCharacter(int character)
        {
            string[] characterlist = { "Blind", "Deaf" };
            if (PlayerNetwork.Instance.photonView.IsMine)
            {
                PlayerNetwork.Instance.photonView.RPC("RPC_SelectCharacter", RpcTarget.Others, 1 - character);
                PlayerPrefs.SetInt("myCharacter", character);
            }
            Debug.Log("Current character: " + characterlist[character]);
        }

        //Calls when click join room button
        public void OnClick_JoinToRoom()
        {
            components.menuNetwork.JoinToRoom(components.roomNameJoinField.text);
        }

        //Calls when click leave room button
        public void OnClick_LeaveRoom()
        {
            components.menuNetwork.LeaveRoom();
        }

        //Calls when click start game button
        public void OnClick_StartGame()
        {
            components.menuNetwork.StartGame();
        }

        //calls when click Browse room list
        public void OnClick_BrowseRoomsList()
        {
            ChangeScreen(components.roomListScreenAnimator);
        }


        public void OnClick_ChangeScreen(string screenName)
        {
            switch (screenName)
            {
                case "Connecting":
                    ChangeScreen(components.connectingScreenAnimator);
                    break;
                case "Menu":
                    ChangeScreen(components.menuScreenAnimator);
                    break;
                case "Game":
                    ChangeScreen(components.gameScreenAnimator);
                    break;
                case "Create":
                    ChangeScreen(components.creatingScreenAnimator);
                    break;
                case "Connect":
                    ChangeScreen(components.connectScreenAnimator);
                    break;
                case "RoomList":
                    ChangeScreen(components.roomListScreenAnimator);
                    break;
                case "Room":
                    ChangeScreen(components.roomScreenAnimator);
                    break;
            }
        }

        //Calls when click Exit
        public void OnClick_Exit()
        {
            Application.Quit();
        }

        #endregion
    }

    //Components container
    [Serializable]
    public struct MainMenuComponents
    {
        [HideInInspector] public MainMenuNetwork menuNetwork;

        [Header("Screens Animators")]
        public Animator connectingScreenAnimator;
        public Animator menuScreenAnimator;
        public Animator gameScreenAnimator;
        public Animator creatingScreenAnimator;
        public Animator connectScreenAnimator;
        public Animator roomListScreenAnimator;
        public Animator roomScreenAnimator;

        [Header("UI")]
        public InputField nicknameField;
        [Space]
        public InputField roomNameCreateField;
        public InputField roomNameJoinField;

        [Header("UI Room creating")]
        public InputField roomScoreToWinField;
        public Toggle roomVisibleToggle;

        [Header("UI Room")]
        public Transform playersRoomListParent;
        public GameObject playerGameObject;
        public List<GameObject> playersGameObjectsList;
        public GameObject startGameBtn;

        [Header("UI RoomList")]
        public Transform roomListParent;
        public GameObject roomGameObject;
        public Dictionary<string, RoomInfo> cachedRoomList;
        public List<GameObject> roomGameObjectsList;
    }
}