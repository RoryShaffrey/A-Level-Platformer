using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private float move;
    public float speed;
    bool facingRight = true;

    [SerializeField] float jumpPower = 7f;
    private bool IsGrounded;
    private float gravity;
    [SerializeField] private float fallMultiplier = 1.2f;
    [SerializeField] private float hangTimeMultiplier = 0.25f;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        gravity = Physics2D.gravity.magnitude;
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
            Vector2 scale = transform.localScale; //Get current scale of character
            scale.x *= -1; // Flip the character's sprite by changing the scale
            transform.localScale = scale; //set new scale to character
        }

        //Jumping
        //basic jump
        if (Input.GetButtonDown("Jump") && IsGrounded)
            {
            rb.velocity = jumpPower * Vector2.up;
            }

        //faster fall
        if (rb.velocity.y > -2)
        {
            rb.gravityScale = 2f;
        }
        else if (rb.velocity.y < 5 && rb.velocity.y > -2)
        {
            rb.gravityScale *= hangTimeMultiplier;
        }
        else
        {
            rb.gravityScale *= fallMultiplier;
        }

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
