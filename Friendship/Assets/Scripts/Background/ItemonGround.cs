using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class ItemonGround : MonoBehaviour
    {
        public string floorType;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            //is not being taken by this other
            if (other.gameObject.tag == "Item")
            {
                other.gameObject.GetComponent<itemState>().isGround = true;
                LevelsAudioManager.Instance.SetClip(other.gameObject, floorType);
            }
        }

        void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.tag == "Item")
            {
                other.gameObject.GetComponent<itemState>().isGround = false;
            }
        }
    }
}