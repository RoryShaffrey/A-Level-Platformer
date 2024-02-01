using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    public Animator bananaAnimation;
    [SerializeField] GameObject banana;
    private void OnTriggerEnter2D(Collider2D collider) //If something collides with it
    {
        if (collider.gameObject.CompareTag("Player")) //If it collides with something with the tag 'Player' (the player)
        {
            PlayerScript player = collider.GetComponent<PlayerScript>(); //references the PlayerScript
            player.canDash = true;
            StartCoroutine(canDash()); //calls the function LoadScene
        }
    }

    private IEnumerator canDash()
    {
        bananaAnimation.SetTrigger("Collect"); //turn to collected banana sprite
        banana.GetComponent<BoxCollider2D>().enabled = false; //turn off the collider
        yield return new WaitForSeconds(3f); //waits three seconds before respawning the banana
        bananaAnimation.SetTrigger("Idle"); //turn back to banana
        banana.GetComponent<BoxCollider2D>().enabled = true; //turn on the collider
    }
}
