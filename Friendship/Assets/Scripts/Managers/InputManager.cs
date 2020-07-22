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
                if (LevelLobbyManager.Instance.isLobby) //if Game = true
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

        public static bool Pause
        {
            get
            {
                if (LevelLobbyManager.Instance.isLobby)
                    return Input.GetKeyDown(KeyCode.Escape);
                else
                    return false;
            }
        }
    }
}