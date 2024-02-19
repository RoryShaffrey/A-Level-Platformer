using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadScript : MonoBehaviour
{
    [SerializeField] private float bounceForce = 50f;
    public Animator bouncePadAnimation;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse); //add a vertical force of 50 to the player
            bouncePadAnimation.Play("BouncePadJump", 0, 0f); //triggers the bouncepad animation from the first frame
        }
    }
}