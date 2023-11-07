using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float move;
    public float speed;
    bool facingRight = true;

    [SerializeField] private float jumpPower = 15f;
    private bool IsGrounded; //only jump again if grounded
    [SerializeField] private float fallMultiplier = 1.01f;
    [SerializeField] private float hangTimeMultiplier = 0.98f;
    private bool falling;
    private float maxJumpTimeCopy;
    [SerializeField] private float maxJumpTime;
    private bool IsJumping; //for variable jump height

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        falling = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Movement
        //basic movement
        move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * move, rb.velocity.y); //(move)
            
        if ((facingRight && move < 0) || (!facingRight && move > 0)) //(move is -1 if going left, 0 if still and 1 if going right)
        {
            flip();
        }
        void flip()
        {
            facingRight = !facingRight; //toggle the facingRight boolean (true/false)
            Vector2 scale = transform.localScale; //Get current scale of character
            scale.x *= -1; // Flip the character's sprite by changing the scale
            transform.localScale = scale; //set new scale to character
        }
        #endregion

        #region Jumping
        //basic jump
        if (Input.GetButtonDown("Jump") && IsGrounded) //If a jump key is pressed
            {
            IsJumping = true; //reset IsJumping
            maxJumpTimeCopy = maxJumpTime; //reset maxJumpTime
            rb.velocity = new Vector2(rb.velocity.x, jumpPower); //(jump)
            }

        //variable jump height
        if (Input.GetButton("Jump") && IsJumping == true) //If a jump key is currently being pressed
        {
            if (maxJumpTimeCopy > 0) //if the max time to hold the jump button is greater than 0
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower); //(jump)
                maxJumpTimeCopy -= Time.deltaTime; //reduce timer by the time taken between frames
            }
            else
            {
                IsJumping = false; //Happens when the max time to hold the jump button down is reached
            }
        }
        if (Input.GetButtonUp("Jump")) //If a jump button is released
        {
            IsJumping = false;
        }

        //clamping/limiting max fall speed
        if (rb.velocity.y < -45f) //If the player is falling with a velocity faster than 10
        {
            rb.velocity = new Vector2(rb.velocity.x, -45f); //set the player's velocity to 10
        }
        #endregion
    }

    private void FixedUpdate() //Called every 0.02 seconds rather than once per frame
    {
        //faster fall
        if (falling == false && rb.velocity.y < -2)//if the player is falling with velocity > 2
        {
            rb.gravityScale *= fallMultiplier; //fall faster
            falling = true;
        }
        else if (!IsGrounded && (rb.velocity.y < 2 && rb.velocity.y > -2))//if the player is near the peak of the jump
        {
            rb.gravityScale *= hangTimeMultiplier; //reduce gravity/falling speed reduced - gives the player more control
        }
        else//at the start of the jump
        {
            rb.gravityScale = 8f; //normal gravity
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision) //when colliding with ground
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
            falling = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) //when leaving the ground
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
}