using System.Collections;
using UnityEngine;

public class TongueCollider2D : MonoBehaviour
{
    public PlayerController playerController;

    [Header("Critical Hit Minigame")]
    public float targetSpeed = 3f;
    private BossMovementScript currentBoss;
    private GameObject criticalBar;
    private Transform startPoint;
    private Transform endPoint;
    private Transform movingTarget;
    private Transform criticalPoint;

    private bool minigameActive = false;
    private bool movingRight = true;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (minigameActive) return; 

        if (other.CompareTag("Enemy"))
        {
            playerController?.OnTongueHit2D(other);
            return;
        }

        if (other.CompareTag("Boss"))
        {
            BossMovementScript boss = other.GetComponent<BossMovementScript>();

            if (boss != null && !boss.isStunned) 
            {
                SetupMinigame(boss);
            }
        }

    }
    private void SetupMinigame(BossMovementScript boss)
    {
        currentBoss = boss;

        Time.timeScale = 0f;

        criticalBar = boss.transform.Find("CriticalHitBar").gameObject;
        startPoint = criticalBar.transform.Find("StartPoint");
        endPoint = criticalBar.transform.Find("EndPoint");
        movingTarget = criticalBar.transform.Find("MovingTarget");
        criticalPoint = criticalBar.transform.Find("CriticalPoint");

        criticalBar.SetActive(true);
        movingTarget.position = startPoint.position;
        movingRight = true;

        minigameActive = true;
        StartCoroutine(CriticalHitRoutine());
    }
    private IEnumerator CriticalHitRoutine()
    {
        while (minigameActive)
        {

            MoveTarget();


            if (Input.GetMouseButtonDown(0))
            {
                bool success = CheckAlignment();
                EndMinigame(success);
            }

            yield return null; 
        }
    }
    private void MoveTarget()
    {
        float delta = targetSpeed * Time.unscaledDeltaTime;

        if (movingRight)
        {
            movingTarget.position = Vector3.MoveTowards(
                movingTarget.position,
                endPoint.position,
                delta
            );

            if (Vector3.Distance(movingTarget.position, endPoint.position) < 0.01f)
                movingRight = false;
        }
        else
        {
            movingTarget.position = Vector3.MoveTowards(
                movingTarget.position,
                startPoint.position,
                delta
            );

            if (Vector3.Distance(movingTarget.position, startPoint.position) < 0.01f)
                movingRight = true;
        }
    }


    private bool CheckAlignment()
    {
        float dist = Vector3.Distance(movingTarget.position, criticalPoint.position);
        return dist < 0.3f; 
    }
    private void EndMinigame(bool success)
    {
        minigameActive = false;
        criticalBar.SetActive(false);
        Time.timeScale = 1f;

        if (success)
        {
            currentBoss.TakeDamage(500); 
        }
        else
        {
            currentBoss.TakeDamage(250); 
        }
        playerController.bossHit = true;
    }
}
