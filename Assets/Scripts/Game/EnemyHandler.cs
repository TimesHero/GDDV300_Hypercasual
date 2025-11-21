using System.Collections;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public Rigidbody2D myRB;
    public int scoreValue; 
    public bool applyPoison;
    public Animator animator;
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
    public void Die()
    {
        Destroy(gameObject);
    }
    public void Leave()
    {
         animator.enabled = true;
    }

}
