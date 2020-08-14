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
        public bool isGround = false;
        public float maxVel = 0;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            //halfheight = GetComponent<Collider2D>().bounds.extents.y - 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
            if (pickuphand != null)
            {
                this.gameObject.transform.position = pickuphand.position;
            }
            if (!isGround)
            {
                if (Mathf.Abs(transform.GetComponent<Rigidbody2D>().velocity.y) > maxVel)
                {
                    maxVel = Mathf.Abs(transform.GetComponent<Rigidbody2D>().velocity.y);
                }
            }
        }
    }
}