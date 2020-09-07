using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friendship
{ 
    public class GA_tablephone_WholeDown : MonoBehaviour
    {
        [Header("TeleWhole")]
        public GameObject item;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        public void StartMovingDown()
        {
            StartCoroutine(WaitforXsec(2f));
        }

        IEnumerator WaitforXsec(float t)
        {
            yield return new WaitForSeconds(t);
            item.GetComponent<Animator>().SetTrigger("MoveDown");
        }
    }
}
