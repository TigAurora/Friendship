using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class GA_Buttons : MonoBehaviour
    {
        [Header("GA_LiftCall")]
        public GameObject GA_Horn;

        [Header("PressedSpirit")]
        public Sprite pressedS;

        //0 = frog, 1 = chick
        public int whichButton;
        public bool pressedornot = false;
        public List<Collider2D> whoPressed;
        Animator anim;
        public bool Completed = false;

        void Start()
        {
            whoPressed = new List<Collider2D>();
            anim = GetComponent<Animator>();
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!Completed && other.gameObject.tag == "Player")
            {
                if (!whoPressed.Contains(other))
                {
                    whoPressed.Add(other);
                }
                if (!pressedornot)
                {
                    anim.SetTrigger("Play");
                    GA_Horn.GetComponent<GA_LiftCall>().buttonPressed(whichButton);
                    pressedornot = true;
                }
            }

        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!Completed && other.gameObject.tag == "Player")
            {
                if (whoPressed.Contains(other))
                {
                    whoPressed.Remove(other);
                }
                if (whoPressed.Count == 0 && pressedornot)
                {
                    pressedornot = false;
                    anim.SetTrigger("End");
                }
            }
        }

        public void Alldone()
        {
            Completed = true;
            GetComponent<Animator>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = pressedS;
        }
    }
}