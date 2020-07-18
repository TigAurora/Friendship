using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace Friendship
{
    public class GameUIManager : MonoBehaviour
    {
        [Header("Components")]
        public GameUIManagerComponents components; //components of this class 

        //Unity methods container
        #region //Unity
        private void Start()
        {
            //If build is mobile turn on mobile UI
#if UNITY_IOS || UNITY_ANDROID
            components.mobileUI.SetActive(true);
#endif
        }
        #endregion

        //Switch UI state
        public void Pause(bool isActive)
        {
            if (isActive)
                components.pauseScreen.SetActive(true);
            else
                components.pauseScreen.SetActive(false);
        }

        //Change screen method 
        void ChangeScreen(GameObject screenCurrent, GameObject screenNext)
        {
            screenCurrent.SetActive(false); //Disable current
            screenNext.SetActive(true); //Enable next
        }

        //Start game method
        public void StartGame()
        {
            ChangeScreen(components.loadedScreen, components.gameScreen);
        }

        //End game method
        public void EndGame(Winner winner)
        {
            //Check winner 
            switch (winner)
            {
                case Winner.FirstPlayer:
                    components.winnerText.text = "P1 Win!";
                    break;
                case Winner.SecondPlayer:
                    components.winnerText.text = "P2 Win!";
                    break;
                case Winner.EnemyLeave:
                    components.winnerText.text = "Enemy leave, you win!"; //Cals when one of players leave
                    break;
            }

            ChangeScreen(components.gameScreen, components.endScreen); //Change screen to end
        }

        //UI draw start timer
        public void DrawTimer(float value)
        {
            components.loadedScreenText.text = ((int)value).ToString();
        }

        //UI draw score
        public void DrawScore()
        {
            components.scoreText.text = GameManager.Instance.GetScore(); //Get score from GameManager
        }

    }

    //Components container
    [Serializable]
    public struct GameUIManagerComponents
    {
        [Header("Mobile")]
        public GameObject mobileUI;

        [Header("Screens")]
        public GameObject loadedScreen, pauseScreen, gameScreen, endScreen;

        [Header("Loaded Screen")]
        public Text loadedScreenText;

        [Header("Game Screen")]
        public Text scoreText;

        [Header("End Screen")]
        public Text winnerText;

    }
}