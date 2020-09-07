using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    //This class need for multy-scene object
    //more https://docs.unity3d.com/ScriptReference/Object.DontDestroyOnLoad.html

    public class DontDestroyOnLoad : MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}
