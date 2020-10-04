using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class LiftFloorCheck : MonoBehaviour
    {
        [HideInInspector] public PhotonView photonView;
        public int Players = 0;
        public GameObject lift;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
                photonView.RPC("RPC_CharacterIn", RpcTarget.All);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PhotonView>().IsMine)
                photonView.RPC("RPC_CharacterOut", RpcTarget.All);
        }

        [PunRPC]
        void RPC_CharacterIn()
        {
            Players++;
            if(Players == 2)
                lift.GetComponent<LiftControll>().PlayersOnFloor = true;
        }

        [PunRPC]
        void RPC_CharacterOut()
        {
            Players--;
            if (Players == 0)
                lift.GetComponent<LiftControll>().PlayersOnFloor = false;
        }

    }
}