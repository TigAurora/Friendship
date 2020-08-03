using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class ItemTrigger : MonoBehaviour
    {
        public GameObject triggeriTem;
        public string whichTrigger;
        public PhotonView photonView;

        void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        // Start is called before the first frame update
        void OnTriggerEnter2D(Collider2D other)
        {
            if (whichTrigger == "gravity")
            {
                if (other.tag == "Player")
                {
                    triggeriTem.GetComponent<Rigidbody2D>().gravityScale = 1;
                    photonView.RPC("RPC_onTrigger", RpcTarget.All);
                }
            }
            else if (whichTrigger == "animation&sound")
            {
                if (other.tag == "Player")
                {
                    triggeriTem.GetComponent<Animator>().enabled = true;
                    gameObject.SetActive(false);
                }
            }
        }

        [PunRPC]
        void RPC_onTrigger()
        {
            gameObject.SetActive(false);
        }
    }
}
