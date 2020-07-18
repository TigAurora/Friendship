using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class InputManager
    {
        //Static horizontal input
        public static float Horizontal
        {
            get
            {
                if (GameManager.Instance.isGame) //if Game = true
                                                 //if PC
#if UNITY_STANDALONE
                    return Input.GetAxis("Horizontal");
#endif
                    //if mobile
#if UNITY_IOS || UNITY_ANDROID
                    return VirtualJoystick.joystickMoveDir.x;

#endif
                else
                    return 0;
            }
        }

        public static float Vertical
        {
            get
            {
                if (GameManager.Instance.isGame)
#if UNITY_STANDALONE
                    return Input.GetAxis("Vertical");
#endif
#if UNITY_IOS || UNITY_ANDROID
                    return VirtualJoystick.joystickMoveDir.z;
#endif
                else
                    return 0;
            }
        }

        public static bool Attack
        {
            get
            {
                if (GameManager.Instance.isGame)
                    return Input.GetMouseButtonDown(0);
                else
                    return false;
            }
        }

        public static bool Pause
        {
            get
            {
                if (GameManager.Instance.isGame)
                    return Input.GetKeyDown(KeyCode.Escape);
                else
                    return false;
            }
        }
    }
}