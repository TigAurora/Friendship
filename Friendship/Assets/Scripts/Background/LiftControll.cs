using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Friendship
{
    public class LiftControll : MonoBehaviour
    {
        [Header("Camera")]
        public Camera camera;

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

        [Header("Lines")]
        public GameObject Line;

        public int PlayersinLift = 0;
        public bool PlayersOnFloor = false;
        public GameObject[] players;
        private string status = "stop";
        public int currentFloor = -1;
        float timeWaiting = 0, totalTime = 3f;

        PhotonView photonView;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            //Find players in scene
            players = new GameObject[2];
            GameObject[] findplayer = GameObject.FindGameObjectsWithTag("Player");

            //Sort by playerID
            foreach (GameObject player in findplayer)
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
            //Sync the timing
            if (PlayerNetwork.Instance.myCharacter == 0)
            {
                if (PlayersinLift == 2 && status == "stop" && currentFloor == 0)
                {
                    NextAnim();
                }
                else if (PlayersinLift == 2 && status == "stop" && currentFloor > 0 && PlayersOnFloor)
                {
                    if (timeWaiting >= totalTime)
                    {
                        NextAnim();
                        timeWaiting = 0;
                    }
                    else
                    {
                        timeWaiting += Time.deltaTime;
                    }
                }
                else if (PlayersinLift == 0 && status == "stop" && currentFloor > 0 && !PlayersOnFloor)
                {
                    if (timeWaiting >= totalTime)
                    {
                        NextAnim();
                        timeWaiting = 0;
                    }
                    else
                    {
                        timeWaiting += Time.deltaTime;
                    }
                }
                else
                {
                    timeWaiting = 0;
                }
            }
        }

        public void DeleteCollider(string which)
        {
            if (which == "g1")
            {
                g1Collider.SetActive(false);
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

        //Set player parent
        public void SetParent(int startorend)
        {
            if(PlayersinLift == 2 && PlayersOnFloor)
                photonView.RPC("RPC_SetParent", RpcTarget.All, startorend);
        }

        public void SetCameraLimit (string which)
        {
            if (PlayersinLift == 2)
            {
                camera.GetComponent<PlayerCamera>().which = which;
                if (which == "LifttoFF")
                {
                    camera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(camera.GetComponent<Camera>().orthographicSize, 6.8f, 1.5f);
                }
                else if (which == "")
                {
                    camera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(camera.GetComponent<Camera>().orthographicSize, 7f, 1f);
                }
            }
        }

        public void LiftBell()
        {
            LevelsAudioManager.Instance.SetClip(Bell, "LiftBellArrive");
            Bell.GetComponent<Animator>().SetTrigger("Play");
        }

        public void LiftArrive()
        {
            GetComponent<AudioSource>().volume = Mathf.Lerp(GetComponent<AudioSource>().volume, 0, 2f);
            LevelsAudioManager.Instance.SetClip(Floor, "LiftFloorArrive");
            StartCoroutine(Arrive());
        }

        public void NextAnim()
        {
            photonView.RPC("RPC_LiftStart", RpcTarget.All);          
        }

        public void LineControll(int status)
        {
            //1 = start, 0 = stop
            if(status == 1)
                Line.GetComponent<Animator>().enabled = true;
            else if(status == 0)
                Line.GetComponent<Animator>().enabled = false;
        }

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

        IEnumerator Arrive()
        {
            yield return new WaitForSeconds(2f);
            currentFloor += 1;
            status = "stop";
        }

        IEnumerator OpenDoor(string which)
        {
            if (which == "left")
            {
                LevelsAudioManager.Instance.SetClip(LeftDoor, "LiftDoorOpen");
            }
            else if (which == "right")
            {
                LevelsAudioManager.Instance.SetClip(RightDoor, "LiftDoorOpen");
            }
            yield return new WaitForSeconds(1f);
            if (which == "left")
            {
                LeftDoor.GetComponent<Collider2D>().isTrigger = true;
                LeftDoor.layer = LayerMask.NameToLayer("Default");
            }
            else if (which == "right")
            {
                RightDoor.GetComponent<Collider2D>().isTrigger = true;
                RightDoor.layer = LayerMask.NameToLayer("Default");
            }
        }


        IEnumerator CloseDoorSoon(string which)
        {
            if (which == "left")
            {
                LeftDoor.GetComponent<Collider2D>().isTrigger = false;
                LeftDoor.layer = LayerMask.NameToLayer("Wall");
                LevelsAudioManager.Instance.SetClip(LeftDoor, "LiftDoorClose");

            }
            else if (which == "right")
            {
                RightDoor.GetComponent<Collider2D>().isTrigger = false;
                RightDoor.layer = LayerMask.NameToLayer("Wall");
                LevelsAudioManager.Instance.SetClip(RightDoor, "LiftDoorClose");
            }
            yield return new WaitForSeconds(1f);
        }

        IEnumerator CloseDoor(string which)
        {
            yield return new WaitForSeconds(1f);
            if (which == "left")
            {
                LeftDoor.GetComponent<Collider2D>().isTrigger = false;
                LeftDoor.layer = LayerMask.NameToLayer("Wall");
                LevelsAudioManager.Instance.SetClip(LeftDoor, "LiftDoorClose");
                
            }
            else if (which == "right")
            {
                RightDoor.GetComponent<Collider2D>().isTrigger = false;
                RightDoor.layer = LayerMask.NameToLayer("Wall");
                LevelsAudioManager.Instance.SetClip(RightDoor, "LiftDoorClose");
                
            }
        }

        [PunRPC]
        void RPC_NextAnim(int startorend)
        {
            //start = 1, end = 0
            if (startorend == 1)
            {
                players[0].transform.SetParent(gameObject.transform);
                players[1].transform.SetParent(gameObject.transform);
            }
            else if (startorend == 0)
            {
                players[0].transform.SetParent(null);
                players[1].transform.SetParent(null);
            }
        }

        [PunRPC]
        void RPC_LiftStart()
        {
            if (status == "stop")
            {
                status = "run";
                //start = 1, end = 0
                if (PlayersinLift == 2 && PlayersOnFloor)
                {
                    players[0].GetComponent<PlayerController>().isAnima = true;
                    players[1].GetComponent<PlayerController>().isAnima = true;
                    players[0].transform.SetParent(gameObject.transform);
                    players[1].transform.SetParent(gameObject.transform);
                }
                GetComponent<Animator>().SetTrigger("Play");
            }
        }

        [PunRPC]
        void RPC_SetParent(int startorend)
        {
            if (PlayersinLift == 2 && PlayersOnFloor)
            {
                //start = 1, end = 0
                if (startorend == 1)
                {
                    players[0].transform.SetParent(gameObject.transform);
                    players[1].transform.SetParent(gameObject.transform);
                }
                else if (startorend == 0)
                {
                    players[0].transform.SetParent(null);
                    players[1].transform.SetParent(null);
                    players[0].GetComponent<PlayerController>().isAnima = false;
                    players[1].GetComponent<PlayerController>().isAnima = false;
                }
            }
        }
    }
}
