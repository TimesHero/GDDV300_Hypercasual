using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
public class BossMovementScript : MonoBehaviour
{
    public Animator animator; 
    public float moveSpeed = 3f;
    public float speedIncrease = 0.5f;
    public float burstDuration = 0.3f;
    public float pauseDuration = 0.5f;
    public int maxHP = 5;
    private int currentHP;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private bool isBursting = false;
    private EnemySpawner enemySpawner;
    public bool isStunned = false;
    public float stunDuration = 1.5f;
    private Slider progress;
    public float startScale = 11f;
    public float endScale = 10f;
    public float duration = 1f;
    private SpriteRenderer spriteRenderer;

    [System.Obsolete]
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        rb = GetComponent<Rigidbody2D>();
        currentHP = maxHP;
        GameObject progressBarObject = GameObject.FindGameObjectWithTag("ProgressBar");
        progress = progressBarObject.GetComponent<Slider>();
        progress.value = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();

        transform.localScale = Vector3.one * startScale;
        Color color = spriteRenderer.color;
        color.a = 0f;
        spriteRenderer.color = color;
        StartCoroutine(Appear());
    }
      private IEnumerator Appear()
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;


            float scale = Mathf.Lerp(startScale, endScale, t);
            transform.localScale = Vector3.one * scale;

            Color color = spriteRenderer.color;
            color.a = Mathf.Lerp(0f, 1f, t);
            spriteRenderer.color = color;

            yield return null;
        }

        transform.localScale = Vector3.one * endScale;
        Color finalColor = spriteRenderer.color;
        finalColor.a = 1f;
        spriteRenderer.color = finalColor;
        StartCoroutine(MoveRoutine());
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
        if (isStunned) return; 

        currentHP -= amount;
        Debug.Log(currentHP);
        progress.value = currentHP;
        if (currentHP <= 0)
        {
            Die();
            return;
        }
        moveSpeed += speedIncrease;
        StartCoroutine(StunRoutine());
    }
    private IEnumerator StunRoutine()
    {
        isStunned = true;
        yield return new WaitForSeconds(stunDuration);
        isStunned = false;
    }



    void Die()
    {
        Debug.Log("Boss defeated!");
        enemySpawner.DefeatedBoss();
        Destroy(gameObject);
    }

}
