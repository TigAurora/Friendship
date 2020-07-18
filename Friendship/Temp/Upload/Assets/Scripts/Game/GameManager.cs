using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Friendship
{
    //Types of winners
    public enum Winner { FirstPlayer, SecondPlayer, EnemyLeave }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; //Singleton

        [Header("Components")]
        public GameManagerComponents components; //components of this class 

        public bool isPause = false, isGame; //Game controllers

        //Unity methods container
        #region //Unity
        private void Awake()
        {
            SingletonInit(); //Singleton initialization

            //Get components
            components.networkManager = GetComponent<GameNetworkManager>();
            components.uIManager = GetComponent<GameUIManager>();
        }
        #endregion

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        //Method for set properties of the game(like score need to win)
        public void SetGameProperties()
        {
            //Get data from Room cache
            object scoreToWinObj = components.networkManager.GetRoomProperties("ScoreToWin");

            //Set data in manager
            components.scoreToWin = (int)scoreToWinObj;
        }

        //Update score method, when anyone die
        public void UpdateScore()
        {
            //Check for last round
            if (IsRoundLast())
                OnGame_Ended(); //if last round, it would start event for end game  

            //UI draw score
            components.uIManager.DrawScore();
        }

        //Get score method from int to string
        public string GetScore()
        {
            //Get 1st player score anm 2nd player
            string score = components.firstPlayerScore + " | " + components.secondPlayerScore;

            //return result
            return score;
        }

        //Method overloading to get only 1 score
        public int GetScore(int playerID)
        {
            //check id
            if (playerID == 0)
                return components.firstPlayerScore; //Return result
            else
                return components.secondPlayerScore;
        }

        //Last round check
        public bool IsRoundLast()
        {
            if (components.firstPlayerScore >= components.scoreToWin || components.secondPlayerScore >= components.scoreToWin)
                return true;
            else
                return false;
        }

        //StartGame method
        public void StartGame()
        {

            isGame = true; // Game started
            //components.networkManager.OnGame_Started();
        }

        //End Game method
        public void EndGame(Winner winner)
        {
            isGame = false;

            components.uIManager.EndGame(winner);
        }

        //Events container
        #region // Event

        //Method calls when round end
        public void OnRound_End(int playerID)
        {
            //Switch id to make plus 1 score winner
            switch (playerID)
            {
                case 0:
                    components.secondPlayerScore++;
                    break;
                case 1:
                    components.firstPlayerScore++;
                    break;
            }

            //Updating score
            UpdateScore();
        }

        //Method calls when game end
        public void OnGame_Ended()
        {
            //Call network manager to end
            components.networkManager.OnGame_Ended();
        }

        #endregion

        //Pause method
        public void Pause()
        {
            isPause = !isPause;
            components.uIManager.Pause(isPause); //UI Draw
        }

        private void Update()
        {
            if (InputManager.Pause)
            {
                Pause();
            }
        }

    }

    //Components container
    [Serializable]
    public struct GameManagerComponents
    {
        [HideInInspector] public GameNetworkManager networkManager;
        [HideInInspector] public GameUIManager uIManager;

        [Header("Parameters")]
        public int scoreToWin;
        public int firstPlayerScore, secondPlayerScore;

    }
}