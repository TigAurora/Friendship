using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

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

        [Header("G1Manager")]
        public GameObject G_manager;

        [Header("AnotherEye")]
        public GameObject AnotherEye;

        PhotonView photonView;
        GameObject detectPlayer;
        float time;
        public bool reachMax = false;
        GAManager GAManager;

        // Start is called before the first frame update
        void Start()
        {
            GAManager = G_manager.GetComponent<GAManager>();

            photonView = GetComponent<PhotonView>();
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length > 0)
            {
                foreach (GameObject player in players)
                {
                    if (player.GetComponent<PhotonView>().IsMine)
                    {
                        if (PlayerNetwork.Instance.myCharacter == 0)
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
            DetectPuzzle();
            if (detectPlayer != null && LevelsManager.Instance.isGame)
            {

                //if (time >= 0.05)
                //{
                    DetectMove();
                    //time = 0;
                //}
                //time += Time.deltaTime;
            }

        }

        void DetectPuzzle()
        {
            if (whichPlayer == 0)
            {
                //Debug.Log("transform:" + transform.position.x + ", max: " + max.position.x);
                if (Math.Abs(transform.position.x - max.position.x) < 0.005)
                {
                    reachMax = true;
                }
                else
                {
                    reachMax = false;
                }
            }
            else
            {
                //Debug.Log("transform:" + transform.position.x + ", max: " + max.position.x);
                if ((Math.Abs(transform.position.y - max.position.y) < 0.01))
                {
                    reachMax = true;
                }
                else
                {
                    reachMax = false;
                }
            }
            if (reachMax && AnotherEye.GetComponent<EyesMovingWithPlayer>().reachMax)
            {
                GAManager.GA_puzzles[0] = true;
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
                    float eyemove = playermove * 0.08f;
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
                    float eyemove = playermove * -0.08f;
                    if (!reachMax || eyemove < 0)
                    {
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
        }



        [PunRPC]
        void RPC_SyncEyeMove(Vector2 move, int which)
        {
            if (which == whichPlayer)
            {
                //transform.position = Vector2.Lerp(transform.position, move, 0.05f);
                transform.position = move;
            }
        }

    }
}
