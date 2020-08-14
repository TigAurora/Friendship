using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class BlindView : MonoBehaviour
    {
        [Header("ColorTest")]
        public bool normal = true;
        public bool black = false;
        public bool opposite = false;
        public Material InvertColorMaterial;
        public Material DefaultMaterial;
        public GameObject level;
        //public GameObject sprites;

        // Start is called before the first frame update
        void Start()
        {
            level = GameObject.Find("Level");
        }

        // Update is called once per frame
        void Update()
        {
            //ChangeColor();
        }

        public void ChangeColor()
        {
            if (normal == true)
            {
                //TurnNormal();
            }
            if (black == true)
            {
                //TurnBlack();
            }
            if (opposite == true)
            {
                //TurnOpposite();
            }
        }

        public void TurnNormal(string Obj)
        {
            foreach (GameObject o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                if (o.GetComponent<SpriteRenderer>() != null)
                {
                    var r = o.GetComponent<SpriteRenderer>();
                    if (r.transform.tag == Obj)
                    {
                        r.material = DefaultMaterial;
                        r.color = new Color(1f, 1f, 1f, 1f);
                    }
                }
            }
            black = false;
            opposite = false;
        }

        public void TurnBlack(string Obj)
        {
            foreach (GameObject o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                if (o.GetComponent<SpriteRenderer>() != null)
                {
                    var r = o.GetComponent<SpriteRenderer>();
                    if (r.transform.tag == "Body" && (r.transform.parent.name == "deaf" || r.transform.parent.name == "wheelchair"))
                    {
                        if (Obj == "Player")
                        {
                            r.material = DefaultMaterial;
                            r.color = new Color(0f, 0f, 0f, 0f);
                        }
                    }
                    else if (r.transform.tag == Obj)
                    {
                        r.material = DefaultMaterial;
                        if (r.transform.name == "g1liftgan")
                        {
                            r.color = new Color(0f, 0f, 0f, 0f);
                        }
                        else
                        {
                            r.color = new Color(0f, 0f, 0f, 1f);
                        }

                    }
                }

            }
            normal = false;
            opposite = false;
        }

        public void TurnOpposite(string Obj)
        {
            foreach (GameObject o in Resources.FindObjectsOfTypeAll(typeof(GameObject)))
            {
                    if (o.GetComponent<SpriteRenderer>() != null)
                    {
                        var r = o.GetComponent<SpriteRenderer>();
                        if (r.transform.tag == Obj)
                        {
                            r.material = InvertColorMaterial;
                            r.material.SetFloat("_Threshold", 1);
                            r.material.color = new Color(r.material.color.r, r.material.color.g, r.material.color.b, 0f);
                            r.color = new Color(r.color.r, r.color.g, r.color.b, 0f);
                        }
                    }

            }
            normal = false;
            black = false;
        }
    }
}