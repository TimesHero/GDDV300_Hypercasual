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
    }
}
