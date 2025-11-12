using System.Collections;
using UnityEngine;

public class TongueCollider2D : MonoBehaviour
{
    public PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            playerController?.OnTongueHit2D(other);
        }
        if (other.CompareTag("Boss"))
        {
        BossMovementScript boss = other.GetComponent<BossMovementScript>();
        if (boss != null)
        {
                boss.TakeDamage(1);
                playerController.bossHit = true;
        }
        }
    }
}
