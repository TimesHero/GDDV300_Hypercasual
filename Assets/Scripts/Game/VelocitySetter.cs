using System.Collections;
using UnityEngine;

    public class VelocitySetter : MonoBehaviour
    {
        public Rigidbody2D myRB;
        public void SetVelocity(Vector2 targetVelocity)
        {
            myRB = GetComponent<Rigidbody2D>();
            myRB.linearVelocity = targetVelocity;
            StartCoroutine(KillTimer());
        }
    
        private IEnumerator KillTimer()
        {
            yield return new WaitForSeconds(10f);
            Destroy(gameObject);
        }
    
    }
