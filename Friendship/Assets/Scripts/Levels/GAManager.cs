using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

namespace Friendship
{
    public class GAManager : MonoBehaviour
    {
        [Header("Puzzles Block")]
        public GameObject[] puzzleBlocks;
        PhotonView photonView;

        public bool[] GA_puzzles;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            GA_puzzles = new bool[99];
            for(int i=0; i < GA_puzzles.Length; ++i)
            {
                GA_puzzles[i] = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            photonView.RPC("RPC_SyncPuzzleBlocks", RpcTarget.All);
        }

        [PunRPC]
        void RPC_SyncPuzzleBlocks()
        {
            for (int i = 0; i < GA_puzzles.Length; ++i)
            {
                if (GA_puzzles[i] == true && puzzleBlocks[i].GetComponent<BoxCollider2D>().enabled)
                {
                    puzzleBlocks[i].GetComponent<AudioSource>().Play();
                    puzzleBlocks[i].GetComponent<SpriteRenderer>().enabled = false;
                    puzzleBlocks[i].GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
