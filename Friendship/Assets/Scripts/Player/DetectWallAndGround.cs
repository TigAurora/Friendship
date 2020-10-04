using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class DetectWallAndGround : MonoBehaviour
    {
        public LayerMask groundLayer, wallLayer, abyssLayer;
        public float WalldetectDistance, AbyssdetectDistance, GrounddetectDistance;

        void Start()
        { }

        public int IsAbyss()
        {
            Vector2 position = transform.position;
            //Default direction = left
            Vector2 LDirection = Vector2.left;
            Vector2 RDirection = Vector2.right;
            RaycastHit2D Lhit = Physics2D.Raycast(position, LDirection, AbyssdetectDistance, abyssLayer);
            RaycastHit2D Rhit = Physics2D.Raycast(position, RDirection, AbyssdetectDistance, abyssLayer);
            if (Lhit.collider != null && Rhit.collider != null)
            {
                return 3;
            }
            else if (Lhit.collider == null && Rhit.collider == null)
            {
                return 0;
            }
            else if (Lhit.collider != null)
            {
                return 1;
            }
            else if (Rhit.collider != null)
            {
                return 2;
            }
            return 3;
        }

        //0 means no wall, 1 means left wall, 2 means right wall, 3 means both wall exist (error)
        public int IsWall()
        {
            Vector2 position = transform.position;
            //Default direction = left
            Vector2 LDirection = Vector2.left;
            Vector2 RDirection = Vector2.right;
            RaycastHit2D Lhit = Physics2D.Raycast(position, LDirection, WalldetectDistance, wallLayer);
            RaycastHit2D Rhit = Physics2D.Raycast(position, RDirection, WalldetectDistance, wallLayer);
            if (Lhit.collider != null && Rhit.collider != null)
            {
                return 3;
            }
            else if (Lhit.collider == null && Rhit.collider == null)
            {
                return 0;
            }
            else if (Lhit.collider != null) 
            {
                return 1;
            }
            else if (Rhit.collider != null)
            {
                return 2;
            }
            return 3;
        }

        public bool IsGround()
        {
            Vector2 position = transform.position;
            //Default direction = left
            Vector2 direction = Vector2.down;
            RaycastHit2D hit = Physics2D.Raycast(position, direction, GrounddetectDistance, groundLayer);

            if (hit.collider != null)
            {
                return true;
            }
            return false;
        }

        //void OnTriggerEnter2D(Collider2D other)
        //{
         //   if (other.tag == "Floor" && tag == "Player" && IsGround())
         //   {
         //       GetComponent<PlayerController>().isGround = true;
         //   }

        //}

        //void OnTriggerExit2D(Collider2D other)
        //{
         //   if (other.tag == "Floor" && tag == "Player" && !IsGround())
         //       GetComponent<PlayerController>().isGround = false;
        //}

        public float DistanceFromWall(int dir)
        {
            Debug.Log("DistanceFromWall Detect");
            Vector2 position = transform.position;
            Vector2 LDirection = Vector2.left;
            Vector2 RDirection = Vector2.right;
            RaycastHit2D Lhit = Physics2D.Raycast(position, LDirection, 5f, wallLayer);
            RaycastHit2D Rhit = Physics2D.Raycast(position, RDirection, 5f, wallLayer);
            //1 = Left Wall, 2 = Right Wall
            if (dir == 1)
            {
                if (Lhit.collider != null)
                {
                    Debug.Log("hit.point.x - position.x = " + Mathf.Abs(Lhit.point.x - position.x));
                    return Mathf.Abs(Lhit.point.x - position.x);
                }
            }
            else if (dir == 2)
            {
                if (Rhit.collider != null)
                {
                    Debug.Log("hit.point.x - position.x = " + Mathf.Abs(Rhit.point.x - position.x));
                    return Mathf.Abs(Rhit.point.x - position.x);
                }
            }
            return -1;
        }
    }
}
