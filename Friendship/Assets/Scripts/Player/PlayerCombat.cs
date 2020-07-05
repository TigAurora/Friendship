using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheBattle
{
    public class PlayerCombat : MonoBehaviour
    {
        //Components
        PhotonView photonView;
        Animator animator;

        PlayerCombatTrigger combatTrigger;
        PlayerData playerData;

        #region //Unity

        private void Start()
        {
            //Get components
            photonView = GetComponent<PhotonView>();
            animator = GetComponentInChildren<Animator>();

            combatTrigger = GetComponentInChildren<PlayerCombatTrigger>();
            playerData = GetComponent<PlayerData>();

            //Add to delegate in CombatTrigger.cs out hit, when the trigger is activated, it will cause a hit
            combatTrigger.OnHit += Hit;
        }

        #endregion

        void Hit(Collider2D hitCollider)
        {
            //Check collider
            if (hitCollider.gameObject.tag == "Player" && hitCollider.gameObject != gameObject) //If enemy
            {
                //Get enemy photon view
                PhotonView enemyPhotonView = hitCollider.GetComponent<PhotonView>();

                enemyPhotonView.RPC("RPC_Hit", RpcTarget.All, new object[] { playerData.GetDamagePower() }); //Send all this enemy was hited. In paramaters send damage
            }
        }

        void AttackAnimation()
        {
            animator.SetTrigger("Attack");
        }

        private void Update()
        {
            //Check is you or not 
            if (photonView.IsMine)
            {
                //Pause check
                if (!GameManager.Instance.isPause)
                    if (InputManager.Attack) //If pressed attack button
                    {
                        AttackAnimation(); //Start attack animation

                        //When an animation is playing, Trigger is turned on at once
                    }
            }
        }

        #region //RPC

        //Hit method
        [PunRPC]
        void RPC_Hit(float damage)
        {
            playerData.GetDamage(damage);
        }

        #endregion

    }
}