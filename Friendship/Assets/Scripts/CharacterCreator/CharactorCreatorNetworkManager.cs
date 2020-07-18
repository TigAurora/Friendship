using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using UnityEngine.SceneManagement;

namespace Friendship
{
    public class CharactorCreatorNetworkManager : MonoBehaviour
    {
        [Header("Components")]
        public CharactorCreatorNetworkComponents components;

        #region //Unity
        private void Awake()
        {
            //Get components
            //components.gameManager = GetComponent<GameManager>();
            components.photonView = GetComponent<PhotonView>();

            //Init massive of players
            components.players = new GameObject[2];
        }
        #endregion
        //Disconnect method
        public void Disconnect()
        {
            //We send to all that we disconnected
            components.photonView.RPC("RPC_Disconnect", RpcTarget.All);

            PhotonNetwork.LeaveRoom(); //Leave room in photon
            Destroy(PlayerNetwork.Instance.gameObject); //Destroy playernetwork

            PhotonNetwork.LoadLevel(0); //Go to MainMenu
        }

        //Events container
        #region //Events

        //Start game event
        public void OnGame_Started()
        {
            //If you blind character
            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {
                //Spawm in scene blindplayer prefab
                PhotonNetwork.Instantiate(Path.Combine("Prefabs/Player", "blindplayer"), components.blindPlayerStartPosition.position, Quaternion.identity, 0);
            }
            else
            {
                PhotonNetwork.Instantiate(Path.Combine("Prefabs/Player", "deafplayer"), components.deafPlayerStartPosition.position, Quaternion.identity, 0);
            }

            GameObject[] people = GameObject.FindGameObjectsWithTag("Player");
            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {

                for (int i = 0; i < people.Length; i++)
                {
                    if (people[i].name.Contains("deaf"))
                    {
                        people[i].SetActive(false);
                    }
                }
            }
            else
            {
                for (int i = 0; i < people.Length; i++)
                {
                    if (people[i].name.Contains("blind"))
                    {
                        people[i].SetActive(false);
                    }
                }
            }
        }

        //End game event
        public void OnGame_Ended()
        {
            //Send all game is end
            components.photonView.RPC("RPC_Game_End", RpcTarget.All);
        }

        [PunRPC]
        void RPC_Disconnect()
        {
            //If you disconnect, you send partner this
            //components.gameManager.EndGame(Winner.EnemyLeave); //And game end
        }

        [PunRPC]
        void RPC_Game_End()
        {
            //Check score
            //if (GameManager.Instance.GetScore(0) >= GameManager.Instance.components.scoreToWin)
                //components.gameManager.EndGame(Winner.FirstPlayer);
            //else if (GameManager.Instance.GetScore(1) >= GameManager.Instance.components.scoreToWin)
                //components.gameManager.EndGame(Winner.SecondPlayer);
        }

        #endregion
        // Update is called once per frame
        void Update()
        {

        }

        //Components container
        [Serializable]
        public struct CharactorCreatorNetworkComponents
        {
            [HideInInspector] public PhotonView photonView;
            [HideInInspector] public CharactorCreatorNetworkManager ccManager;

            [Header("Components")]
            public GameObject[] players;
            public Transform blindPlayerStartPosition, deafPlayerStartPosition;
        }
    }
}