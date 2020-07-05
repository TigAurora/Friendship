using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBattle
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] float smoothFollow;
        [SerializeField] Vector2 minCameraPos, maxCameraPos; //Min and max camera position
        [HideInInspector] public Transform player;

        private void FixedUpdate()
        {
            //Null referens check
            if (player != null)
                CameraMove();
        }

        void CameraMove()
        {
            //Get player position
            Vector3 playerPos = new Vector3(player.position.x, player.position.y, -10);
            //Make smooth move
            Vector3 smoothPos = Vector3.Lerp(transform.position, playerPos, smoothFollow * Time.deltaTime);

            //Make clamp and set camera position
            transform.position = CameraClamp(ref smoothPos);
        }

        //Clam method
        Vector3 CameraClamp(ref Vector3 value)
        {
            //Change x and y in a given vector 
            value.x = Mathf.Clamp(value.x, minCameraPos.x, maxCameraPos.x);
            value.y = Mathf.Clamp(value.y, minCameraPos.y, maxCameraPos.y);
            return value; //return with changes
        }
    }
}