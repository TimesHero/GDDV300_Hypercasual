using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Transform target;        
    public Transform mover;       
    public Transform pointA;        
    public Transform pointB;        

    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public float returnSpeed = 2f;
    public float returnDelay = 0.5f;

    private bool isHolding = false;
    private bool isReturning = false;
    private float t = 0f;      
    private int direction = 1;     

    private Vector3 targetStartPos;
    private Vector3 moverStartPos;
    private GameObject caughtEnemy = null;

    void Start()
    {
        targetStartPos = target.position;
        moverStartPos = mover.position;
    }

    void Update()
    {
        if (isReturning) return; 

        if (Input.GetMouseButtonDown(0))
        {
            isHolding = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isHolding = false;
            mover.position = target.position;
            StartCoroutine(ReturnToStart());
        }

        if (isHolding)
        {
            MoveTarget();
        }
    }

    void MoveTarget()
    {
        t += direction * moveSpeed * Time.deltaTime;
        if (t > 1f)
        {
            t = 1f;
            direction = -1;
        }
        else if (t < 0f)
        {
            t = 0f;
            direction = 1;
        }

        target.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }

    IEnumerator ReturnToStart()
    {
        isReturning = true;
        yield return new WaitForSeconds(returnDelay);

        Vector3 targetInitial = target.position;
        Vector3 moverInitial = mover.position;
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * returnSpeed;
            float lerpT = Mathf.Clamp01(elapsed);

            target.position = Vector3.Lerp(targetInitial, targetStartPos, lerpT);
            mover.position = Vector3.Lerp(moverInitial, moverStartPos, lerpT);

            yield return null;
        }
        if (caughtEnemy != null)
        {
            Destroy(caughtEnemy);
            caughtEnemy = null;
        }
        t = 0f;
        direction = 1;
        isReturning = false;
    }
   public void OnTongueHit2D(Collider2D other)
{
    if (caughtEnemy == null && other.CompareTag("Enemy"))
    {
        caughtEnemy = other.gameObject;
        caughtEnemy.transform.SetParent(mover);
        Rigidbody2D rb = caughtEnemy.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;    
            rb.isKinematic = true;        
        }
        VelocitySetter vs = caughtEnemy.GetComponent<VelocitySetter>();
        if (vs != null)
        {
            vs.enabled = false;
        }
    }
}
}
