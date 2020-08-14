using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class EyesMovingWithPlayer : MonoBehaviour
    {
        [Header("Position")]
        public Transform min;
        public Transform max;
        public Transform playerLastPos;
        public Transform DetectLimit;

        //0 for blind, 1 for deaf
        [Header("WhichPlayer")]
        public int whichPlayer;

        PhotonView photonView;
        GameObject detectPlayer;
        float time;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PhotonView>().IsMine)
                    {
                        if (PlayerPrefs.GetInt("myCharacter") == 0)
                        {
                            if (whichPlayer == 0)
                            {
                                detectPlayer = player;
                            }
                        }
                        else
                        {
                            if (whichPlayer == 1)
                            {
                                detectPlayer = player;
                            }
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (detectPlayer != null && LevelsManager.Instance.isGame)
            {
                if (time >= 0.05)
                {
                    DetectMove();
                    time = 0;
                }
                time += Time.deltaTime;
            }

        }

        void DetectMove()
        {
            float playermove = detectPlayer.transform.position.x - playerLastPos.position.x;
            if (Mathf.Abs(playermove) >= 0.02 && detectPlayer.transform.position.x <= DetectLimit.position.x)
            {
                playerLastPos.position = detectPlayer.transform.position;
                if (whichPlayer == 0)
                {
                    float eyemove = playermove * 0.8f;
                    if (transform.position.x + eyemove < min.position.x)
                    {
                        eyemove = min.position.x;
                    }
                    else if (transform.position.x + eyemove > max.position.x)
                    {
                        eyemove = max.position.x;
                    }
                    else
                    {
                        eyemove += transform.position.x;
                    }
                    Vector2 move = new Vector2(eyemove, transform.position.y);
                    photonView.RPC("RPC_SyncEyeMove", RpcTarget.All, move, whichPlayer);
                }
                else if (whichPlayer == 1)
                {
                    float eyemove = playermove;
                    eyemove *= -1;
                    if (transform.position.y + eyemove < min.position.y)
                    {
                        eyemove = min.position.y;
                    }
                    else if (transform.position.y + eyemove > max.position.y)
                    {
                        eyemove = max.position.y;
                    }
                    else
                    {
                        eyemove += transform.position.y;
                    }
                    Vector2 move = new Vector2(transform.position.x, eyemove);
                    photonView.RPC("RPC_SyncEyeMove", RpcTarget.All, move, whichPlayer);
                }
            }
        }

        [PunRPC]
        void RPC_SyncEyeMove(Vector2 move, int which)
        {
            if (which == whichPlayer)
            {
                transform.position = Vector2.Lerp(transform.position, move, 0.1f);
            }
        }
    }
}
