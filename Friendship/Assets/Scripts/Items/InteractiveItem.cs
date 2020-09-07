using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

namespace Friendship
{
    public class InteractiveItem : MonoBehaviour
    {
        [Header("GameObects")]
        public GameObject[] items;

        [Header("G1Manager")]
        public GameObject G_manager;

        GAManager GAManager;
        PhotonView photonView;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            GAManager = G_manager.GetComponent<GAManager>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void InteractiveAction()
        {
            if (transform.name.Contains("g1p3tablephone"))
            {
                photonView.RPC("RPC_SyncTeleInteraction", RpcTarget.All);
            }

        }

        [PunRPC]
        void RPC_SyncTeleInteraction()
        {
            //PhoneBody
            items[0].GetComponent<SoundWithAnim>().End();
            //PhoneHandle
            items[1].GetComponent<Animator>().enabled = true;
            //TeleStop
            items[2].GetComponent<SoundWithAnim>().End();
            items[3].GetComponent<SoundWithAnim>().End();
            //items[4].GetComponent<Animator>().SetTrigger("MoveDown");
            GAManager.GA_puzzles[2] = true;
        }

    }
}