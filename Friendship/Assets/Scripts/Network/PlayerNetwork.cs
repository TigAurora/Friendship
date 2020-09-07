using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Sprites;

namespace Friendship
{
    public class PlayerNetwork : MonoBehaviour
    {

        public int myCharacter;

        //public GameObject[] allCharacters;

        //Signleton
        public static PlayerNetwork Instance;
        public GameObject loading;

        [HideInInspector] public PhotonView photonView;

        public string nickname;
        int playersInGame, levelPositionFinish;

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
            //if (Instance != null && PlayerPrefs.HasKey("myCharacter"))
            //{
                //Instance.myCharacter = PlayerPrefs.GetInt("myCharacter");
            //}
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

        public void Set_Character(int which)
        {
            myCharacter = which;
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
                //SceneManager.sceneLoaded -= OnSceneFinishedLoading;

                if (IsMaster())
                    MasterLoadedGame("CharacterCreator");
                else
                    NonMasterLoadedGame("CharacterCreator");
            }
            if (scene.name == "LevelA")
            {
                //Remove event from manager 
                SceneManager.sceneLoaded -= OnSceneFinishedLoading;
                Debug.Log("scene.name == LevelA");
                if (IsMaster())
                    MasterLoadedGame("LevelA");
                else
                    NonMasterLoadedGame("LevelA");
            }
        }

        #endregion

        void MasterLoadedGame(string scene)
        {
            //Master loaded first and wait another
            photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient, scene); //Send to master "Im loaded"
            photonView.RPC("RPC_LoadGameOthers", RpcTarget.Others, scene); //Send others players to start load scene
        }

        void NonMasterLoadedGame(string scene)
        {
            photonView.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient, scene); //Send to master "Im loaded"
        }

        #region //RPC

        //Load scene method
        [PunRPC]
        void RPC_StartLoading()
        {
            loading.SetActive(true);
        }

        [PunRPC]
        public void RPC_FinishLoading()
        {
            //if (loading.activeSelf)
                //Debug.Log("它曾存在过！！");
            //else
                //Debug.Log("它不曾存在过？？？");

            loading.SetActive(false);
        }

        [PunRPC]
        void RPC_FinishLevelPositionSet()
        {
            levelPositionFinish++; //1 player finish positioning
            Debug.Log("finishlevel outside");

            if (levelPositionFinish == PhotonNetwork.PlayerList.Length)
            {
                photonView.RPC("RPC_FinishLoading", RpcTarget.All); //Send to all "RPC_FinishLoading"
                levelPositionFinish = 0;
            }
        }

        [PunRPC]
        void RPC_LoadGameOthers(string scene)
        {
            PhotonNetwork.LoadLevel(scene);
        }

        //Loaded scene method
        [PunRPC]
        void RPC_LoadedGameScene(string scene)
        {
            playersInGame++; //Plus 1 player

            //Check loaded players
            if (playersInGame == PhotonNetwork.PlayerList.Length)
            {
                photonView.RPC("OnPlayersLoaded", RpcTarget.All, scene); //Send to all "All loaded"
            }
        }

        //Start game method
        [PunRPC]
        void OnPlayersLoaded(string scene)
        {
            Debug.Log("OnPlayerLoaded scene name = " + scene);
            if (scene == "CharacterCreator")
                CharactorCreator.Instance.StartGame();
            else if (scene == "LevelA")
                LevelsNetwork.Instance.StartGame();
            playersInGame = 0;
        }

        [PunRPC]
        void RPC_SelectCharacter(int character)
        {
            PlayerPrefs.SetInt("myCharacter", character);
            myCharacter = character;
        }

        #endregion
    }
}