using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class LiftOnTriggerEnter : MonoBehaviour
    {
        [Header("Lift")]
        public GameObject lift;
        public GameObject anotherDetect;

        public bool isLift = false;
        public int which, Players, whichPlayer;
        public bool[] PlayerInLift;

        [HideInInspector] public PhotonView photonView;
        // Start is called before the first frame update
        void Start()
        {
            whichPlayer = PlayerNetwork.Instance.myCharacter;
            photonView = GetComponent<PhotonView>();
            PlayerInLift = new bool[2] { false, false };
        }

        // Update is called once per frame
        void Update()
        {
            //photonView.RPC("RPC_NextAnim", RpcTarget.All);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player" && other.GetComponent<PhotonView>().IsMine && whichPlayer == which)
                photonView.RPC("RPC_CharacterGetIn", RpcTarget.All, whichPlayer);
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Player" && other.GetComponent<PhotonView>().IsMine && whichPlayer == which)
                photonView.RPC("RPC_CharacterGetOut", RpcTarget.All, whichPlayer);
        }

        [PunRPC]
        void RPC_NextAnim()
        {
            if (Players == 2)
            {
                lift.GetComponent<LiftControll>().SetParent(1);
                lift.GetComponent<LiftControll>().NextAnim();
                //lift.GetComponent<LiftControll>().SetCameraLimit("LifttoFF");
            }
        }


        [PunRPC]
        void RPC_CharacterGetIn(int character)
        {
            PlayerInLift[character] = true;
            anotherDetect.GetComponent<LiftOnTriggerEnter>().PlayerInLift[character] = true;
            anotherDetect.GetComponent<LiftOnTriggerEnter>().Players++;
            Players++;
            lift.GetComponent<LiftControll>().PlayersinLift = Players;
        }

        [PunRPC]
        void RPC_CharacterGetOut(int character)
        {
            PlayerInLift[character] = false;
            anotherDetect.GetComponent<LiftOnTriggerEnter>().PlayerInLift[character] = false;
            anotherDetect.GetComponent<LiftOnTriggerEnter>().Players--;
            Players--;
            lift.GetComponent<LiftControll>().PlayersinLift = Players;
        }
    }
}