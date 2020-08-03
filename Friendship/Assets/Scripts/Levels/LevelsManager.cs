using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Pun;
using Photon.Realtime;

namespace Friendship
{
    public class LevelsManager : MonoBehaviour
    {
        [Header("Components")]
        public LevelsComponents components; //components of this class 

        public bool isPause = false, isGame; //Game controllers

        public static LevelsManager Instance; //Singleton

        private void Awake()
        {
            SingletonInit(); //Singleton initialization

            //Get components
            components.networkManager = GetComponent<LevelsNetwork>();
            //components.uIManager = GetComponent<GameUIManager>();
        }

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            isGame = true;
        }

        //Pause method
        public void Pause()
        {
            isPause = !isPause;
            //components.uIManager.Pause(isPause); //UI Draw
        }

        private void Update()
        {
            if (InputManager.Pause)
            {
                Pause();
            }
        }

        //Components container
        [Serializable]
        public struct LevelsComponents
        {
            [HideInInspector] public LevelsNetwork networkManager;
            //[HideInInspector] public GameUIManager uIManager;

            [Header("Parameters")]
            public int scoreToWin;
            public int firstPlayerScore, secondPlayerScore;

        }
    }
}