using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Friendship
{
    public class itemController : MonoBehaviour,IPunObservable
    {
        [SerializeField] Transform itemSprite;
        Vector3 targetPosition, targetScale;
        Quaternion targetRotation;
        Transform targetParent;
        public PhotonView photonView;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            SmoothMove();
        }

        //Photon method to send and get data
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(transform.parent);
                stream.SendNext(transform.position);
                stream.SendNext(transform.localScale);
                stream.SendNext(transform.rotation);
            }
            else
            {
                targetParent = (Transform)stream.ReceiveNext();
                targetPosition = (Vector3)stream.ReceiveNext();
                targetScale = (Vector3)stream.ReceiveNext();
                targetRotation = (Quaternion)stream.ReceiveNext();
            }
        }

        void SmoothMove()
        {
            transform.SetParent(targetParent);
            //Sync scale
            itemSprite.localScale = targetScale;

            //Set position using lerp
            transform.position = Vector3.Lerp(transform.position, targetPosition, 0.2f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 720f * Time.deltaTime);
        }

    }
}
