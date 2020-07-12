using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;
using Photon.Pun;
using Photon.Realtime;

public class PartSelector : MonoBehaviour
{

    [Header("Pages")]
    public GameObject[] pages;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick_parts(string name)
    {
        foreach (GameObject page in pages)
        {
            if (page.tag != name)
            {
                page.SetActive(false);
            }
            else if (page.name != name + "p1")
            {
                page.SetActive(false);
            }
            else
            {
                page.SetActive(true);
            }

        }
    }

}
