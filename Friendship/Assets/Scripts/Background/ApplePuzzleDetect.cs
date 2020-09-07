using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

namespace Friendship
{
    public class ApplePuzzleDetect : MonoBehaviour
    {
        [Header("G1Manager")]
        public GameObject G_manager;

        GAManager GAManager;

        // Start is called before the first frame update
        void Start()
        {
            GAManager = G_manager.GetComponent<GAManager>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
                if (other.gameObject.GetComponent<PlayerController>().iteminhand != null)
                {
                    if (other.gameObject.GetComponent<PlayerController>().iteminhand.transform.name == "apple")
                    {
                        GAManager.GA_puzzles[1] = true;
                        gameObject.SetActive(false);
                    }
                }

            }
        }
    }
}