using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class DetectGrab : MonoBehaviour
    {
        public GameObject player;
        // Start is called before the first frame update
        void Start()
        {
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //is not being taken by this other
            if (other.tag == "Item" && player.GetComponent<PlayerController>().CheckPickAnim && player.GetComponent<PlayerController>().isPick && player.GetComponent<PlayerController>().iteminhand.gameObject.name == other.gameObject.name)
            {
                Debug.Log(transform.name + " detect anim " + other.name);
                //player.GetComponent<PlayerController>().pointIK.position = player.GetComponent<PlayerController>().pickuphand.position;
                //player.GetComponent<PlayerController>().pickupIK.position = new Vector3(0,0,0);
                player.GetComponent<PlayerController>().Pickup(2);
                //player.GetComponent<PlayerController>().CorrectLeftArm();
            }
        }
    }
}
