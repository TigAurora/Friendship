using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.IO;
using Photon.Realtime;
using UnityEngine.SceneManagement;

namespace Friendship
{
    public class LevelsAudioManager : MonoBehaviour
    {
        public LevelsAudioComponents components;

        //Signleton
        public static LevelsAudioManager Instance;

        #region //Unity
        private void Awake()
        {
            SingletonInit();
            //Get components
            components.LevelAManager = GetComponent<LevelsManager>();
            components.photonView = GetComponent<PhotonView>();
            components.audioManager = new Dictionary<string, AudioClip>();

            foreach (AudioClip clip in components.audioClips)
            {
                if (!components.audioManager.ContainsValue(clip))
                {
                    components.audioManager.Add(clip.name,clip);
                    Debug.Log(clip.name);
                }
            }
        }

        void SingletonInit()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;
        }
        #endregion
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //Play sound base on environment
        public void SetClip(GameObject Obj, string environment)
        {
            //Set AudioClip and Volume for Correct or Wrong Answer
            if (Obj.gameObject.name.Contains("lamp"))
            {
                if (environment == "correct")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["CorrectAnswer"];
                }
                else if (environment == "wrong")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["WrongAnswer"];
                }
                Obj.gameObject.GetComponent<AudioSource>().Play();
            }
            //Set AudioClip and Volume for Frog and Chick button
            if (Obj.gameObject.name.Contains("g1p4"))
            {
                if (environment == "frog")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["Frog"];
                }
                else if (environment == "chick")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["Chick"];
                }
                Obj.gameObject.GetComponent<AudioSource>().Play();
            }

            //Set AudioClip and Volume for apple
                if (Obj.gameObject.name.Contains("apple"))
            {
                if (environment == "wood")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["AppleDropOnWood"];
                }
                else if (environment == "other")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["AppleDropOnOther"];
                }
                if (Obj.gameObject.GetComponent<itemState>().maxVel / 12 > 1)
                {
                    Obj.gameObject.GetComponent<AudioSource>().volume = 0.3f;
                    Obj.gameObject.GetComponent<AudioSource>().Play();
                }
                //else if (Obj.gameObject.GetComponent<itemState>().maxVel / 12 < 0)
                //{
                // Obj.gameObject.GetComponent<AudioSource>().volume = 0.1f;
                // Obj.gameObject.GetComponent<AudioSource>().Play();
                //}
                else
                {
                    Obj.gameObject.GetComponent<AudioSource>().volume = (Obj.gameObject.GetComponent<itemState>().maxVel / 12) * 0.3f;
                    Obj.gameObject.GetComponent<AudioSource>().Play();
                }
                Obj.gameObject.GetComponent<itemState>().maxVel = 0;
            }
            else if (Obj.gameObject.name.Contains("Lift") || Obj.gameObject.name.Contains("lift"))
            {
                if (environment == "Start")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["ElevatorMove"];
                }
                else if (environment == "LiftFloorArrive")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["ElevatorArrive"];
                }
                else if (environment == "LiftBellArrive")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["ElevatorBell"];
                }
                else if (environment == "LiftDoorOpen")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["ElevatorDoorOpen"];
                }
                else if (environment == "LiftDoorClose")
                {
                    Obj.gameObject.GetComponent<AudioSource>().clip = components.audioManager["ElevatorDoorClose"];
                }
                Obj.gameObject.GetComponent<AudioSource>().enabled = true;
                Obj.gameObject.GetComponent<AudioSource>().Play();
            }
        }

        //Components container
        [Serializable]
        public struct LevelsAudioComponents
        {
            [HideInInspector] public PhotonView photonView;
            [HideInInspector] public LevelsManager LevelAManager;
            [HideInInspector] public Dictionary<string, AudioClip> audioManager;

            [Header("Audios Used in Level")]
            public AudioClip[] audioClips;
        }
    }
}