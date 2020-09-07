using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{ 
    public class WalkingSound : MonoBehaviour
    {
        public GameObject footl;
        public GameObject footr;
        // Start is called before the first frame update
        void Start()
        {
        
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
