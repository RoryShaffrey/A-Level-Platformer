using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleScript : MonoBehaviour
{
    public Animator appleAnimation;
    [SerializeField] GameObject apple;
    private void OnTriggerEnter2D(Collider2D collider) //If something collides with it
    {
        if (collider.gameObject.CompareTag("Player")) //If it collides with something with the tag 'Player' (the player)
        {
            PlayerScript player = collider.GetComponent<PlayerScript>(); //references the PlayerScript
            if (player.jumpsRemaining != 2)
            {
                player.jumpsRemaining += 1;
                player.maxJumpTimeCopy += 0.07f; //add an extra boost on top of the 0.08 second boost
                if (player.maxJumpTimeCopy > 0.3f) //to cap the max jump time at 0.3 seconds
                {
                    player.maxJumpTimeCopy = 0.3f;
                }
            }
            StartCoroutine(GainJump()); //calls the function LoadScene
        }
    }

    private IEnumerator GainJump()
    {
        appleAnimation.SetTrigger("Collect"); //turn to collected apple sprite
        apple.GetComponent<BoxCollider2D>().enabled = false; //turn off the collider
        yield return new WaitForSeconds(3f); //waits three seconds before respawning the apple
        appleAnimation.SetTrigger("Idle"); //turn back to apple
        apple.GetComponent<BoxCollider2D>().enabled = true; //turn on the collider
    }
}
