using System.Collections;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public Rigidbody2D myRB;
    public int scoreValue; 
    public bool applyPoison;
    public Animator animator;
    public bool isGrabbed;
    private SpriteRenderer sr;
    private Vector3 defaultScale;

    void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void SetVelocity(Vector2 targetVelocity)
    {
        myRB = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        myRB.linearVelocity = targetVelocity;

        FlipSprite(targetVelocity);   
        StartCoroutine(KillTimer());
    }

    private void FlipSprite(Vector2 vel)
    {
        if (Mathf.Abs(vel.x) < 0.01f)
            return;
        sr.flipX = vel.x > 0f;
    }


    private IEnumerator KillTimer()
    {
        yield return new WaitForSeconds(10f);
        Leave();
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
