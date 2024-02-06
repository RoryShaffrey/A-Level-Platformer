using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    public GameObject SpawnPoint;
    public GameObject CheckPoint;
    private bool IsTriggered = false; //checks if this checkpoint is triggered

    private void OnTriggerEnter2D(Collider2D collider) //If something collides with it
    {
        if (!IsTriggered)
        {
            if (collider.gameObject.CompareTag("Player")) //If it collides with something with the tag 'Player' (the player)
            {
                SpawnPoint.transform.position = new Vector2(CheckPoint.transform.position.x, CheckPoint.transform.position.y + 0.8f); //Change the spawnpoint's position to this checkpoint's position
                IsTriggered = true; //prevents the checkpoint from being retriggered later (validation/future-proofing)
                CheckPoint.GetComponent<SpriteRenderer>().color = new Color(0, 191/255f, 11/255f); //make the checkpoint green
            }
        }
    }
}
