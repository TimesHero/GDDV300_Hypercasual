using System.Collections;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public Rigidbody2D myRB;
    public int scoreValue; 
    public bool applyPoison;
    public Animator animator;
    public bool isGrabbed;
    public void SetVelocity(Vector2 targetVelocity)
    {
        myRB = GetComponent<Rigidbody2D>();
        myRB.linearVelocity = targetVelocity;
        StartCoroutine(KillTimer());
    }
    void Update()
    {
        if (isGrabbed)
        {
           transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
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
        animator.SetBool("Die", true);
    }

}
