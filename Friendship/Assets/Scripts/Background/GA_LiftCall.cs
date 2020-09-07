using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class GA_LiftCall : MonoBehaviour
    {
        [Header("Lights")]
        public GameObject[] Lights;
        public Sprite darkS;
        public Sprite lightS;

        [Header("Buttons")]
        public GameObject FrogB;
        public GameObject ChickB;

        [Header("G1Manager")]
        public GameObject G_manager;

        GAManager GAManager;
        public PhotonView photonView;

        float time, timeWaiting = 2;
        public bool isStart = false, isEnd = false, isNext = false, isBlind = false;
        public Animator anim;
        int whichSound = -1, passed = 0;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            GAManager = G_manager.GetComponent<GAManager>();
            anim = GetComponent<Animator>();
            isStart = false;
            if (PlayerNetwork.Instance.myCharacter == 0)
            {
                isBlind = true;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //photonView.RPC("RPC_Update", RpcTarget.All);
            if (anim.enabled && !isStart && isBlind)
            {
                RandomSound();
                photonView.RPC("RPC_Start", RpcTarget.All, whichSound);
            }
            if (!isEnd && isNext && isBlind)
            {
                if (time >= timeWaiting)
                {
                    RandomSound();
                    photonView.RPC("RPC_Update", RpcTarget.All, whichSound);
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
        }

        public void AnimEnd()
        {
            anim.SetTrigger("End");
        }

        public void buttonPressed(int whichButton)
        {
            //0 for frog, 1 for chicken
            if (whichButton == whichSound)
            {
                if (passed < 2)
                {
                    Lights[passed].GetComponent<SpriteRenderer>().sprite = lightS;
                    LevelsAudioManager.Instance.SetClip(Lights[passed], "correct");
                    ++passed;
                    isNext = true;
                }
                else if (passed == 2)
                {
                    Lights[passed].GetComponent<SpriteRenderer>().sprite = lightS;
                    LevelsAudioManager.Instance.SetClip(Lights[2], "correct");
                    //photonView.RPC("RPC_AllPassed", RpcTarget.All);
                    GAManager.GA_puzzles[3] = true;
                    isEnd = true;
                    FrogB.GetComponent<GA_Buttons>().Alldone();
                    ChickB.GetComponent<GA_Buttons>().Alldone();
                }
                //photonView.RPC("RPC_CorrectAnswer", RpcTarget.All);
            }
            else
            {
                //photonView.RPC("RPC_WrongAnswer", RpcTarget.All);
                LevelsAudioManager.Instance.SetClip(Lights[passed], "wrong");
                if (passed < 2)
                {
                    ++passed;
                }
                for (int i = 0; i < passed; ++i)
                {
                    Lights[i].GetComponent<SpriteRenderer>().sprite = darkS;
                }
                isNext = true;
                passed = 0;
            }
        }

        void RandomSound()
        {
            whichSound = Random.Range(0, 2);
        }

        [PunRPC]
        void RPC_CorrectAnswer()
        {
            if (passed < 2)
            {
                LevelsAudioManager.Instance.SetClip(Lights[passed], "correct");
                Lights[passed].GetComponent<Animator>().SetTrigger("Play");
                ++passed;
                isNext = true;
            }
            else if (passed == 2)
            {
                LevelsAudioManager.Instance.SetClip(Lights[2], "correct");
                Lights[2].GetComponent<Animator>().SetTrigger("Play");
                photonView.RPC("RPC_AllPassed", RpcTarget.All);
            }
        }

        [PunRPC]
        void RPC_WrongAnswer()
        {
            if (passed < 2)
            {
                ++passed;
            }
            LevelsAudioManager.Instance.SetClip(Lights[passed], "wrong");
            for(int i = 0; i < passed; ++i)
            {              
                Lights[i].GetComponent<Animator>().SetTrigger("Reset");
            }
            isNext = true;
            passed = 0;
        }

        [PunRPC]
        void RPC_Start(int which)
        {
            isStart = true;
            isEnd = false;
            if (which == 0)
            {
                LevelsAudioManager.Instance.SetClip(gameObject, "frog");
                whichSound = 0;
            }
            else
            {
                LevelsAudioManager.Instance.SetClip(gameObject, "chick");
                whichSound = 1;
            }
            anim.SetTrigger("Play");
        }

        [PunRPC]
        void RPC_Update(int which)
        {
            if (which == 0)
            {
                LevelsAudioManager.Instance.SetClip(gameObject, "frog");
                whichSound = 0;
            }
            else
            {
                LevelsAudioManager.Instance.SetClip(gameObject, "chick");
                whichSound = 1;
            }
            anim.SetTrigger("Play");
            time = 0;
            isNext = false;
        }

        [PunRPC]
        void RPC_AllPassed()
        {
            GAManager.GA_puzzles[3] = true;
            isEnd = true;
            FrogB.GetComponent<GA_Buttons>().Alldone();
            ChickB.GetComponent<GA_Buttons>().Alldone();
        }


        [PunRPC]
        void RPC_RandomSound(int which)
        {
            if (which == 0)
            {
                LevelsAudioManager.Instance.SetClip(gameObject, "frog");
                whichSound = 0;
            }
            else
            {
                LevelsAudioManager.Instance.SetClip(gameObject, "chick");
                whichSound = 1;
            }
        }

    }

}
