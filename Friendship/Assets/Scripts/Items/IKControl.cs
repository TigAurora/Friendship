using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class IKControl : MonoBehaviour
    {
        protected Animator animator;
        public bool ikActive = true;
        public Transform rightHandObj = null;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        void OnAnimatorIK()
        {
            if (animator)
            {
                if (ikActive)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
                    //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
                    if (rightHandObj != null)
                    {
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, rightHandObj.position);
                        //animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);
                    }

                }

                else
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                    //animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
                }
            }
        }
        }
    }