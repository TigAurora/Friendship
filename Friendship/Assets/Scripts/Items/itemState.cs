using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class itemState : MonoBehaviour
    {
        public Transform pickuphand;
        PhotonView photonView;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        // Update is called once per frame
        void Update()
        {
            if (pickuphand != null)
            {
                this.gameObject.transform.position = pickuphand.position;
            }
        }
    }
}