using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Friendship
{
    public class LevelLobbyNetwork : MonoBehaviour
    {

        [Header("Components")]
        public LevelLobbyNetworkComponents components;

        //Signleton
        public static LevelLobbyNetwork Instance;

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

        // Start is called before the first frame update
        void Start()
        {
            SetPlayersPositions();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //Reset position after entering lobby
        public void SetPlayersPositions()
        {
            //Find players in scene
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            //Sort by playerID
            foreach (GameObject player in players)
            {
                if (player.name.Contains("blind"))
                    components.players[0] = player.gameObject;
                else
                    components.players[1] = player.gameObject;
            }

            //Send all to do reset players positions
            components.photonView.RPC("RPC_SetPlayersPositions", RpcTarget.All);
        }

        #region //RPC

        [PunRPC]
        void RPC_SetPlayersPositions()
        {
            //Reset cached player positions
            components.players[0].transform.position = components.blindPlayerStartPosition.position;
            components.players[1].transform.position = components.deafPlayerStartPosition.position;
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
        public struct LevelLobbyNetworkComponents
        {
            [HideInInspector] public PhotonView photonView;
            [HideInInspector] public LevelLobbyManager LevelManager;

            [Header("Components")]
            public GameObject[] players;
            public Transform blindPlayerStartPosition, deafPlayerStartPosition;
        }
    }
}
