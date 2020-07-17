using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.U2D.Animation;

public class CharactorCreator : MonoBehaviour
{

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

    // Start is called before the first frame update
    void Start()
    {
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
                //components.Blind.SetActive(true);
                //eye.SetActive(false);
                //wheelchair.SetActive(false);
            }
            else
            {
                //components.Deaf.SetActive(true);
                //sunglasses.SetActive(false);
            }
        }

        defaultbody_pressed();
    }

    // Update is called once per frame
    void Update()
    {
    }

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


            tpname = randBody("cat", 16);
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
            //eyeResolver.SetCategoryAndLabel("eye", "eye" + UnityEngine.Random.Range(0, 0));
            //wheelchairResolver.SetCategoryAndLabel("wheelchair", "wheelchair" + UnityEngine.Random.Range(0, 0));
        }
        tpname = randBody("eyebrow", 17);
        eyebrowResolver.SetCategoryAndLabel("eyebrow", tpname);
        unselectbody("eyebrow");
        selectbody(tpname);
    }

    //body parts onClick
    public void onClick_mouth(string type)
    {
        mouthResolver.SetCategoryAndLabel("mouth", type);
    }
    public void onClick_haira(string type)
    {
        //Debug.Log("outside: " + "hairb.getlabel() = " + hairbResolver.GetLabel() + ", type = " + type);
        if (hairbResolver.GetLabel() == "hairb1" && type != "haira10")
        {
            hairbResolver.SetCategoryAndLabel("hairb", "hairb2");
            unselectbody("hairb");
            selectbody("hairb2");
            //Debug.Log("inside");
        }
        hairaResolver.SetCategoryAndLabel("haira", type);
    }
    public void onClick_hairb(string type)
    {
        if (type == "hairb1")
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

        hairbResolver.SetCategoryAndLabel("hairb", type);
    }
    public void onClick_hairc(string type)
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
        haircResolver.SetCategoryAndLabel("hairc", type);
    }
    public void onClick_eye(string type)
    {
        eyeResolver.SetCategoryAndLabel("eye", type);
    }
    public void onClick_eyebrow(string type)
    {
        eyebrowResolver.SetCategoryAndLabel("eyebrow", type);
    }
    public void onClick_cat(string type)
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
        catResolver.SetCategoryAndLabel("cat", type);
    }
    public void onClick_sunglasses(string type)
    {
        sunglassesResolver.SetCategoryAndLabel("sunglasses", type);
    }
    public void onClick_wheelchair(string type)
    {
        wheelchairResolver.SetCategoryAndLabel("wheelchair", type);
    }

    //Initialize default body part squares with pressed sprite and animation
    private void defaultbody_pressed()
    {
        string tpname;

        if (PlayerPrefs.GetInt("myCharacter") == 1)
        {
            tpname = "eye" + deye;
            unselectbody("eye");
            selectbody(tpname);
        }

        tpname = "eyebrow" + deyebrow;
        unselectbody("eyebrow");
        selectbody(tpname);

        tpname = "haira" + dhaira;
        unselectbody("haira");
        selectbody(tpname);

        tpname = "hairb" + dhairb;
        unselectbody("hairb");
        selectbody(tpname);

        tpname = "hairc" + dhairc;
        unselectbody("hairc");
        selectbody(tpname);

        tpname = "cat" + dcat;
        unselectbody("cat");
        selectbody(tpname);

        tpname = "mouth" + dmouth;
        unselectbody("mouth");
        selectbody(tpname);

        if (PlayerPrefs.GetInt("myCharacter") == 0)
        {
            tpname = "sunglasses" + dsunglasses;
            unselectbody("eye");
            selectbody(tpname);
        }

        if (PlayerPrefs.GetInt("myCharacter") == 1)
        {
            tpname = "wheelchair" + dwheelchair;
            unselectbody("eye");
            selectbody(tpname);
        }

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
            if (name[i] >= '0' && name[i] <= '9')
            {
                i = name.Length;
            }
            else
            {
                trg += name[i];
            }
        }

        return trg;
    }

    //Find object in p1 or p2, name sample: haira24
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
}
