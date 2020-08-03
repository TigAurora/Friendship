using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System;
using UnityEngine.UI;

namespace Friendship
{
    public class Popups : MonoBehaviour
    {

        [Header("Popups")]
        public GameObject confirmp;
        public GameObject requestconfirmp;
        public Animator requestanimator;
        public Animator confirmanimator;
        public TMP_Text requestText;
        public TMP_Text confirmText;
        public GameObject trans;

        public GameObject Cancel;
        public GameObject yes;
        public GameObject no;


        [HideInInspector] public static Popups Instance;
        [HideInInspector] public PhotonView photonView;

        private string waiting, deny, acceptrequest, cancelrequest;

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        void Awake()
        {
            photonView = GetComponent<PhotonView>();
        }

        // Start is called before the first frame update
        void Start()
        {
            waiting = "Waiting for another player to confirm...";
            deny = "Another player denied the request";
            acceptrequest = "Another player request for entering the game";
            cancelrequest = "Another Player cancels the request";
        }

        // Update is called once per frame
        void Update()
        {
        }

        //OnClick functions

        public void onClick_CancelRequest()
        {
            requestanimator.SetTrigger("close");
            photonView.RPC("RPC_CancelRequest", RpcTarget.Others);
            trans.SetActive(false);
            requestconfirmp.SetActive(false);
        }

        public void onClick_Requestconfirm()
        {
            requestconfirmp.SetActive(true);
            Cancel.GetComponent<Button>().interactable = true;
            trans.SetActive(true);
            requestText.text = waiting;
            requestanimator.SetTrigger("pop");
            photonView.RPC("RPC_RequestConfirm", RpcTarget.Others);
        }

        public void onClick_ConfirmYes()
        {
            if (PlayerNetwork.Instance.photonView.IsMine)
            {
                PlayerNetwork.Instance.photonView.RPC("RPC_StartLoading", RpcTarget.All);
            }
            photonView.RPC("RPC_RequestConfirmed", RpcTarget.All);
        }

        public void onClick_ConfirmNo()
        {
            photonView.RPC("RPC_RequestDenied", RpcTarget.Others);
            confirmp.SetActive(false);
            trans.SetActive(false);
        }

        #region //RPC

        //receiving request
        [PunRPC]
        void RPC_RequestConfirm()
        {
            confirmText.text = acceptrequest;
            confirmp.SetActive(true);
            trans.SetActive(true);
            confirmanimator.SetTrigger("pop");
            yes.GetComponent<Button>().interactable = true;
            no.GetComponent<Button>().interactable = true;
        }

        [PunRPC]
        void RPC_RequestDenied()
        {
            requestText.text = deny;
            StartCoroutine(waitforsecs(1f, requestanimator, requestconfirmp, "cancel"));
        }

        [PunRPC]
        void RPC_RequestConfirmed()
        {
            //confirmanimator.SetTrigger("close");
            //requestanimator.SetTrigger("close");
            //requestconfirmp.SetActive(false);
            //confirmp.SetActive(false);
            //trans.SetActive(false);
            PhotonNetwork.LoadLevel("LevelA");
        }

        [PunRPC]
        void RPC_CancelRequest()
        {
            confirmText.text = cancelrequest;
            StartCoroutine(waitforsecs(1f, confirmanimator, confirmp, "yesorno"));
        }

        #endregion

        //Supporting functions
        IEnumerator waitforsecs(float sec, Animator anim, GameObject obj, string whichtoban)
        {
            if (whichtoban == "yesorno")
            {
                yes.GetComponent<Button>().interactable = false;
                no.GetComponent<Button>().interactable = false;
            }
            else if (whichtoban == "cancel")
            {
                Cancel.GetComponent<Button>().interactable = false;
            }
            yield return new WaitForSeconds(sec);
            anim.SetTrigger("close");
            yield return new WaitForSeconds(0.2f);
            obj.SetActive(false);
            trans.SetActive(false);
        }

    }
}