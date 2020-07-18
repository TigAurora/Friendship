using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class PlayerController : MonoBehaviourPun, IPunObservable
    {
        //Components
        PhotonView photonView;
        Animator animator;
        PlayerCamera playerCamera;

        [Header("Parameters")]
        [SerializeField] float speedMove;
        [Header("Components")]
        [SerializeField] Transform playerSprite;

        Vector3 targetPosition;
        Quaternion targetRotation;

        #region// Unity

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
            animator = GetComponentInChildren<Animator>();

            //Find camera
            playerCamera = Camera.main.GetComponent<PlayerCamera>();

            //If photonview is your
            if (photonView.IsMine)
            {
                //Set player target
                playerCamera.player = transform;
            }
        }

        #endregion

        private void FixedUpdate()
        {
            //if photonviw is your
            if (photonView.IsMine)
            {
                //Pause check
                if (!GameManager.Instance.isPause)
                    Move();
            }
            else //if is another player
                SmoothMove();

            //Animation();
        }

        //Photon method to send and get data
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
            }
            else
            {
                targetPosition = (Vector3)stream.ReceiveNext();
            }
        }

        //Check input method
        Vector3 CheckInput()
        {
            //set local variables
            float xMove = InputManager.Horizontal * speedMove * Time.deltaTime;
            float yMove = InputManager.Vertical * speedMove * Time.deltaTime;

            Vector2 move = new Vector2(xMove, yMove);

            return move;
        }

        //Move method
        void Move()
        {
            //Rotate by X move 
            Rotation(CheckInput().x);

            transform.Translate(CheckInput());
        }

        //Smooth move, if controller is another player
        void SmoothMove()
        {
            //Set position using lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);
        }

        //Rotation method
        void Rotation(float x)
        {
            //Change X in scale
            if (x > 0)
                playerSprite.localScale = new Vector2(1, 1);
            else if (x < 0)
                playerSprite.localScale = new Vector2(-1, 1);
        }

        //Animation method
        //void Animation()
        //{
            //If player is't stay
            //if (CheckInput().x != 0 || CheckInput().y != 0)
                //animator.SetBool("isMove", true);
            //else
                //animator.SetBool("isMove", false);
        //}

    }
}
