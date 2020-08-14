using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class LiftControll : MonoBehaviour
    {
        [Header("Colliders")]
        public GameObject LeftDoor;
        public GameObject RightDoor;
        public GameObject g1Collider;
        public GameObject LiftCollider;

        [Header("Lights")]
        public GameObject UpLight;
        public GameObject DownLight;

        [Header("AudioControl")]
        public GameObject Floor;
        public GameObject Bell;

        public int PlayersinLift = 0;
        public GameObject[] players;
        private bool isContainB = false;
        private bool isContainD = false;


        // Start is called before the first frame update
        void Start()
        {
            //Find players in scene
            players = GameObject.FindGameObjectsWithTag("Player");

            //Sort by playerID
            foreach (GameObject player in players)
            {
                if (player.name.Contains("blindplayer"))
                    players[0] = player.gameObject;
                else if (player.name.Contains("deafplayer"))
                    players[1] = player.gameObject;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<AudioSource>().volume == 0)
            {
                GetComponent<AudioSource>().Stop();
            }
            DetectPlayerPos();
        }

        public void DeleteCollider(string which)
        {
            if (which == "g1")
            {
                g1Collider.SetActive(false);
            }
        }

        public void DetectPlayerPos()
        {
            if (LiftCollider.GetComponent<Collider2D>().bounds.Contains(players[0].transform.position))
            {
                isContainB = true;
            }
            else
            {
                isContainB = false;
            }
            if (LiftCollider.GetComponent<Collider2D>().bounds.Contains(players[1].transform.position))
            {
                isContainD = true;
            }
            else
            {
                isContainD = false;
            }

            if (isContainB && isContainD)
            {
                PlayersinLift = 2;
            }
            else if (!isContainB && !isContainD)
            {
                PlayersinLift = 0;
            }
            else if (isContainB || isContainD)
            {
                PlayersinLift = 1;
            }
        }


        //Control Lights
        //0 = CLOSE, 1 = OPEN
        public void ControllUp(int status)
        {
            if (status == 0)
            {
                UpLight.SetActive(false);
            }
            else if (status == 1)
            {
                UpLight.SetActive(true);
            }
        }

        public void ControllDown(int status)
        {
            if (status == 0)
            {
                DownLight.SetActive(false);
            }
            else if (status == 1)
            {
                DownLight.SetActive(true);
            }
        }

        //Anim Sound Control
        public void LiftStart()
        {
            GetComponent<AudioSource>().enabled = true;
            LevelsAudioManager.Instance.SetClip(gameObject, "Start");

        }

        public void LiftArrive()
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0, 2f);
            LevelsAudioManager.Instance.SetClip(Floor, "LiftFloorArrive");
            LevelsAudioManager.Instance.SetClip(Bell, "LiftBellArrive");
        }

        public void LiftBell()
        { }

        public void LiftOpenDoor(string OpenWhichDoor)
        {
            StartCoroutine(OpenDoor(OpenWhichDoor));
        }

        public void LiftCloseDoor(string CloseWhichDoor)
        {
            StartCoroutine(CloseDoor(CloseWhichDoor));
        }

        public void LiftCloseDoorSoon(string CloseWhichDoor)
        {
            StartCoroutine(CloseDoorSoon(CloseWhichDoor));
        }

        IEnumerator OpenDoor(string which)
        {
            yield return new WaitForSeconds(2f);
            if (which == "left")
            {
                LevelsAudioManager.Instance.SetClip(LeftDoor, "LiftDoorOpen");
                yield return new WaitForSeconds(2f);
                LeftDoor.GetComponent<Collider2D>().isTrigger = true;
            }
            else if (which == "right")
            {
                LevelsAudioManager.Instance.SetClip(RightDoor, "LiftDoorOpen");
                yield return new WaitForSeconds(2f);
                RightDoor.GetComponent<Collider2D>().isTrigger = true;
            }
        }


        IEnumerator CloseDoorSoon(string which)
        {
            if (which == "left")
            {
                LeftDoor.GetComponent<Collider2D>().isTrigger = false;
                LevelsAudioManager.Instance.SetClip(LeftDoor, "LiftDoorClose");

            }
            else if (which == "right")
            {
                RightDoor.GetComponent<Collider2D>().isTrigger = false;
                LevelsAudioManager.Instance.SetClip(RightDoor, "LiftDoorClose");
            }
            yield return new WaitForSeconds(2f);

        }

        IEnumerator CloseDoor(string which)
        {
            yield return new WaitForSeconds(2f);
            if (which == "left")
            {
                LeftDoor.GetComponent<Collider2D>().isTrigger = false;
                LevelsAudioManager.Instance.SetClip(LeftDoor, "LiftDoorClose");
                
            }
            else if (which == "right")
            {
                RightDoor.GetComponent<Collider2D>().isTrigger = false;
                LevelsAudioManager.Instance.SetClip(RightDoor, "LiftDoorClose");
                
            }
        }

    }
}
