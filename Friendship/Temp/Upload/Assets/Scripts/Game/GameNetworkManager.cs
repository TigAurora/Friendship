using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Friendship
{
    public class GameNetworkManager : MonoBehaviourPunCallbacks
    {
        [Header("Components")]
        public GameNetworkManagerComponents components;

        //Unity methods container
        #region //Unity
        private void Awake()
        {
            //Get components
            components.gameManager = GetComponent<GameManager>();
            components.photonView = GetComponent<PhotonView>();

            //Init massive of players
            components.players = new GameObject[2];
        }
        #endregion

        //Get room custom propeties given when room created
        public object GetRoomProperties(string id)
        {
            //Get data
            ExitGames.Client.Photon.Hashtable hashtable = PhotonNetwork.CurrentRoom.CustomProperties;

            //Return data by ID
            return hashtable[id];
        }

        //Disconnect method
        public void Disconect()
        {
            //We send to all that we disconnected
            components.photonView.RPC("RPC_Disconnect", RpcTarget.All);

            PhotonNetwork.LeaveRoom(); //Leave room in photon
            Destroy(PlayerNetwork.Instance.gameObject); //Destroy playernetwork

            PhotonNetwork.LoadLevel(0); //Go to MainMenu
        }

        //Reset position after die
        public void ResetPlayersPositions()
        {
            //Find players in scene
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

            //Sort by playerID
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<PlayerData>().playerID == 0)
                    components.players[0] = players[i];
                else
                    components.players[1] = players[i];
            }

            //Send all to do reset players positions
            components.photonView.RPC("RPC_ResetPlayersPositions", RpcTarget.All);
        }

        //Events container
        #region //Events

        //Start game event
        public void OnGame_Started()
        {
            //If you master(room owner)
            if (PhotonNetwork.IsMasterClient)
            {
                //Spawm in scene player1 prefab
                PhotonNetwork.Instantiate(Path.Combine("Prefabs/Player", "Player P1"), components.firstPlayerStartPosition.position, Quaternion.identity, 0);
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs/Player", "Player P2"), components.secondPlayerStartPosition.position, Quaternion.identity, 0);
            }
        }

        //End game event
        public void OnGame_Ended()
        {
            //Send all game is end
            components.photonView.RPC("RPC_Game_End", RpcTarget.All);
        }

        #endregion

        #region //RPC

        [PunRPC]
        void RPC_Disconnect()
        {
            //If you disconnect, you send enemy this
            components.gameManager.EndGame(Winner.EnemyLeave); //And game end
        }

        [PunRPC]
        void RPC_Game_End()
        {
            //Check winner
            if (GameManager.Instance.GetScore(0) >= GameManager.Instance.components.scoreToWin)
                components.gameManager.EndGame(Winner.FirstPlayer);
            else if (GameManager.Instance.GetScore(1) >= GameManager.Instance.components.scoreToWin)
                components.gameManager.EndGame(Winner.SecondPlayer);
        }

        [PunRPC]
        void RPC_ResetPlayersPositions()
        {
            //Reset cached player positions
            components.players[0].transform.position = components.firstPlayerStartPosition.position;
            components.players[1].transform.position = components.secondPlayerStartPosition.position;
        }

        #endregion

    }

    //Components container
    [Serializable]
    public struct GameNetworkManagerComponents
    {
        [HideInInspector] public PhotonView photonView;
        [HideInInspector] public GameManager gameManager;

        [Header("Components")]
        public GameObject[] players;
        public Transform firstPlayerStartPosition, secondPlayerStartPosition;
    }
}