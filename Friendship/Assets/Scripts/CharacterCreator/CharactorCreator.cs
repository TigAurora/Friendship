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

    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();

    SpriteResolver hairaResolver;

    // Start is called before the first frame update
    void Start()
    {
        //if(PlayerPrefs.GetInt("myCharacter") == 0)
            //components.Blind.SetActive(true);
        //else
            //components.Deaf.SetActive(true);

        foreach (var resolver in FindObjectsOfType<SpriteResolver>())
        {
            spriteResolvers.Add(resolver);
            if (resolver.GetCategory() == "haira")
            {
                hairaResolver = resolver;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void onClick_haira(string type)
    {
        hairaResolver.SetCategoryAndLabel("haira", type);
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
