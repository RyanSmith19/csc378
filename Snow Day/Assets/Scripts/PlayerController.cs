using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float moveSpeed = 12f;
    private float jumpingPower = 30f;
    private float onGroundValue = 0.2f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private AudioSource jumpSound; 
    [SerializeField] private Sprite[] walkSprites;
    [SerializeField] private float spriteSize = 1f;

    // Position and size of the ground check box
    private Vector2 groundCheckBoxPosition;
    private Vector2 groundCheckBoxSize = new Vector2(0.6f, 0.2f);

    public Sprite idleSprite; // Original sprite
    private int currentSpriteIndex = 0; // Index of the current sprite
    private float spriteChangeThreshold = 1f; // Threshold for changing sprites
    private float distanceSinceLastSpriteChange = 0f; // Distance since last sprite change

    private SpriteRenderer spriteRenderer;

    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [SerializeField] private HealthBar healthBar;

    [SerializeField] private float healthRegenRate = 1f; // Health points per second

    [SerializeField] private GameObject snowballPrefab; // Reference to the snowball prefab
    [SerializeField] private Transform throwPoint; // Point from where the snowball will be thrown
    [SerializeField] private float throwForce = 10f; // Force with which the snowball will be thrown
    [SerializeField] private float throwAngle = 45f; // Angle at which the snowball will be thrown
    [SerializeField] private float throwCooldown = 1f; // Cooldown time between throws
    [SerializeField] private float maxSnowballVelocity = 20f; // Maximum velocity for the snowball

    private bool canThrow = true;

    private Collider2D playerCollider; // Reference to the player's collider

    private Respawn respawn;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite; // Set the original sprite

        // Set the size of the sprite renderer
        spriteRenderer.transform.localScale = new Vector3(spriteSize, spriteSize, 1f);

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        StartCoroutine(RegenerateHealth());

        playerCollider = GetComponent<Collider2D>(); // Get the player's collider

        respawn = FindObjectOfType<Respawn>();
    }

    void Update()
    {
        // GetKey checks if the specified key is being held down
        horizontal = Input.GetAxis("Horizontal");

        // Move the player horizontally
        MovePlayer(horizontal);

        // Jump if the player is grounded and the jump button is pressed
        if (Input.GetButtonDown("Jump") && IsOnGround())
        {
            Jump();
        }

        if (horizontal != 0)
        {
            // Update sprite based on horizontal movement
            distanceSinceLastSpriteChange += Mathf.Abs(horizontal) * moveSpeed * Time.deltaTime;
            if (distanceSinceLastSpriteChange > spriteChangeThreshold)
            {
                ChangeSprite();
                distanceSinceLastSpriteChange = 0f;
            }
        }
        else
        {
            // Change back to the original sprite when not moving
            spriteRenderer.sprite = idleSprite;
        }

        // Check if the throw button is pressed and the player can throw
        if (Input.GetButtonDown("Fire1") && canThrow)
        {
            ThrowSnowball();
        }
    }

    private void ChangeSprite()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % walkSprites.Length;
        spriteRenderer.sprite = walkSprites[currentSpriteIndex];
    }

    private bool IsOnGround()
    {
        // if the velocity is less than the value then player can jump
        return Mathf.Abs(rb.velocity.y) <= onGroundValue;
    }
    
    private bool IsGrounded()
    {
        groundCheckBoxPosition = new Vector2(transform.position.x, transform.position.y - 0.5f);

        // Check if the ground check box overlaps with the ground layer
        RaycastHit2D hit = Physics2D.BoxCast(groundCheckBoxPosition, groundCheckBoxSize, 0f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    // Function to move the player horizontally
    void MovePlayer(float direction)
    {
        // Calculate the movement amount based on the moveSpeed
        float moveAmount = direction * moveSpeed * Time.deltaTime;

        // Move the player's position horizontally
        transform.Translate(Vector3.right * moveAmount);

        // Flip the player's sprite based on the movement direction
        if (direction > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // Function to make the player jump
    void Jump()
    {
        // Apply an upward force to the rigidbody to make the player jump
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

        // Play the jump sound
        jumpSound.Play();
    }

    // Function to flip the player's sprite
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("DamagingObject"))
        {
            Rigidbody2D fallingObjectRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (fallingObjectRb != null)
            {
                float impactVelocity = fallingObjectRb.velocity.magnitude;
                if (impactVelocity > 3f) // Adjust the threshold as needed
                {
                    TakeDamage(10);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        currentHealth = maxHealth;
        healthBar.SetHealth(currentHealth);

        if (respawn != null)
        {
            transform.position = respawn.GetCurrentCheckpoint();
        }
        else
        {
            Debug.LogWarning("Respawn component not found!");
        }
    }

    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            if (currentHealth < maxHealth)
            {
                currentHealth += Mathf.RoundToInt(healthRegenRate);
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
                healthBar.SetHealth(currentHealth);
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private void ThrowSnowball()
    {
        // Instantiate the snowball at the throw point
        GameObject snowball = Instantiate(snowballPrefab, throwPoint.position, throwPoint.rotation);

        // Get the Rigidbody2D component of the snowball
        Rigidbody2D snowballRb = snowball.GetComponent<Rigidbody2D>();

        // Determine the throw direction based on the player's facing direction
        float direction = isFacingRight ? 1f : -1f;
        Vector2 throwDirection = new Vector2(direction * Mathf.Cos(throwAngle * Mathf.Deg2Rad), Mathf.Sin(throwAngle * Mathf.Deg2Rad));
        Vector2 throwVelocity = throwDirection * throwForce;

        // Set the snowball's velocity to the player's current velocity plus the throw velocity
        Vector2 finalVelocity = rb.velocity + throwVelocity;

        // Clamp the final velocity to the maximum allowed velocity
        finalVelocity = Vector2.ClampMagnitude(finalVelocity, maxSnowballVelocity);

        // Apply the clamped velocity to the snowball
        snowballRb.velocity = finalVelocity;

        // Ignore collision between the snowball and the player
        Physics2D.IgnoreCollision(playerCollider, snowball.GetComponent<Collider2D>());

        // Start the cooldown coroutine
        StartCoroutine(ThrowCooldown());
    }

    private IEnumerator ThrowCooldown()
    {
        canThrow = false;
        yield return new WaitForSeconds(throwCooldown);
        canThrow = true;
    }
}