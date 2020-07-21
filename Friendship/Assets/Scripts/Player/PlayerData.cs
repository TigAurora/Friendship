using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.U2D.Animation;

namespace Friendship
{
    public class PlayerData : MonoBehaviour
    {

        [Header("Components")]
        public PlayerDataComponents components;

        [Header("Variables")]
        public int whichplayer;

        public string[] currentUI;

        SpriteResolver hairaResolver;
        SpriteResolver hairbResolver;
        SpriteResolver haircResolver;
        SpriteResolver catResolver;
        SpriteResolver sunglassesResolver;
        SpriteResolver mouthResolver;
        SpriteResolver eyeResolver;
        SpriteResolver eyebrowResolver;
        SpriteResolver wheelchairResolver;

        #region //Unity

        void Awake()
        {
            components.photonView = GetComponent<PhotonView>();
        }

        void Start()
        {
            currentUI = new string[] {"", "", "", "", "", "", "", "", ""};
            if (GetComponent<PhotonView>().IsMine)
            {
                whichplayer = PlayerPrefs.GetInt("myCharacter");
            }
            else
            {
                if (PlayerPrefs.GetInt("myCharacter") == 0)
                    whichplayer = 1;
                else if (PlayerPrefs.GetInt("myCharacter") == 1)
                    whichplayer = 0;
            }
            getResolver(whichplayer);

        }

        #endregion

        //Get resolver
        public void getResolver(int getwhich)
        {
            foreach (SpriteResolver resolver in Resources.FindObjectsOfTypeAll<SpriteResolver>())
            {
                if (resolver.transform.parent.parent.gameObject.GetComponent<PlayerData>().whichplayer == getwhich)
                {
                    if (resolver.GetCategory() == "haira")
                    {
                        hairaResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "hairb")
                    {
                        hairbResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "hairc")
                    {
                        haircResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "eye")
                    {
                        eyeResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "sunglasses")
                    {
                        sunglassesResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "eyebrow")
                    {
                        eyebrowResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "cat")
                    {
                        catResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "wheelchair")
                    {
                        wheelchairResolver = resolver;
                    }
                    else if (resolver.GetCategory() == "mouth")
                    {
                        mouthResolver = resolver;
                    }

                }
            }
        }

        //Confirm current UI and send it thru RPC
        public void Confirmcreation()
        {
            getCurrentUI();
            components.photonView.RPC("RPC_syncPlayerUI", RpcTarget.Others, currentUI);
        }

        //UpdateUI with given bodyparts
        public void UpdateUI(string[] bodyUI, int whichPlayer)
        {
            catResolver.SetCategoryAndLabel(getType(bodyUI[0]), bodyUI[0]);
            eyebrowResolver.SetCategoryAndLabel(getType(bodyUI[1]), bodyUI[1]);
            if (whichPlayer == 1)
            {
                eyeResolver.SetCategoryAndLabel(getType(bodyUI[2]), bodyUI[2]);
                //wheelchairResolver.SetCategoryAndLabel(getType(bodyUI[8]), bodyUI[8]);
            }
            else if (whichPlayer == 0)
            {
                sunglassesResolver.SetCategoryAndLabel(getType(bodyUI[3]), bodyUI[3]);
            }
            mouthResolver.SetCategoryAndLabel(getType(bodyUI[4]), bodyUI[4]);
            hairaResolver.SetCategoryAndLabel(getType(bodyUI[5]), bodyUI[5]);
            hairbResolver.SetCategoryAndLabel(getType(bodyUI[6]), bodyUI[6]);
            haircResolver.SetCategoryAndLabel(getType(bodyUI[7]), bodyUI[7]);
        }

        //GetCurrentUI
        public void getCurrentUI()
        {
            currentUI[0] = catResolver.GetLabel();
            currentUI[1] = eyebrowResolver.GetLabel();
            if (whichplayer == 1)
            {
                currentUI[2] = eyeResolver.GetLabel();
                //currentUI[8] = wheelchairResolver.GetLabel();
            }
            else if (whichplayer == 0)
            {
                currentUI[3] = sunglassesResolver.GetLabel();
            }
            currentUI[4] = mouthResolver.GetLabel();
            currentUI[5] = hairaResolver.GetLabel();
            currentUI[6] = hairbResolver.GetLabel();
            currentUI[7] = haircResolver.GetLabel();

        }

        //SupportFunctions
        //get body part type/tag
        private string getType(string name)
        {
            string trg = "";
            for (int i = 0; i < name.Length; ++i)
            {
                if (name[i] >= 'a' && name[i] <= 'z')
                {
                    trg += name[i];
                }
                else
                {
                    i = name.Length;
                }
            }

            return trg;
        }

        #region //RPC
        [PunRPC]
        void RPC_syncPlayerUI(string[] anotherplayerUI)
        {
            //Find players in scene

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            //Sort by playerID
            foreach (GameObject player in players)
            {
                if (!player.GetComponent<PhotonView>().IsMine)
                {
                    player.GetComponent<PlayerData>().getResolver(player.GetComponent<PlayerData>().whichplayer);
                    player.GetComponent<PlayerData>().UpdateUI(anotherplayerUI, player.GetComponent<PlayerData>().whichplayer);
                }
            }
        }

        #endregion

    }

    //Components container
    [Serializable]
    public struct PlayerDataComponents
    {
        //0 - cat
        //1 - eyebrow
        //2 - eye
        //3 - sunglasses
        //4 - mouth
        //5 - haira
        //6 - hairb
        //7 - hairc
        //8 - wheelchair
        public string[] bodayparts;
        [HideInInspector] public PhotonView photonView;
    }
}
