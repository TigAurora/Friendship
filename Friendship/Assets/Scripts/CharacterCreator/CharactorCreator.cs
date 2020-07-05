using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CharactorCreator : MonoBehaviour
{

    [Header("Components")]
    public CharactorCreatorComponents components;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("myCharacter") == 0)
            components.BlindInterface.SetActive(true);
        else
            components.DeafInterface.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Components container
    [Serializable]
    public struct CharactorCreatorComponents
    {
        [HideInInspector] public CharactorCreator menuNetwork;


        [Header("UI")]
        public GameObject BlindInterface;
        public GameObject DeafInterface;

    }
}
