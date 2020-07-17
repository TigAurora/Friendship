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

    [Header("Components")]
    public CharactorCreatorComponents components;

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
    }

    // Update is called once per frame
    void Update()
    {
    }

    //Randomize character
    public void onClick_randomize()
    {
        mouthResolver.SetCategoryAndLabel("mouth", "mouth" + UnityEngine.Random.Range(1, 35));
        hairbResolver.SetCategoryAndLabel("hairb", "hairb" + UnityEngine.Random.Range(1, 33));
        if (hairbResolver.GetLabel() == "hairb1")
        {
            hairaResolver.SetCategoryAndLabel("haira", "haira20");
            haircResolver.SetCategoryAndLabel("hairc", "hairc1");
            catResolver.SetCategoryAndLabel("cat", "cat1");
        }
        else
        {
            hairaResolver.SetCategoryAndLabel("haira", "haira" + UnityEngine.Random.Range(1, 20));
            haircResolver.SetCategoryAndLabel("hairc", "hairc" + UnityEngine.Random.Range(1, 16));
            catResolver.SetCategoryAndLabel("cat", "cat" + UnityEngine.Random.Range(1, 16));
        }
        if (PlayerPrefs.GetInt("myCharacter") == 0)
        {
            sunglassesResolver.SetCategoryAndLabel("sunglasses", "sunglasses" + UnityEngine.Random.Range(1, 20));
        }
        else
        {
            //eyeResolver.SetCategoryAndLabel("eye", "eye" + UnityEngine.Random.Range(0, 0));
            //wheelchairResolver.SetCategoryAndLabel("wheelchair", "wheelchair" + UnityEngine.Random.Range(0, 0));
        }
        eyebrowResolver.SetCategoryAndLabel("eyebrow", "eyebrow" + UnityEngine.Random.Range(1, 17));
    }

    public void onClick_mouth(string type)
    {
        mouthResolver.SetCategoryAndLabel("mouth", type);
    }
    public void onClick_haira(string type)
    {
        if (hairbResolver.GetLabel() == "hairb1")
        {
            hairbResolver.SetCategoryAndLabel("hairb", "hairb2");
        }
        hairaResolver.SetCategoryAndLabel("haira", type);
    }
    public void onClick_hairb(string type)
    {
        if (type == "hairb1")
        {
            hairaResolver.SetCategoryAndLabel("haira", "haira20");
            haircResolver.SetCategoryAndLabel("hairc", "hairc1");
            catResolver.SetCategoryAndLabel("cat", "cat1");
        }
        hairbResolver.SetCategoryAndLabel("hairb", type);
    }
    public void onClick_hairc(string type)
    {
        if (hairbResolver.GetLabel() == "hairb1")
        {
            hairbResolver.SetCategoryAndLabel("hairb", "hairb2");
            hairaResolver.SetCategoryAndLabel("haira", "haira1");
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
            hairaResolver.SetCategoryAndLabel("haira", "haira1");
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


    //Components container
    [Serializable]
    public struct CharactorCreatorComponents
    {
        [HideInInspector] public CharactorCreator menuNetwork;


        [Header("Player")]
        public GameObject Blind;
        public GameObject Deaf;

    }
}
