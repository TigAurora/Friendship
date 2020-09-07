using Photon.Pun;
using UnityEngine;

namespace Friendship
{
    public class DetectItem : MonoBehaviour
    {
        PhotonView photonView;
        public GameObject item;

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
            //is not being taken by this other
            if (other.tag == "Player")
            {
                //Debug.Log(transform.name + " OnTrigger Enter " + other.name);
                photonView.RPC("RPC_SyncEnterItems", RpcTarget.All, other.name);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            //is not being taken by this other
            if (other.tag == "Player" && item.GetComponent<itemState>().pickuphand != other.GetComponent<PlayerController>().pickuphand && !other.GetComponent<PlayerController>().isPick)
            {
                //Debug.Log(transform.name + " OnTrigger Exit " + other.name);
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
                    if (!p.GetComponent<PlayerController>().pickableitems.Contains(item))
                        p.GetComponent<PlayerController>().pickableitems.Add(item);
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
                    if (p.GetComponent<PlayerController>().pickableitems.Contains(item))
                        p.GetComponent<PlayerController>().pickableitems.Remove(item);
                }
            }
        }
    }
}