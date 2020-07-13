using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBones : MonoBehaviour
{
    private Animator myAnim;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject temp = null;
            Transform[] trans = GameObject.Find("blindplayer").GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trans)
            {
                if (t.gameObject.name == "Player P1")
                {
                    temp = t.gameObject;
                }
            }
            GameObject temp1 = GameObject.Find("blind1");
            temp.SetActive(true);
            temp1.SetActive(false);
        }
    }
}
