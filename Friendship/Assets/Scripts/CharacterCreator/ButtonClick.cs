using System.Collections;
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
        foreach (Transform item in parent.GetComponent<Transform>())
            item.gameObject.GetComponent<Image>().sprite = normal;
        this.GetComponent<Image>().sprite = pressed;
    }
}
