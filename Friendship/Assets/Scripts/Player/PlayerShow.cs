using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class PlayerShow : MonoBehaviour
    {

        // Start is called before the first frame update
        void Start()
        {

            if (GetComponent<PhotonView>().IsMine)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
