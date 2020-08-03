using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Friendship
{
    public class PlayerController : MonoBehaviourPun, IPunObservable
    {
        //Components
        PhotonView photonView;
        Animator animator, wheelanimator;
        PlayerCamera playerCamera;

        [Header("Parameters")]
        [SerializeField] float speedMove;
        [Header("Components")]
        [SerializeField] Transform playerSprite;

        private bool cameraset = false;
        //have an item on hand or not
        public bool isPick = false;
        public GameObject iteminhand;
        public List<GameObject> pickableitems;
        public Transform righthand;

        Vector3 targetPosition, targetScale;
        Quaternion targetRotation;

        public string currentScene = "";

        #region// Unity

        private void Start()
        {
            pickableitems = new List<GameObject>();
            photonView = GetComponent<PhotonView>();
            if (PlayerPrefs.GetInt("myCharacter") == 0)
            {
                if (photonView.IsMine)
                {
                    animator = GetComponentInChildren<Animator>();
                }
                else
                {
                    foreach (Transform obj in GetComponent<Transform>())
                    {
                        if (obj.gameObject.name == "deaf")
                        {
                            animator = obj.gameObject.GetComponentInChildren<Animator>();
                        }
                        else if (obj.gameObject.name == "wheelchair walk")
                        {
                            wheelanimator = obj.gameObject.GetComponentInChildren<Animator>();
                        }
                    }

                }
            }
            else
            {
                if (photonView.IsMine)
                {
                    foreach (Transform obj in GetComponent<Transform>())
                    {
                        if (obj.gameObject.name == "deaf")
                        {
                            animator = obj.gameObject.GetComponentInChildren<Animator>();
                        }
                        else if (obj.gameObject.name == "wheelchair walk")
                        {
                            wheelanimator = obj.gameObject.GetComponentInChildren<Animator>();
                        }
                    }
                }
                else
                {
                    animator = GetComponentInChildren<Animator>();
                }
            }

        }

        #endregion

        private void Update()
        {
            if (SceneManager.GetActiveScene().name.Contains("Level"))
            {
                //if photonview is you
                if (photonView.IsMine)
                {
                    if (!LevelsManager.Instance.isPause)
                    {
                        CheckPickup();
                    }
                }
            }
        }

        private void FixedUpdate()
        {

            //Pause check
            if (SceneManager.GetActiveScene().name.Contains("Level"))
            {
                if (!cameraset)
                {
                    //Find camera
                    playerCamera = Camera.main.GetComponent<PlayerCamera>();

                    //If photonview is your
                    if (photonView.IsMine)
                    {
                        //Set player target
                        playerCamera.player = transform;
                        cameraset = true;
                        //Debug.Log("cameraset = true");
                    }
                }
                //if photonview is you
                if (photonView.IsMine)
                {
                    if (!LevelsManager.Instance.isPause)
                    {
                        Move();
                        Animation(CheckInput().x, CheckInput().y);
                    }
                }
                else //if is another player
                {
                    SmoothMove();
                }
            }
        }


        //Photon method to send and get data
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.localScale);
            }
            else
            {
                targetPosition = (Vector3)stream.ReceiveNext();
                targetScale = (Vector3)stream.ReceiveNext();
            }
        }

        //Check input method
        Vector3 CheckInput()
        {
            //set local variables
            float xMove = InputManager.Horizontal * speedMove * Time.deltaTime;
            //float yMove = InputManager.Vertical * speedMove * Time.deltaTime;

            Vector2 move = new Vector2(xMove, 0);

            return move;
        }

        void CheckPickup()
        {
            if (InputManager.Interact)
            {
                Debug.Log("Epressed");
                if (isPick && iteminhand != null)
                {
                    Debug.Log("Putdown");
                    ManualAnimation("isThrow");
                    Vector3 temp = iteminhand.transform.parent.position;
                    iteminhand.transform.SetParent(null);
                    iteminhand.transform.position = temp;
                    iteminhand.GetComponent<Rigidbody2D>().gravityScale = 1;
                    iteminhand.GetComponent<itemState>().pickuphand = null;
                    iteminhand = null;
                    photonView.RPC("RPC_SyncPutdown", RpcTarget.Others);
                    isPick = false;
                }
                else if (pickableitems.Count > 0)
                {
                    Debug.Log("Pickup");
                    iteminhand = pickableitems[0];
                    ManualAnimation("isPick");
                    //iteminhand.transform.position = righthand.position;
                    iteminhand.GetComponent<Rigidbody2D>().gravityScale = 0;
                    iteminhand.GetComponent<itemState>().pickuphand = righthand;
                    iteminhand.transform.position = righthand.position;
                    iteminhand.transform.SetParent(righthand);
                    photonView.RPC("RPC_SyncPickup", RpcTarget.Others);
                    isPick = true;
                }
            }
 
        }

        Vector3 CheckSmoothMove()
        {
            //set local variables
            float xMove = targetPosition.x - transform.position.x;
            //float yMove = InputManager.Vertical * speedMove * Time.deltaTime;

            Vector2 move = new Vector2(xMove, 0);

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
            //Sync scale
            playerSprite.localScale = targetScale;

            //Set position using lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);
        }

        //Rotation method
        void Rotation(float x)
        {
            //Change X in scale
            if (x > 0)
                playerSprite.localScale = new Vector2(Mathf.Abs(playerSprite.localScale.x), Mathf.Abs(playerSprite.localScale.y));
            else if (x < 0)
                playerSprite.localScale = new Vector2(Mathf.Abs(playerSprite.localScale.x) * -1 , Mathf.Abs(playerSprite.localScale.y));
        }

        //Animation method
        void Animation(float x, float y)
        {
            //If player is't stay
            if (x != 0 || y != 0)
            {
                animator.SetBool("isWalk", true);
                photonView.RPC("RPC_SyncAnimation", RpcTarget.Others, "isWalk", true);
            }
            else
            {
                animator.SetBool("isWalk", false);
                photonView.RPC("RPC_SyncAnimation", RpcTarget.Others, "isWalk", false);
            }
        }

        void ManualAnimation(string aniName)
        {
            if (aniName == "isPick")
            {
                animator.SetTrigger("isPick");
                photonView.RPC("RPC_SyncAnimation", RpcTarget.Others, "isPick", true);
            }
            else if (aniName == "isThrow")
            {
                animator.SetTrigger("isThrow");
                //photonView.RPC("RPC_SyncAnimation", RpcTarget.Others, "isThrow", true);
            }
        }

        //RPC functions
        [PunRPC]
        void RPC_SyncAnimation(string animation, bool state)
        {
            if (!photonView.IsMine)
                animator.SetBool(animation, state);
        }

        [PunRPC]
        void RPC_SyncPickup()
        {
            if (!photonView.IsMine)
            {
                iteminhand = pickableitems[0];
                iteminhand.GetComponent<itemState>().pickuphand = righthand;
                iteminhand.transform.SetParent(righthand);
            }
        }

        [PunRPC]
        void RPC_SyncPutdown()
        {
            if (!photonView.IsMine)
            {
                iteminhand.GetComponent<itemState>().pickuphand = null;
                iteminhand.transform.SetParent(null);
            }
        }

    }
}
