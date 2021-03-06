﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO;

public class ButtonClick : MonoBehaviour
{
    [Header("Images")]
    public Sprite normal;
    public Sprite pressed;

    [Header("RuntimeAnimatorController")]
    public RuntimeAnimatorController x1;
    public RuntimeAnimatorController x2;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void onClick_select()
    {
        GameObject parent = this.transform.parent.gameObject;
        string tag = parent.tag;
        GameObject cv = GameObject.Find("Canvas");
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
        this.GetComponent<Image>().sprite = pressed;
        this.GetComponent<Animator>().runtimeAnimatorController = x2;
    }

    public void onClick_confirm()
    {

    }
}