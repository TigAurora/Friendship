using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;

namespace TheBattle
{
    public class PlayerData : MonoBehaviour
    {

        [Header("Components")]
        public PlayerDataComponents components;

        [Header("Variables")]
        public int playerID;

        [Range(0, 100)] public float HP;

        [Header("Damage")]
        public float hitDamageMin;
        public float hitDamageMax;

        #region //Unity

        void Start()
        {
            UpdateUI();
        }

        #endregion

        //Get damage method
        public void GetDamage(float damage)
        {
            HP -= damage;

            UpdateUI();

            if (HP <= 0)
            {
                Death();
            }
        }

        //Get random damage power method
        public float GetDamagePower()
        {
            //get random value use min and max variables
            float result = UnityEngine.Random.Range(hitDamageMin, hitDamageMax);
            return result;
        }

        //Draw UI method
        void UpdateUI()
        {
            //Draw hp
            components.hpBar.fillAmount = HP / 100;
        }

        //Death method
        void Death()
        {
            //Send to manager death info
            GameManager.Instance.OnRound_End(playerID);

            //last round check
            if (!GameManager.Instance.IsRoundLast())
            {
                //if not the last round 
                HP = 100; //reset hp
                UpdateUI();

                //Reset positions
                GameManager.Instance.components.networkManager.ResetPlayersPositions();
            }
        }
    }

    //Components container
    [Serializable]
    public struct PlayerDataComponents
    {
        public Image hpBar;
    }
}
