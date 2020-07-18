using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{
    public class PlayerCombatTrigger : MonoBehaviour
    {
        public delegate void CombatTriggerEvent(Collider2D collider); //We create the new delegate
        public CombatTriggerEvent OnHit; //Create OnHit Event

        //If collider triggered
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnHit(other); //Call event
        }
    }
}
