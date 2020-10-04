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
                if(!items[3].GetComponent<AudioSource>().isPlaying)
                    photonView.RPC("RPC_SyncTeleInteraction", RpcTarget.All);
            }
        }

        IEnumerator delayComplete()
        {
            yield return new WaitForSeconds(1f);
            //PhoneBody
            items[0].GetComponent<SoundWithAnim>().End();
            //PhoneHandle
            items[1].GetComponent<Animator>().enabled = true;
            //TeleStop
            items[2].GetComponent<SoundWithAnim>().End();
            //items[4].GetComponent<Animator>().SetTrigger("MoveDown");
            yield return new WaitForSeconds(2.2f);
            GAManager.GA_puzzles[2] = true;
        }

        [PunRPC]
        void RPC_SyncTeleInteraction()
        {
            StartCoroutine(delayComplete());
        }

    }
}