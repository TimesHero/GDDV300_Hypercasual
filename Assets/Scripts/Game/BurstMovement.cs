using System.Collections;
using UnityEngine;

public class BurstVelocitySetter : MonoBehaviour
{
    public Rigidbody2D myRB;
    public float burstSpeed = 10f; 
    public float burstIntervalMin = 0.5f;  
    public float burstIntervalMax = 1.5f; 
    public float burstDuration = 0.2f;

    private VelocitySetter velocitySetter; 

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        velocitySetter = GetComponent<VelocitySetter>();
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
            Vector2 burstDirection = Vector2.right; 
            if (Random.value > 0.5f) 
            {
                burstDirection = Vector2.left;  
            }
            velocitySetter.SetVelocity(burstDirection * burstSpeed);
            yield return new WaitForSeconds(burstDuration);
            velocitySetter.SetVelocity(Vector2.zero);
        }
    }
}
