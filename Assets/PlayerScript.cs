using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float move;
    public float speed;
    bool facingRight = true;

    public float jumpPower;
    bool IsGrounded;
    public float fallMultiplier;
    private float newGravity;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Movement
        //basic movement
        move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(speed * move, rb.velocity.y);
            
        if ((facingRight && move < 0) || (!facingRight && move > 0)) //(move is -1 if going left, 0 if still and 1 if going right)
        {
            flip();
        }

        void flip()
        {
            facingRight = !facingRight; //toggle the facingRight boolean (true/false)
            Vector3 scale = transform.localScale; //Get current scale of character
            scale.x *= -1; // Flip the character's sprite by changing the scale
            transform.localScale = scale; //set new scale to character
        }

        //Jumping
        //basic jump
        if (Input.GetButtonDown("Jump") && IsGrounded)
            {
                rb.AddForce(new Vector2(rb.velocity.x, jumpPower));
            }

            //faster fall
            //if (rb.velocity.y < 0)
            //{
                //rb.gravityScale = newGravity * fallMultiplier;
            //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
}
