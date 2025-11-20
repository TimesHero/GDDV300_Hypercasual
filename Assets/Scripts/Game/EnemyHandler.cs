using System.Collections;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    public Rigidbody2D myRB;
    public int scoreValue;
    public bool applyPoison;
    private Camera mainCamera;
    private bool isOnScreen = true; 
    private Coroutine killTimerCoroutine; 
    private float timer = 10f;

    void Start()
    {
        myRB = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        killTimerCoroutine = StartCoroutine(KillTimer());
    }

    public void SetVelocity(Vector2 targetVelocity)
    {
        myRB.linearVelocity = targetVelocity;
    }

    private IEnumerator KillTimer()
    {
        while (true)
        {
            if (IsOnScreen())
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0f && !IsOnScreen())
            {
                Destroy(gameObject);
                yield break; 
            }

            if (timer <= 0f && IsOnScreen())
            {
                timer = 10f; 
            }

            yield return null;
        }
    }

    private bool IsOnScreen()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        return viewportPosition.x >= 0 && viewportPosition.x <= 1 && viewportPosition.y >= 0 && viewportPosition.y <= 1;
    }
}
