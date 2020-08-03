using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class DetectItem : MonoBehaviour
    {
        private GameObject temp;
        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            //Debug.Log("OnTriggerEnter2D I met outside " + other.name);
            if (other.tag == "Item")
            {
                temp = other.transform.parent.gameObject;
                if(!GetComponent<PlayerController>().pickableitems.Contains(temp))
                    GetComponent<PlayerController>().pickableitems.Add(temp);
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.tag == "Item" && GetComponent<PlayerController>().isPick == false)
            {
                temp = other.transform.parent.gameObject;
                if (GetComponent<PlayerController>().pickableitems.Contains(temp))
                    GetComponent<PlayerController>().pickableitems.Remove(temp);
            }
        }


    }
}