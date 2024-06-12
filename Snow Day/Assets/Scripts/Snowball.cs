using UnityEngine;

public class Snowball : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DamagingObject"))
        {
            // Logic to break the obstacle
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // Logic to damage the enemy
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(10); // Adjust the damage value as needed
            }
        }

        // Destroy the snowball after collision
        Destroy(gameObject);
    }
}
