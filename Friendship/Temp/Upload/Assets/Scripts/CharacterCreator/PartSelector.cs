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

    [Header("MaxPages")]
    public int maxeye;
    public int maxeyebrow;
    public int maxsunglasses;
    public int maxhaira;
    public int maxhairb;
    public int maxhairc;
    public int maxmouth;
    public int maxcat;
    public int maxwheelchair;

    [Header("Pagebutton")]
    public GameObject previousbutton;
    public GameObject nextbutton;

    [Header("Pagenumber")]
    public GameObject currentnumber;
    public GameObject totalnumber;
    public Sprite[] pagenumber;

    private string currentpart;
    private int currentpage;
    private Dictionary<string, int> max = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        max.Add("maxeye", maxeye);
        max.Add("maxeyebrow", maxeyebrow);
        max.Add("maxsunglasses", maxsunglasses);
        max.Add("maxhaira", maxhaira);
        max.Add("maxhairb", maxhairb);
        max.Add("maxhairc", maxhairc);
        max.Add("maxmouth", maxmouth);
        max.Add("maxcat", maxcat);
        max.Add("maxwheelchair", maxwheelchair);

        if (PlayerPrefs.GetInt("myCharacter") == 0)
        {
            currentpart = "sunglasses";
        }
        else
        {
            currentpart = "eye";
        }
        currentpage = 1;

        foreach (GameObject page in pages)
        {
            if (page.name == currentpart + "p" + currentpage)
            {
                page.SetActive(true);
                previousbutton.SetActive(false);
            }
        }

        totalnumber.GetComponent<Image>().sprite = pagenumber[max["max" + currentpart] - 1];

    }

    // Update is called once per frame
    void Update()
    {
        //Same as clicking next and previous button
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(currentpage < max["max" + currentpart])
                onClick_nextpage();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (currentpage > 1)
                onClick_previouspage();
        }

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
                currentpart = name;
                currentpage = 1;
                previousbutton.SetActive(false);
                nextbutton.SetActive(true);
                page.SetActive(true);
                currentnumber.GetComponent<Image>().sprite = pagenumber[0];
                totalnumber.GetComponent<Image>().sprite = pagenumber[max["max" + currentpart] - 1];
            }

        }
    }

    public void onClick_previouspage()
    {
        currentpage -= 1;

        if (currentpage == 1)
        {
            previousbutton.SetActive(false);
        }

        if (currentpage < max["max" + currentpart])
        {
            nextbutton.SetActive(true);
        }

        setActivepage(currentpart + "p" + currentpage);
        currentnumber.GetComponent<Image>().sprite = pagenumber[currentpage - 1];
    }

    public void onClick_nextpage()
    {
        currentpage += 1;

        if (currentpage == max["max" + currentpart])
        {
            nextbutton.SetActive(false);
        }

        if(currentpage > 1)
        {
            previousbutton.SetActive(true); 
        }

        setActivepage(currentpart + "p" + currentpage);
        currentnumber.GetComponent<Image>().sprite = pagenumber[currentpage - 1];
    }

    public void setActivepage(string name)
    {
        foreach (GameObject page in pages)
        {
            if (page.name != name)
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
