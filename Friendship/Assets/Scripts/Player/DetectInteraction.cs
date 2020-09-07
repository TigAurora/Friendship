using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class DetectInteraction : MonoBehaviour
    {
        PhotonView photonView;
        public GameObject[] items;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }


        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                photonView.RPC("RPC_SyncEnterItems", RpcTarget.All, other.name);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                photonView.RPC("RPC_SyncExitItems", RpcTarget.All, other.name);
            }
        }


        [PunRPC]
        void RPC_SyncEnterItems(string player)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.name == player)
                {
                    foreach (GameObject item in items)
                    {
                        if (!p.GetComponent<PlayerController>().interactiveitems.Contains(item))
                            p.GetComponent<PlayerController>().interactiveitems.Add(item);
                    }
                }
            }
        }

        [PunRPC]
        void RPC_SyncExitItems(string player)
        {
            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject p in players)
            {
                if (p.name == player)
                {
                    foreach (GameObject item in items)
                    {
                        if (p.GetComponent<PlayerController>().interactiveitems.Contains(item))
                            p.GetComponent<PlayerController>().interactiveitems.Remove(item);
                    }
                }
            }
        }
    }
}
