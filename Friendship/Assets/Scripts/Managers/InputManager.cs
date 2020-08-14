using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Friendship
{
    public class InputManager
    {
        static Dictionary<string, KeyCode> keyMapping;
        static string[] keyMaps = new string[8]
        {
        "Left",
        "Right",
        "Jump",
        "Crouch",
        "Interact",
        "Listen",
        "Speedup",
        "Escape"
        };
        static KeyCode[] defaults = new KeyCode[8]
        {
        KeyCode.A,
        KeyCode.D,
        KeyCode.W,
        KeyCode.S,
        KeyCode.E,
        KeyCode.Q,
        KeyCode.LeftShift,
        KeyCode.Escape
        };

        private static void InitializeDictionary()
        {
            keyMapping = new Dictionary<string, KeyCode>();
            for (int i = 0; i < keyMaps.Length; ++i)
            {
                keyMapping.Add(keyMaps[i], defaults[i]);
            }
        }

        public static void SetKeyMap(string keyMap, KeyCode key)
        {
            if (!keyMapping.ContainsKey(keyMap))
                throw new ArgumentException("Invalid KeyMap in SetKeyMap: " + keyMap);
            keyMapping[keyMap] = key;
        }

        public static bool GetKeyDown(string keyMap)
        {
            return Input.GetKeyDown(keyMapping[keyMap]);
        }

        public static bool GetKey(string keyMap)
        {
            return Input.GetKey(keyMapping[keyMap]);
        }

        static InputManager()
        {
            InitializeDictionary();
        }

        public static float GetAxis(string whichaxis)
        {
            if (whichaxis == "Horizontal")
            {
                if (Input.GetKey(keyMapping["Left"]) && !Input.GetKey(keyMapping["Right"]))
                {
                    return -1;
                }
                else if (Input.GetKey(keyMapping["Right"]) && !Input.GetKey(keyMapping["Left"]))
                {
                    return 1;
                }
                else
                    return 0;
            }
            return 0;
        }

        //Static horizontal input
        public static float Horizontal
        {
            get
            {
                if (LevelsManager.Instance.isGame) //if Game = true
                                                 //if PC
#if UNITY_STANDALONE
                    return GetAxis("Horizontal");
#endif
                    //if mobile
#if UNITY_IOS || UNITY_ANDROID
                    return VirtualJoystick.joystickMoveDir.x;

#endif
                else
                    return 0;
            }
        }

        public static bool Pause
        {
            get
            {
                if (LevelsManager.Instance.isGame)
                    return GetKeyDown("Escape");
                else
                    return false;
            }
        }

        public static bool Interact
        {
            get
            {
                if (LevelsManager.Instance.isGame)
                    return GetKeyDown("Interact");
                else
                    return false;
            }
        }
    }
}