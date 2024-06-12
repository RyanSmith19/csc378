using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject snowballPrefab; // Reference to the snowball prefab
    [SerializeField] private Transform throwPoint; // Point from where the snowball will be thrown
    [SerializeField] private float throwForce = 10f; // Force with which the snowball will be thrown
    [SerializeField] private float throwAngle = 45f; // Angle at which the snowball will be thrown
    [SerializeField] private float throwCooldown = 2f; // Cooldown time between throws
    [SerializeField] private float maxSnowballVelocity = 20f; // Maximum velocity for the snowball
    [SerializeField] private Transform player; // Reference to the player

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private GameObject objectToDestroy1; // First object to destroy
    [SerializeField] private GameObject objectToDestroy2; // Second object to destroy

    private bool canThrow = true;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        gameObject.tag = "Enemy"; // Ensure the enemy has the "Enemy" tag
    }

    void Update()
    {
        if (canThrow)
        {
            ThrowSnowball();
        }
    }

    private void ThrowSnowball()
    {
        float[] angles = { throwAngle - 15f, throwAngle, throwAngle + 15f }; // Three different angles
        GameObject[] snowballs = new GameObject[angles.Length];

        for (int i = 0; i < angles.Length; i++)
        {
            // Instantiate the snowball at the throw point
            snowballs[i] = Instantiate(snowballPrefab, throwPoint.position, throwPoint.rotation);

            // Get the Rigidbody2D component of the snowball
            Rigidbody2D snowballRb = snowballs[i].GetComponent<Rigidbody2D>();

            // Calculate the direction to the player
            Vector2 directionToPlayer = (player.position - throwPoint.position).normalized;
            Vector2 throwDirection = new Vector2(directionToPlayer.x * Mathf.Cos(angles[i] * Mathf.Deg2Rad), Mathf.Sin(angles[i] * Mathf.Deg2Rad));
            Vector2 throwVelocity = throwDirection * throwForce;

            // Clamp the final velocity to the maximum allowed velocity
            throwVelocity = Vector2.ClampMagnitude(throwVelocity, maxSnowballVelocity);

            // Apply the clamped velocity to the snowball
            snowballRb.velocity = throwVelocity;

            // Ignore collision between the snowball and the enemy
            Collider2D snowballCollider = snowballs[i].GetComponent<Collider2D>();
            Collider2D enemyCollider = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(snowballCollider, enemyCollider);
        }

        // Ignore collisions between the snowballs
        for (int i = 0; i < snowballs.Length; i++)
        {
            Collider2D colliderA = snowballs[i].GetComponent<Collider2D>();
            for (int j = i + 1; j < snowballs.Length; j++)
            {
                Collider2D colliderB = snowballs[j].GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(colliderA, colliderB);
            }
        }

        // Start the cooldown coroutine
        StartCoroutine(ThrowCooldown());
    }

    private IEnumerator ThrowCooldown()
    {
        canThrow = false;
        yield return new WaitForSeconds(throwCooldown);
        canThrow = true;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Logic for enemy death
        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
        }

        if (objectToDestroy1 != null)
        {
            Destroy(objectToDestroy1);
        }

        if (objectToDestroy2 != null)
        {
            Destroy(objectToDestroy2);
        }

        Destroy(gameObject);
    }
}