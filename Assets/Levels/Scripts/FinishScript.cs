using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //for moving between scenes

public class FinishScript : MonoBehaviour
{
    public Animator crossfade;
    private float transitionTime = 0.75f; //default value 0.75 seconds
    public StopwatchScript StopwatchScript; //references the StopwatchScript

    private void OnTriggerEnter2D(Collider2D collider) //If something collides with it
    {
        if (collider.gameObject.CompareTag("Player")) //If it collides with something with the tag 'Player' (the player)
        {
            PlayerScript player = collider.GetComponent<PlayerScript>(); //references the PlayerScript
            if (SceneManager.GetActiveScene().buildIndex == 3) //if the current scene is the first level
            {
                StopwatchScript = FindObjectOfType<StopwatchScript>(); //references the StopwatchScript
                StopwatchScript.playing = false; //stops the stopwatch
            }
            StartCoroutine(LoadScene(player)); //calls the function LoadScene
        }
    }

    private IEnumerator LoadScene(PlayerScript player)
    {
        yield return new WaitForSeconds(0.1f); //add a short delay of 0.1 seconds before stopping the character
        player.canMove = false; //stop the character
        crossfade.SetTrigger("Start"); //starts the transition
        yield return new WaitForSeconds(transitionTime); //Waits the transition time (0.75 seconds)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //goes to the next scene (the first level)
        player.canMove = true; //give the user back control of the character
    }
}