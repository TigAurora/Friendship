using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Friendship
{
    public class LevelsNetwork : MonoBehaviour
    {

        [Header("Components")]
        public LevelsNetworkComponents components;
        PlayerCamera playerCamera;

        //Signleton
        public static LevelsNetwork Instance;

        #region //Unity
        private void Awake()
        {
            SingletonInit();
            //Get components
            components.LevelAManager = GetComponent<LevelsManager>();
            components.photonView = GetComponent<PhotonView>();
            components.BlindView = GetComponent<BlindView>();

            //Init massive of players
            components.players = new GameObject[2];
        }

        void OnApplicationQuit()
        {
            Instance.Disconnect();
        }

        [RuntimeInitializeOnLoadMethod]

        #endregion

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        void Start()
        {
        }

        public void StartGame()
        {
            Debug.Log("LevelsNetwork startgame");
            StartCoroutine(WaitforPositionReset());
            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {
                components.BlindView.TurnBlack("Background");
                components.BlindView.TurnBlack("Floor");
                components.BlindView.TurnBlack("Player");
                components.BlindView.TurnOpposite("Item");
                components.BlindView.TurnOpposite("SoundSource");
                components.BlindView.TurnOpposite("SoundSourceComp");
            }
            else if (PlayerPrefs.GetInt("myCharacter") == 1)
            {
                Camera.main.GetComponent<AudioListener>().enabled = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //Reset position after entering lobby
        IEnumerator WaitforPositionReset()
        {
            SetPlayersPositions(1);
            SyncPlayerUI();
            yield return new WaitForSeconds(1f);
            PlayerNetwork.Instance.photonView.RPC("RPC_FinishLevelPositionSet", RpcTarget.MasterClient);
            components.LevelAManager.isGame = true;
            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {
                playerCamera = Camera.main.GetComponent<PlayerCamera>();
                playerCamera.player = components.players[0].transform;
            }
            else
            {
                playerCamera = Camera.main.GetComponent<PlayerCamera>();
                playerCamera.player = components.players[1].transform;
            }
        }

        public void SetPlayersPositions(int whichLevel)
        {
            //Find players in scene
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            //Sort by playerID
            foreach (GameObject player in players)
            {
                if (player.name.Contains("blindplayer"))
                    components.players[0] = player.gameObject;
                else if(player.name.Contains("deafplayer"))
                    components.players[1] = player.gameObject;
            }

            //Send all to do reset players positions
            components.photonView.RPC("RPC_SetPlayersPositions", RpcTarget.All, whichLevel);
        }

        public void SyncPlayerUI()
        {
            //Find players in scene
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            foreach (GameObject player in players)
            {
                if (player.GetComponent<PhotonView>().IsMine)
                {
                    player.GetComponent<PlayerData>().Confirmcreation();
                }
            }
        }

        //Disconnect method
        public void Disconnect()
        {
            //We send to all that we disconnected
            components.photonView.RPC("RPC_Disconnect", RpcTarget.All);

            PhotonNetwork.LeaveRoom(); //Leave room in photon
            Destroy(PlayerNetwork.Instance.gameObject); //Destroy playernetwork

            PhotonNetwork.LoadLevel(0); //Go to MainMenu
        }

        #region //RPC

        [PunRPC]
        void RPC_Disconnect()
        {
            //If you disconnect, you send this
            components.LevelAManager.EndGame(); //And game end
        }

        [PunRPC]
        void RPC_SetPlayersPositions(int whichLevel)
        {
            //Reset cached player positions
            components.players[0].transform.position = components.blindPlayerStartPosition[whichLevel-1].position;
            components.players[0].transform.localScale = new Vector3(0.75f, 0.75f, 1);

            components.players[1].transform.position = components.deafPlayerStartPosition[whichLevel - 1].position;
            components.players[1].transform.localScale = new Vector3(0.75f, 0.75f, 1);
            //Debug.Log("blind.transform.position:" + components.players[0].transform.position + ", deaf.transform.position:" + components.players[1].transform.position);
            foreach (Transform obj in components.players[0].GetComponent<Transform>())
            {
                obj.gameObject.SetActive(true);
            }
            foreach (Transform obj in components.players[1].GetComponent<Transform>())
            {
                obj.gameObject.SetActive(true);
            }
        }

        #endregion

        //Components container
        [Serializable]
        public struct LevelsNetworkComponents
        {
            [HideInInspector] public PhotonView photonView;
            [HideInInspector] public LevelsManager LevelAManager;
            [HideInInspector] public BlindView BlindView;

            [Header("Components")]
            public GameObject[] players;
            public Transform[] blindPlayerStartPosition, deafPlayerStartPosition;
        }
    }
}
