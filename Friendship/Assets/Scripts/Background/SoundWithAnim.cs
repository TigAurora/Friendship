﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class SoundWithAnim : MonoBehaviour
    {
        public float TimeLoop;
        public float time;
        public bool isPlaying = false, isStart = false, isLoop = false;
        public Animator anim;

        // Start is called before the first frame update
        void Start()
        {
            anim = GetComponent<Animator>();
            isStart = false;
        }

        void Update()
        {
            if (anim.enabled && !isStart)
            {
                isStart = true;
                anim.SetTrigger("Play");
            }
            if (!isPlaying && isStart && isLoop)
            {
                if (time >= TimeLoop)
                {
                    anim.SetTrigger("Play");
                    time = 0;
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
        }

        //Use at the beginning of the animation, ONLY use for anim with sound
        public void AnimStart()
        {
            StartSound();
            isPlaying = true;
        }
        //Use at the end of the animation, MUST be used by all anim
        public void AnimEnd()
        {
            anim.SetTrigger("End");
            isPlaying = false;
        }

        public bool isSound()
        {
            return GetComponent<AudioSource>().enabled;
        }

        public bool isAnim()
        {
            return GetComponent<Animator>().enabled;
        }



        public void StartSound()
        {
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().Play();
        }

    }
}
