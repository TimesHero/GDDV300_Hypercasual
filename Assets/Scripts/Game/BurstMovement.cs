using System.Collections;
using UnityEngine;

public class BurstVelocitySetter : MonoBehaviour
{
    public Rigidbody2D myRB;
    public float burstSpeed = 10f; 
    public float burstIntervalMin = 0.5f;  
    public float burstIntervalMax = 1.5f; 
    public float burstDuration = 0.2f;

    private EnemyHandler velocitySetter; 
    private bool firstBurstDone = false;
    private Vector2 initialDirection;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        velocitySetter = GetComponent<EnemyHandler>();
        initialDirection = transform.position.x > 0 ? Vector2.left : Vector2.right;

        if (velocitySetter != null)
        {
            StartCoroutine(BurstMovement());
        }
    }

    private IEnumerator BurstMovement()
    {
        while (true) 
        {
            float waitTime = Random.Range(burstIntervalMin, burstIntervalMax);
            yield return new WaitForSeconds(waitTime);

            Vector2 burstDirection;

            if (!firstBurstDone)
            {
                burstDirection = initialDirection;
                firstBurstDone = true;
            }
            else
            {
                burstDirection = Random.value > 0.5f ? Vector2.right : Vector2.left;
            }

            velocitySetter.SetVelocity(burstDirection * burstSpeed);
            yield return new WaitForSeconds(burstDuration);
            velocitySetter.SetVelocity(Vector2.zero);
        }
    }
}
