using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

namespace Friendship
{
    public class PlayerNetwork : MonoBehaviour
    {

        public int myCharacter;

        //public GameObject[] allCharacters;

        //Signleton
        public static PlayerNetwork Instance;

        [HideInInspector] public PhotonView photonView;

        public string nickname;
        int playersInGame;

        #region //Unity
        private void Awake()
        {
            SingletonInit();

            photonView = GetComponent<PhotonView>();
            SceneManager.sceneLoaded += OnSceneFinishedLoading; //Add event to scene manager

            //How many per second should send packaged
            PhotonNetwork.SendRate = 60;
            PhotonNetwork.SerializationRate = 30;
        }
        void Start()
        {
            if (Instance != null && PlayerPrefs.HasKey("myCharacter"))
            {
                Instance.myCharacter = PlayerPrefs.GetInt("myCharacter");
            }
        }
        #endregion

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        //Set nickname method
        public void Set_NickName(string name)
        {
            //Set local name
            nickname = name;
            //Set Photon name
            PhotonNetwork.NickName = name;
        }

        //Master check method
        public bool IsMaster()
        {
            if (PhotonNetwork.IsMasterClient)
                return true;
            else
                return false;
        }

        //Events container
        #region //Events

        //Calls when scene finished loading
        void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            //Check scene
            if (scene.name == "CharacterCreator")
            {
                //Remove event from manager 
                SceneManager.sceneLoaded -= OnSceneFinishedLoading;

                if (IsMaster())
                    MasterLoadedGame();
                else
                    NonMasterLoadedGame();
            }
        }

        #endregion

        void MasterLoadedGame()
        {
            //Master loaded first and wait another
            photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient); //Send to master "Im loaded"
            photonView.RPC("RPC_LoadGameOthers", RpcTarget.Others); //Send others players to start load scene
        }

        void NonMasterLoadedGame()
        {
            photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient); //Send to master "Im loaded"
        }

        #region //RPC

        //Load scene method
        [PunRPC]
        void RPC_LoadGameOthers()
        {
            PhotonNetwork.LoadLevel("CharacterCreator");
        }

        //Loaded scene method
        [PunRPC]
        void RPC_LoadedGameScene()
        {
            playersInGame++; //Plus 1 player

            //Check loaded players
            if (playersInGame == PhotonNetwork.PlayerList.Length)
            {
                photonView.RPC("OnPlayersLoaded", RpcTarget.All); //Send to all "All loaded"
            }
        }

        //Start game method
        [PunRPC]
        void OnPlayersLoaded()
        {
            CharactorCreator.Instance.StartGame();
        }

        [PunRPC]
        void RPC_SelectCharacter(int character)
        {
            PlayerPrefs.SetInt("myCharacter", character);
        }

        #endregion
    }
}