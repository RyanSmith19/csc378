using UnityEngine;

public class EnemySnowball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Logic to break the obstacle
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // Logic to damage the player
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(30); // Adjust the damage value as needed
            }
        }

        // Destroy the snowball after collision
        Destroy(gameObject);
    }
}
