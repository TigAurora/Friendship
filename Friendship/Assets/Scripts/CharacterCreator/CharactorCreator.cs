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
    public class CharactorCreator : MonoBehaviour
    {

        public static CharactorCreator Instance;
        [HideInInspector] public PhotonView photonView;
        [HideInInspector] public bool Blindloaded = false;
        [HideInInspector] public bool Deafloaded = false;
        [HideInInspector] public bool Allloaded = false;

        [Header("CharacterFeature")]
        public GameObject eye;
        public GameObject sunglasses;
        public GameObject wheelchair;

        [Header("Images")]
        public Sprite normal;
        public Sprite pressed;

        [Header("RuntimeAnimatorController")]
        public RuntimeAnimatorController x1;
        public RuntimeAnimatorController x2;

        [Header("Default Body Parts")]
        public int dhaira;
        public int dhairb;
        public int dhairc;
        public int dcat;
        public int dmouth;
        public int deye;
        public int dwheelchair;
        public int dsunglasses;
        public int deyebrow;

        [Header("Player Ground")]
        public GameObject deafground;
        public GameObject blindground;

        private CharactorCreatorNetworkManager networkmanager;
        public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();

        SpriteResolver hairaResolver;
        SpriteResolver hairbResolver;
        SpriteResolver haircResolver;
        SpriteResolver catResolver;
        SpriteResolver sunglassesResolver;
        SpriteResolver mouthResolver;
        SpriteResolver eyeResolver;
        SpriteResolver eyebrowResolver;
        SpriteResolver wheelchairResolver;


        #region //Initialization
        void Awake()
        {
            photonView = GetComponent<PhotonView>();
            SingletonInit();            
        }

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }

        public void StartGame()
        {
            networkmanager = GetComponent<CharactorCreatorNetworkManager>();
            networkmanager.OnGame_Started();

            //setActivePlayer();
            foreach (var resolver in FindObjectsOfType<SpriteResolver>())
            {
                spriteResolvers.Add(resolver);
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

                if (PlayerPrefs.GetInt("myCharacter") == 0)
                {
                    //blind.SetActive(true);
                    eye.SetActive(false);
                    wheelchair.SetActive(false);
                }
                else
                {
                    //deaf.SetActive(true);
                    sunglasses.SetActive(false);
                }
            }

            defaultbody_pressed_selected();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {
                deafground.SetActive(false); 
            }
            else
            {
                blindground.SetActive(false);
            }
        }

        void Update()
        {
            if (!(Blindloaded && Deafloaded))
            {
                GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
                if (players.Length > 0)
                {
                    foreach (GameObject player in players)
                    {
                        if (player.GetComponent<PhotonView>().IsMine)
                        {
                            if (PlayerPrefs.GetInt("myCharacter") == 0)
                            {
                                Instance.photonView.RPC("RPC_Blindloaded", RpcTarget.All);
                            }
                            else if (PlayerPrefs.GetInt("myCharacter") == 1)
                            {
                                Instance.photonView.RPC("RPC_Deafloaded", RpcTarget.All);
                            }
                        }
                    }
                }
            }
        }

        //Initialize default body part squares with pressed sprite and animation
        private void defaultbody_pressed_selected()
        {
            string tpname;

            tpname = "eyebrow" + deyebrow;
            unselectbody("eyebrow");
            selectbody(tpname);
            eyebrowResolver.SetCategoryAndLabel("eyebrow", tpname);

            tpname = "haira" + dhaira;
            unselectbody("haira");
            selectbody(tpname);
            hairaResolver.SetCategoryAndLabel("haira", tpname);

            tpname = "hairb" + dhairb;
            unselectbody("hairb");
            selectbody(tpname);
            hairbResolver.SetCategoryAndLabel("hairb", tpname);

            tpname = "hairc" + dhairc;
            unselectbody("hairc");
            selectbody(tpname);
            haircResolver.SetCategoryAndLabel("hairc", tpname);

            tpname = "cat" + dcat;
            unselectbody("cat");
            selectbody(tpname);
            catResolver.SetCategoryAndLabel("cat", tpname);

            tpname = "mouth" + dmouth;
            unselectbody("mouth");
            selectbody(tpname);
            mouthResolver.SetCategoryAndLabel("mouth", tpname);

            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {
                tpname = "sunglasses" + dsunglasses;
                unselectbody("sunglasses");
                selectbody(tpname);
                sunglassesResolver.SetCategoryAndLabel("sunglasses", tpname);
            }

            if (PlayerPrefs.GetInt("myCharacter") == 1)
            {
                tpname = "eye" + deye;
                unselectbody("eye");
                selectbody(tpname);
                eyeResolver.SetCategoryAndLabel("eye", tpname);

                //tpname = "wheelchair" + dwheelchair;
                //unselectbody("wheelchair");
                //selectbody(tpname);
                //wheelchairResolver.SetCategoryAndLabel("wheelchair", tpname);
            }

        }
        #endregion


        #region //Buttonfunctions
        //Randomize character
        public void onClick_randomize()
        {
            string tpname;

            tpname = randBody("mouth", 35);
            mouthResolver.SetCategoryAndLabel("mouth", tpname);
            unselectbody("mouth");
            selectbody(tpname);

            tpname = randBody("hairb", 33);
            hairbResolver.SetCategoryAndLabel("hairb", tpname);
            unselectbody("hairb");
            selectbody(tpname);
        

            if (hairbResolver.GetLabel() == "hairb1")
            {
                int which = UnityEngine.Random.Range(0, 2);
                //which haira with hairb1
                if (which == 0)
                {
                    hairaResolver.SetCategoryAndLabel("haira", "haira20");
                    unselectbody("haira");
                    selectbody("haira20");
                }
                else
                {
                    hairaResolver.SetCategoryAndLabel("haira", "haira10");
                    unselectbody("haira");
                    selectbody("haira10");
                }

                haircResolver.SetCategoryAndLabel("hairc", "hairc1");
                unselectbody("hairc");
                selectbody("hairc1");

                catResolver.SetCategoryAndLabel("cat", "cat1");
                unselectbody("cat");
                selectbody("cat1");
            }
            else
            {
                tpname = randBody("haira", 20);
                hairaResolver.SetCategoryAndLabel("haira", tpname);
                unselectbody("haira");
                selectbody(tpname);

                tpname = randBody("hairc", 16);
                haircResolver.SetCategoryAndLabel("hairc", tpname);
                unselectbody("hairc");
                selectbody(tpname);


                tpname = randBody("cat", 30);
                catResolver.SetCategoryAndLabel("cat", tpname);
                unselectbody("cat");
                selectbody(tpname);
            }
            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {
                tpname = randBody("sunglasses", 20);
                sunglassesResolver.SetCategoryAndLabel("sunglasses", tpname);
                unselectbody("sunglasses");
                selectbody(tpname);
            }
            else
            {
                tpname = randBody("eye", 20);
                eyeResolver.SetCategoryAndLabel("eye", tpname);
                unselectbody("eye");
                selectbody(tpname);

                //tpname = randBody("wheelchair", 20);
                //wheelchairResolver.SetCategoryAndLabel("wheelchair", tpname);
                //unselectbody("wheelchair");
                //selectbody(tpname);
            }
            tpname = randBody("eyebrow", 17);
            eyebrowResolver.SetCategoryAndLabel("eyebrow", tpname);
            unselectbody("eyebrow");
            selectbody(tpname);
        }

        //body parts onClick
        public void onClick_mouth()
        {
            mouthResolver.SetCategoryAndLabel("mouth", getNamefromBtn());
        }
        public void onClick_haira()
        {
            //Debug.Log("outside: " + "hairb.getlabel() = " + hairbResolver.GetLabel() + ", type = " + type);
            if (hairbResolver.GetLabel() == "hairb1" && getNamefromBtn() != "haira10")
            {
                hairbResolver.SetCategoryAndLabel("hairb", "hairb2");
                unselectbody("hairb");
                selectbody("hairb2");
                //Debug.Log("inside");
            }
            hairaResolver.SetCategoryAndLabel("haira", getNamefromBtn());
        }
        public void onClick_hairb()
        {
            if (getNamefromBtn() == "hairb1")
            {
                hairaResolver.SetCategoryAndLabel("haira", "haira20");
                unselectbody("haira");
                selectbody("haira20");

                haircResolver.SetCategoryAndLabel("hairc", "hairc1");
                unselectbody("hairc");
                selectbody("hairc1");

                catResolver.SetCategoryAndLabel("cat", "cat1");
                unselectbody("cat");
                selectbody("cat1");
            }
            else if(hairbResolver.GetLabel() == "hairb1")
            {
                hairaResolver.SetCategoryAndLabel("haira", "haira1");
                unselectbody("haira");
                selectbody("haira1");
            }

            hairbResolver.SetCategoryAndLabel("hairb", getNamefromBtn());
        }
        public void onClick_hairc()
        {
            if (hairbResolver.GetLabel() == "hairb1")
            {
                hairbResolver.SetCategoryAndLabel("hairb", "hairb2");
                unselectbody("hairb");
                selectbody("hairb2");

                hairaResolver.SetCategoryAndLabel("haira", "haira1");
                unselectbody("haira");
                selectbody("haira1");
            }
            haircResolver.SetCategoryAndLabel("hairc", getNamefromBtn());
        }
        public void onClick_eye()
        {
            eyeResolver.SetCategoryAndLabel("eye", getNamefromBtn());
        }
        public void onClick_eyebrow()
        {
            eyebrowResolver.SetCategoryAndLabel("eyebrow", getNamefromBtn());
        }
        public void onClick_cat()
        {
            if (hairbResolver.GetLabel() == "hairb1")
            {
                hairbResolver.SetCategoryAndLabel("hairb", "hairb2");
                unselectbody("hairb");
                selectbody("hairb2");

                hairaResolver.SetCategoryAndLabel("haira", "haira1");
                unselectbody("haira");
                selectbody("haira1");
            }
            catResolver.SetCategoryAndLabel("cat", getNamefromBtn());
        }
        public void onClick_sunglasses()
        {
            sunglassesResolver.SetCategoryAndLabel("sunglasses", getNamefromBtn());
        }
        public void onClick_wheelchair()
        {
            wheelchairResolver.SetCategoryAndLabel("wheelchair", getNamefromBtn());
        }
        #endregion

        #region //Supportfunctions
        //Get current selected parts name from button
        private string getNamefromBtn()
        {
            var buttonSelf = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
            int num = getNum(buttonSelf.name);
            string type = getType(buttonSelf.name);
            int whichPage = getNum(buttonSelf.transform.parent.gameObject.name);

            int finalnum = num + 12 * (whichPage - 1);

            //Debug.Log("BtnNum: " + num + ", whichPage: " + whichPage + "result: " + type + finalnum);

            return type + finalnum;
        }

        //revert (sample)haira1 to (sample haira(1)
        private string revert(string name)
        {
            string type = getType(name);
            int num = getNum(name);
            if (num % 12 == 0)
            {
                num = 12;
            }
            else
            {
                num = num % 12;
            }
            //Debug.Log(type + "(" + num + ")");
            return type + " (" + num + ")";
        }

        //Randomize bodypart with numbers(not include)
        private string randBody(string name, int limit)
        {
            //Debug.Log("randBody name:" + name + UnityEngine.Random.Range(1, limit));
            return name + UnityEngine.Random.Range(1, limit);
        }

        //get body part type/tag
        private string getType(string name)
        {
            //Debug.Log("getType name:" + name);
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

        //Find object in p1, p2, p3, etc. name sample: haira24
        private string whichPage(string name)
        {
            string type = getType(name);
            int num = getNum(name);
            if (num % 12 == 0)
            {
                num = num / 12;
            }
            else
            {
                num = num / 12 + 1;
            }
            //Debug.Log("ori: name, " + name + "after: " + type + 'p' + num);
            return type + 'p' + num;
        }

        //Get integer in a string
        private int getNum(string name)
        {
            string num = "";
            for (int i = 0; i < name.Length; ++i)
            {
                if (name[i] >= '0' && name[i] <= '9')
                {
                    num += name[i];
                }
            }

            return int.Parse(num);
        }

        //find gameobjects(body parts) and unselect them all
        private void unselectbody(string name)
        {
            GameObject cv = GameObject.Find("Canvas");
            string tag = getType(name);
            foreach (Transform obj in cv.GetComponent<Transform>())
            {
                if (obj.gameObject.tag == tag)
                {
                    foreach (Transform item in obj.GetComponent<Transform>())
                    {
                        item.gameObject.GetComponent<Image>().sprite = normal;
                        item.gameObject.GetComponent<Animator>().runtimeAnimatorController = x1;
                    }
                }
            }
        }

        //find gameobject(body parts) and select it
        private void selectbody(string name)
        {
            GameObject cv = GameObject.Find("Canvas");
            string revertname = revert(name);
            string pagename = whichPage(name);
            foreach (Transform obj in cv.GetComponent<Transform>())
            {
                if (obj.gameObject.name == pagename)
                {
                    foreach (Transform item in obj.GetComponent<Transform>())
                    {
                        if (item.gameObject.name == revertname)
                        {
                            item.gameObject.GetComponent<Image>().sprite = pressed;
                            item.gameObject.GetComponent<Animator>().runtimeAnimatorController = x2;
                        }
                    }
                }
            }
        }
        #endregion

        //RPC
        [PunRPC]
        void RPC_Blindloaded()
        {
            Blindloaded = true;
            if (Blindloaded && Deafloaded)
            {
                if (PlayerNetwork.Instance.photonView.IsMine)
                {
                    PlayerNetwork.Instance.photonView.RPC("RPC_FinishLoading", RpcTarget.All);
                    Allloaded = true;
                }
            }
        }

        [PunRPC]
        void RPC_Deafloaded()
        {
            Deafloaded = true;
            if (Blindloaded && Deafloaded)
            {
                if (PlayerNetwork.Instance.photonView.IsMine)
                {
                    PlayerNetwork.Instance.photonView.RPC("RPC_FinishLoading", RpcTarget.All);
                    Allloaded = true;
                }
            }
        }
    }
}