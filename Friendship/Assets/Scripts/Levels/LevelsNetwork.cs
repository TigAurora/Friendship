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
            //components.gameManager = GetComponent<GameManager>();
            components.photonView = GetComponent<PhotonView>();

            //Init massive of players
            components.players = new GameObject[2];
        }
        #endregion

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void Start()
        {
            //Debug.Log("LevelsNetwork startgame");
            StartCoroutine(WaitforPositionReset());
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

        #region //RPC

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

            [Header("Components")]
            public GameObject[] players;
            public Transform[] blindPlayerStartPosition, deafPlayerStartPosition;
        }
    }
}
