using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Experimental.U2D.Animation;

namespace Friendship
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] float smoothFollow;
        [SerializeField] Vector2 minCameraPos, maxCameraPos; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_LifttoFF, maxCameraPos_LifttoFF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_FF, maxCameraPos_FF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_LifttoSF, maxCameraPos_LifttoSF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_SF, maxCameraPos_SF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_LifttoTF, maxCameraPos_LifttoTF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_TF, maxCameraPos_TF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_LifttoFoF, maxCameraPos_LifttoFoF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_FoF, maxCameraPos_FoF; //Min and max camera position
        [SerializeField] Vector2 minCameraPos_LifttoEND, maxCameraPos_LifttoEND; //Min and max camera position
        [SerializeField] public string which = "";

        float tempminx, tempmaxx, tempminy, tempmaxy;
        public Transform player;

        void Start()
        {
        }

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
            //Debug.Log("Camera moving: camera position = " + transform.position);
        }

        //Clam method
        Vector3 CameraClamp(ref Vector3 value)
        {
            if (which == "LifttoFF")
            {
                tempminx = minCameraPos_LifttoFF.x;
                tempmaxx = maxCameraPos_LifttoFF.x;
                tempminy = minCameraPos_LifttoFF.y;
                tempmaxy = maxCameraPos_LifttoFF.y;
            }
            else if (which == "FF")
            {
                tempminx = minCameraPos_FF.x;
                tempmaxx = maxCameraPos_FF.x;
                tempminy = minCameraPos_FF.y;
                tempmaxy = maxCameraPos_FF.y;
            }
            else if (which == "LifttoSF")
            {
                tempminx = minCameraPos_LifttoSF.x;
                tempmaxx = maxCameraPos_LifttoSF.x;
                tempminy = minCameraPos_LifttoSF.y;
                tempmaxy = maxCameraPos_LifttoSF.y;
            }
            else if (which == "SF")
            {
                tempminx = minCameraPos_SF.x;
                tempmaxx = maxCameraPos_SF.x;
                tempminy = minCameraPos_SF.y;
                tempmaxy = maxCameraPos_SF.y;
            }
            else if (which == "LifttoTF")
            {
                tempminx = minCameraPos_LifttoTF.x;
                tempmaxx = maxCameraPos_LifttoTF.x;
                tempminy = minCameraPos_LifttoTF.y;
                tempmaxy = maxCameraPos_LifttoTF.y;
            }
            else if (which == "TF")
            {
                tempminx = minCameraPos_TF.x;
                tempmaxx = maxCameraPos_TF.x;
                tempminy = minCameraPos_TF.y;
                tempmaxy = maxCameraPos_TF.y;
            }
            else if (which == "LifttoFoF")
            {
                tempminx = minCameraPos_LifttoFoF.x;
                tempmaxx = maxCameraPos_LifttoFoF.x;
                tempminy = minCameraPos_LifttoFoF.y;
                tempmaxy = maxCameraPos_LifttoFoF.y;
            }
            else if (which == "FoF")
            {
                tempminx = minCameraPos_FoF.x;
                tempmaxx = maxCameraPos_FoF.x;
                tempminy = minCameraPos_FoF.y;
                tempmaxy = maxCameraPos_FoF.y;
            }
            else if (which == "LifttoEND")
            {
                tempminx = minCameraPos_LifttoEND.x;
                tempmaxx = maxCameraPos_LifttoEND.x;
                tempminy = minCameraPos_LifttoEND.y;
                tempmaxy = maxCameraPos_LifttoEND.y;
            }
            else if (which == "")
            {
                tempminx = minCameraPos.x;
                tempmaxx = maxCameraPos.x;
                tempminy = minCameraPos.y;
                tempmaxy = maxCameraPos.y;
            }

            //Change x and y in a given vector 
            value.x = Mathf.Clamp(value.x, tempminx, tempmaxx);
            value.y = Mathf.Clamp(value.y, tempminy, tempmaxy);
            return value; //return with changes
        }
    }
}