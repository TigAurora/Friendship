using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.U2D.IK;

namespace Friendship
{
    public class PlayerController : MonoBehaviourPun, IPunObservable
    {
        //Components
        PhotonView photonView;
        Animator animator;
        PlayerCamera playerCamera;
        DetectWallAndGround detect;
        GameObject wheelchair;

        [Header("Parameters")]
        [SerializeField] public float speedMove;
        [SerializeField] public float speedShift;
        [SerializeField] public float validpickrange;
        [SerializeField] public float validthrowrange;
        public bool isPick = false, isGround = false, isAnima = false, CheckPickAnim = false, isSync = true;
        //status
        public bool isWalk = false, isRun = false, isFalling = false;

        [Header("Transforms")]
        [SerializeField] public Transform playerSprite;
        [SerializeField] public Transform pickuphand, pickupIK, defaultIK, pointIK, pointDefaultIK, armlDefault;

        [Header("LayerMask")]
        [SerializeField] public LayerMask groundLayer;
        [SerializeField] public LayerMask wallLayer;

        [HideInInspector] public bool cameraset = false;
        [HideInInspector] public GameObject iteminhand;
        [HideInInspector] Vector3 targetPosition, targetScale;
        [HideInInspector] Quaternion targetRotation;
        [HideInInspector] public string currentScene = "";

        [HideInInspector] public List<GameObject> pickableitems;
        [HideInInspector] public List<GameObject> interactiveitems;
        public int thisCharacter = -1;


        #region// Unity

        private void Start()
        {
            pickableitems = new List<GameObject>();
            interactiveitems = new List<GameObject>();
            photonView = GetComponent<PhotonView>();
            detect = GetComponent<DetectWallAndGround>();
            if (transform.name.Contains("deaf"))
            {
                thisCharacter = 1;
            }
            else if (transform.name.Contains("blind"))
            {
                thisCharacter = 0;
            }
            if (PlayerNetwork.Instance.myCharacter == 0)
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
                        else if (obj.gameObject.name == "wheelchair")
                        {
                            wheelchair = obj.gameObject;
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
                        else if (obj.gameObject.name == "wheelchair")
                        {
                            wheelchair = obj.gameObject;
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
                        CheckInteraction();
                        if (detect.IsGround())
                        {
                            if (isFalling)
                            {
                                isFalling = false;
                                isAnima = false;
                                photonView.RPC("RPC_SyncBool", RpcTarget.All, "isFall", false, thisCharacter);
                            }
                        }
                        else if (GetComponent<Rigidbody2D>().velocity.y < 0)
                        {
                            isAnima = true;
                            isFalling = true;
                            isGround = false;
                            photonView.RPC("RPC_SyncBool", RpcTarget.All, "isFall", true, thisCharacter);
                        }

                    }
                }
            }
            else if (SceneManager.GetActiveScene().name == "CharacterCreator")
            {
                if (detect.IsGround())
                {
                    if (isFalling)
                    {
                        isFalling = false;
                        animator.SetBool("isFall", false);
                    }
                }
                else if (GetComponent<Rigidbody2D>().velocity.y < 0)
                {
                    isFalling = true;
                    isGround = false;
                    animator.SetBool("isFall", true);
                }
            }
        }

