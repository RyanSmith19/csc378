using UnityEngine;

public class Snowball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            // Logic to break the obstacle
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Logic to damage or destroy the enemy
            Destroy(collision.gameObject);
        }

        // Destroy the snowball after collision
        Destroy(gameObject);
    }
}
