using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class CollectApples : MonoBehaviour
    {
        [Header("Applelamps")]
        public GameObject[] appleLamps;
        public GameObject apple,applechild;
        public Sprite darkS, lightS;
        public int requiredApple;
        public int count = 0;
        [HideInInspector] public PhotonView photonView;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Item" && other.name == "apple" && !other.GetComponent<Rigidbody2D>())
            {
                //photonView.RPC("RPC_CollectApple", RpcTarget.All);
                appleLamps[count].GetComponent<SpriteRenderer>().sprite = lightS;
                appleLamps[count].GetComponent<AudioSource>().Play();
                ++count;
            }             
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Item" && other.name == "apple" && !other.GetComponent<Rigidbody2D>())
            {
                //photonView.RPC("RPC_DeleteApple", RpcTarget.All);
                Destroy(apple.gameObject);
                Destroy(applechild.gameObject);
            }
        }

        [PunRPC]
        void RPC_CollectApple()
        {

        }

        [PunRPC]
        void RPC_DeleteApple()
        {

        }
    }
}