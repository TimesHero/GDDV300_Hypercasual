using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class BossMovementScript : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float speedIncrease = 0.5f;
    public float burstDuration = 0.3f;
    public float pauseDuration = 0.5f;
    public int maxHP = 5;
    private int currentHP;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private bool isBursting = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        StartCoroutine(MoveRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            moveDirection = Random.insideUnitCircle.normalized;
            isBursting = true;
            yield return new WaitForSeconds(burstDuration);
            isBursting = false;
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(pauseDuration);
        }
    }
    void FixedUpdate()
    {
        if (isBursting)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Border"))
        {
            moveDirection = Vector2.Reflect(moveDirection, collision.contacts[0].normal).normalized;
        }
    }
    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Die();
        }
        else
        {
            moveSpeed += speedIncrease;
        }
    }

    void Die()
    {
        Debug.Log("Boss defeated!");
        Destroy(gameObject);
    }

}