        private void FixedUpdate()
        {

            //Pause check
            if (SceneManager.GetActiveScene().name.Contains("Level"))
            {
                if (photonView.IsMine)
                {
                    if (!cameraset)
                    {
                        //Find camera
                        playerCamera = Camera.main.GetComponent<PlayerCamera>();
                        //If photonview is your
                        //Set player target
                        playerCamera.player = transform;
                        cameraset = true;
                        //Debug.Log("cameraset = true");
                    }
                    if (!LevelsManager.Instance.isPause)
                    {
                        Move();
                        Animation(CheckInput("").x, CheckInput("").y);
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
            if (isSync)
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
        }

        //Check input method
        Vector3 CheckInput(string whichWall)
        {
            float xMove = 0;
            float isShift = 0;

            if (isAnima)
                return new Vector2(0, 0);
            //set local variables
            if (InputManager.GetKey("Speedup"))
            {
                isShift = 1;
            }
            if (whichWall == "LeftWall")
            {
                if (InputManager.Horizontal > 0)
                    xMove = InputManager.Horizontal * (speedMove + speedShift * isShift) * Time.deltaTime;
            }
            else if (whichWall == "RightWall")
            {
                if (InputManager.Horizontal < 0)
                    xMove = InputManager.Horizontal * (speedMove + speedShift * isShift) * Time.deltaTime;
            }
            else
                xMove = InputManager.Horizontal * (speedMove + speedShift * isShift) * Time.deltaTime;
            //float yMove = InputManager.Vertical * speedMove * Time.deltaTime;

            Vector2 move = new Vector2(xMove, 0);


            return move;
        }

        public void CheckInteraction()
        {
            if (InputManager.GetKeyDown("Interact") && !isAnima && InputManager.Horizontal == 0)
            {
                Debug.Log("Epressed");
                if (interactiveitems.Count > 0)
                {
                    interactiveitems[0].GetComponent<InteractiveItem>().InteractiveAction();
                }
                else if (isPick && iteminhand != null)
                {
                    if (ValidRange(validthrowrange))
                    {
                        Putdown(1);
                    }
                }
                else if (pickableitems.Count > 0)
                {
                    for (int i = 0; i < pickableitems.Count; ++i)
                    {
                        //Judge whether pick range is valid
                        if (pickableitems[i].GetComponent<itemState>().isGround)
                        {
                            if (ValidRange(validpickrange))
                            {
                                if (transform.position.x > pickableitems[0].transform.position.x)
                                {
                                    if (transform.localScale.x < 0)
                                    {
                                        Debug.Log("Pickup");
                                        Pickup(1);
                                        i = pickableitems.Count;
                                    }
                                }
                                else if (transform.position.x < pickableitems[0].transform.position.x)
                                {
                                    if (transform.localScale.x > 0)
                                    {
                                        Debug.Log("Pickup");
                                        Pickup(1);
                                        i = pickableitems.Count;
                                    }
                                }
                                //iteminhand.transform.position = righthand.position;
                            }
                        }
                    }
                }
            }
 
        }

        bool ValidRange(float distance)
        {
            //Face Left
            if (transform.localScale.x < 0)
            {
                if (detect.DistanceFromWall(1) < distance && detect.DistanceFromWall(1) != -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (transform.localScale.x > 0)
            {
                if (detect.DistanceFromWall(2) < distance && detect.DistanceFromWall(2) != -1)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
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
            Rotation(CheckInput("").x);

            if(detect.IsWall() == 1 || detect.IsAbyss() == 1)
                transform.Translate(CheckInput("LeftWall"));
            else if (detect.IsWall() == 2 || detect.IsAbyss() == 2)
                transform.Translate(CheckInput("RightWall"));
            else
                transform.Translate(CheckInput(""));
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

        public void TestChange()
        {
            defaultIK.position = new Vector3(10,10,10);
        }

        //Animation method
        void Animation(float x, float y)
        {
            //If player is't stay
            if (x != 0 || y != 0)
            {
                if ((x > 0 && (detect.IsWall() != 2) && (detect.IsAbyss() != 2)) || (x < 0 && (detect.IsWall() != 1) && (detect.IsAbyss() != 1)))
                {
                    if (InputManager.GetKey("Speedup"))
                    {
                        photonView.RPC("RPC_SyncBool", RpcTarget.All, "isRun", true, thisCharacter);
                        photonView.RPC("RPC_SyncBool", RpcTarget.All, "isWalk", false, thisCharacter);
                    }
                    else
                    {
                        photonView.RPC("RPC_SyncBool", RpcTarget.All, "isRun", false, thisCharacter);
                        photonView.RPC("RPC_SyncBool", RpcTarget.All, "isWalk", true, thisCharacter);
                    }
                }
                else
                {
                    photonView.RPC("RPC_SyncBool", RpcTarget.All, "isRun", false, thisCharacter);
                    photonView.RPC("RPC_SyncBool", RpcTarget.All, "isWalk", false, thisCharacter);
                }
            }
            else
            {
                photonView.RPC("RPC_SyncBool", RpcTarget.All, "isRun", false, thisCharacter);
                photonView.RPC("RPC_SyncBool", RpcTarget.All, "isWalk", false, thisCharacter);
            }

        }

        #region //Support functions      
        public void ThrowItem()
        {
            if (photonView.IsMine)
                photonView.RPC("RPC_SyncThrowItem", RpcTarget.All, transform.name);
        }

        public void Putdown(int stage)
        {
            if (photonView.IsMine)
                photonView.RPC("RPC_SyncPutdown", RpcTarget.All, transform.name, stage);
        }

        public void Pickup(int stage)
        {
            if (photonView.IsMine)
            {
                if (stage == 2)
                {
                    CheckPickAnim = false;
                }
                photonView.RPC("RPC_SyncPickup", RpcTarget.All, transform.name, stage);
            }
        }

        public void Anim_CorrectLeftArmPickSec()
        {
            //StartCoroutine(MoveOverSeconds(pointIK.gameObject, pointDefaultIK.position, 0.3f));
            CheckPickAnim = true;
            StartCoroutine(MoveOverSeconds(pointIK.gameObject, pointDefaultIK.position, 0.4f));
        }

        public void Anim_CorrectLeftArmPickFst()
        {
            StartCoroutine(MoveOverSeconds(pickupIK.gameObject, iteminhand.transform.position, 0.2f));
        }

        public void StartAnim()
        {
            isAnima = true;
        }

        public void FinishAnim()
        {
            isAnima = false;
        }

        public IEnumerator MoveOverSeconds(GameObject objectToMove, Vector3 end, float seconds)
        {
            //Debug.Log("MoveOverSeconds: Objecttomovename:" + objectToMove.name + "checkPickAnim = " + CheckPickAnim);
            float elapsedTime = 0;
            Vector3 startingPos = objectToMove.transform.position;
            while (elapsedTime < seconds)
            {
                objectToMove.transform.position = Vector3.Lerp(startingPos, end, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            objectToMove.transform.position = end;
        }

        #endregion

        #region        //RPC functions
        [PunRPC]
        void RPC_SyncTrigger(string animation)
        {
            animator.SetTrigger(animation);
            if (transform.name.Contains("deaf"))
            {
                wheelchair.GetComponentInChildren<Animator>().SetTrigger(animation);
            }
        }

        [PunRPC]
        void RPC_SyncBool(string animation, bool state, int which)
        {
            if (thisCharacter == which)
            {
                animator.SetBool(animation, state);
                if ((animation.Contains("Walk") || animation.Contains("Run")) && thisCharacter == 1)
                {
                    wheelchair.GetComponentInChildren<Animator>().SetBool(animation, state);
                    if (!wheelchair.GetComponentInChildren<AudioSource>().isPlaying &&
                            (wheelchair.GetComponentInChildren<Animator>().GetBool("isWalk") || wheelchair.GetComponentInChildren<Animator>().GetBool("isRun")))
                        wheelchair.GetComponentInChildren<AudioSource>().Play();
                    else if (wheelchair.GetComponentInChildren<AudioSource>().isPlaying &&
                            !wheelchair.GetComponentInChildren<Animator>().GetBool("isWalk") && !wheelchair.GetComponentInChildren<Animator>().GetBool("isRun"))
                        wheelchair.GetComponentInChildren<AudioSource>().Stop();

                }
            }
        }

        [PunRPC]
        void RPC_SyncThrowItem(string player)
        {
            if (transform.name == player && iteminhand != null)
            {
                Vector3 temp = iteminhand.transform.parent.position;
                iteminhand.transform.SetParent(null);
                iteminhand.transform.position = temp;
                iteminhand.GetComponent<Rigidbody2D>().gravityScale = 1;
                iteminhand.GetComponent<itemState>().pickuphand = null;
                iteminhand = null;
            }

        }

        [PunRPC]
        void RPC_SyncPutdown(string player, int stage)
        {
            //3 isThrow Trigger for 3 transition
            if (transform.name == player && stage == 1)
            {
                //Debug.Log("isThrow");
                animator.SetTrigger("isThrow");
            }
            else if (transform.name == player && stage == 2)
            {
                //Debug.Log("isThrow1");
                animator.SetTrigger("isThrow1");
            }
            else if (transform.name == player && stage == 3)
            {
                //Debug.Log("isThrow2");
                animator.SetTrigger("isThrow2");
                isPick = false;
            }
        }

        [PunRPC]
        void RPC_SyncPickup(string player, int stage)
        {
            //3 isPick Trigger for 3 transition
            if (transform.name == player && stage == 1)
            {
                //Debug.Log("isPick");
                isPick = true;
                CheckPickAnim = true;
                iteminhand = pickableitems[0];
                //pointIK.transform.position = new Vector3(iteminhand.transform.position.x,
                    //iteminhand.transform.position.y + iteminhand.GetComponent<itemState>().halfheight,
                    //iteminhand.transform.position.z);
                animator.SetTrigger("isPick");
            }
            else if (transform.name == player && stage == 2)
            {
                //Debug.Log("isPick1");
                pointIK.transform.position = iteminhand.transform.position;
                iteminhand.GetComponent<Rigidbody2D>().gravityScale = 0;
                iteminhand.GetComponent<itemState>().pickuphand = pickuphand;
                iteminhand.transform.SetParent(pickuphand);
                pickupIK.GetComponent<LimbSolver2D>().GetChain(0).target = pointIK;
                //CheckPickAnim = true;
                animator.SetTrigger("isPick1");
            }
            else if (transform.name == player && stage == 3)
            {
                //Debug.Log("isPick2");
                CheckPickAnim = false;
                pickupIK.transform.position = armlDefault.position;
                pickupIK.GetComponent<LimbSolver2D>().GetChain(0).target = defaultIK;
                animator.SetTrigger("isPick2");
            }
        }
        #endregion
    }
}
