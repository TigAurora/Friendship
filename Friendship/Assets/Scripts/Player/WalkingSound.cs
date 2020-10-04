using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{ 
    public class WalkingSound : MonoBehaviour
    {
        public GameObject footl;
        public GameObject footr;
        public GameObject wheelchair;
        public string which;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        void Update()
        {
            if (which == "wheelchair")
            {
                if (wheelchair.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length <= wheelchair.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
                {
                    WheelChairStop();
                }
            }
        }

        public void WheelChairWalk()
        {
            wheelchair.GetComponent<AudioSource>().Play();
        }

        public void WheelChairStop()
        {
            wheelchair.GetComponent<AudioSource>().Stop();
        }

        public void footlWalk()
        {
            footl.GetComponent<AudioSource>().Play();
        }

        public void footrWalk()
        {
            footr.GetComponent<AudioSource>().Play();
        }
    }
}
