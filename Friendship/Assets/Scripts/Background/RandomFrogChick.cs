using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFrogChick : MonoBehaviour
{
    float time, rand;
    // Start is called before the first frame update
    void Start()
    {
        rand = Random.Range(1f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= rand)
        {
            GetComponent<Animator>().SetTrigger("Play");
            time = 0;
            rand = Random.Range(1f, 15f);
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    public void AnimEnd()
    {
        GetComponent<Animator>().SetTrigger("End");
    }
}
