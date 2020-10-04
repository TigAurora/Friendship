using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class SoundWithAnim : MonoBehaviour
    {
        [Header("Components")]
        public GameObject[] otherObjs;

        public float TimeLoop;
        float time;
        public bool isPlaying = false, isStart = false, isLoop = false, isEnd = false;
        public Animator anim;
        public List<Animator> otherAnims;

        // Start is called before the first frame update
        void Start()
        {
            otherAnims = new List<Animator>();
            anim = GetComponent<Animator>();
            if (otherObjs.Length > 0)
            {
                for (int i = 0; i < otherObjs.Length; ++i)
                {
                    otherAnims.Add(otherObjs[i].GetComponent<Animator>());
                }
            }
            isStart = false;
        }

        void Update()
        {
            if (anim.enabled && !isStart)
            {
                isStart = true;
                isEnd = false;
                anim.SetTrigger("Play");
                for (int i = 0; i < otherAnims.Count; ++i)
                {
                    otherAnims[i].enabled = true;
                    otherAnims[i].SetTrigger("Play");
                }
            }
            if (!isPlaying && isStart && isLoop & !isEnd)
            {
                if (time >= TimeLoop)
                {
                    anim.SetTrigger("Play");
                    for (int i = 0; i < otherAnims.Count; ++i)
                    {
                        otherAnims[i].SetTrigger("Play");
                    }
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

        //Stop ALL (anim & sound & loop)
        public void End()
        {
            isEnd = true;
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

        public void StartSoundFadein(float volume)
        {
            GetComponent<AudioSource>().enabled = true;
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().volume = Mathf.Lerp(0, volume, 1.5f);
        }

        public void FadeOutSound()
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0, 2f);
            StartCoroutine(FadeOutX());
        }

        IEnumerator FadeOutX()
        {
            yield return new WaitForSeconds(2.2f);
            GetComponent<AudioSource>().Stop();
        }

    }
}
