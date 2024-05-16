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

    // Position and size of the ground check box
    private Vector2 groundCheckBoxPosition;
    private Vector2 groundCheckBoxSize = new Vector2(0.6f, 0.2f);

    public Sprite idleSprite; // Original sprite
    private int currentSpriteIndex = 0; // Index of the current sprite
    private float spriteChangeThreshold = 0.1f; // Threshold for changing sprites
    private float distanceSinceLastSpriteChange = 0f; // Distance since last sprite change

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = idleSprite; // Set the original sprite
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

    }

    private void ChangeSprite()
    {
        currentSpriteIndex = (currentSpriteIndex + 1) % walkSprites.Length;
        spriteRenderer.sprite = walkSprites[currentSpriteIndex];
    }

    private bool IsOnGround(){
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

    // Function to make the player jum
    void Jump()
    {
        // Apply an upward force to the rigidbody to make the player jump
        rb.velocity = new Vector2(rb.velocity.x, jumpingPower);

        // Play the jump sound
        //jumpSound.Play();
    }

    // Function to flip the player's sprite
    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
