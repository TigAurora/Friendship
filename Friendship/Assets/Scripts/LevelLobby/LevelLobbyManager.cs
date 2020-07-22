using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Friendship
{
    public class LevelLobbyManager : MonoBehaviour
    {
        [Header("Components")]
        public LevelLobbyComponents components; //components of this class 

        public bool isPause = false, isLobby; //Game controllers

        public static LevelLobbyManager Instance; //Singleton

        private void Awake()
        {
            SingletonInit(); //Singleton initialization

            //Get components
            components.networkManager = GetComponent<LevelLobbyNetwork>();
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
            isLobby = true;

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
        public struct LevelLobbyComponents
        {
            [HideInInspector] public LevelLobbyNetwork networkManager;
            //[HideInInspector] public GameUIManager uIManager;

            [Header("Parameters")]
            public int scoreToWin;
            public int firstPlayerScore, secondPlayerScore;

        }
    }
}