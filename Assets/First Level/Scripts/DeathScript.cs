using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    public GameObject Player;
    public GameObject SpawnPoint;

    private void OnCollisionEnter2D(Collision2D collision) //If something collides with it
    {
        if (collision.gameObject.CompareTag("Player")) //If it collides with something with the tag 'Player' (the player)
        {
            Player.transform.position = SpawnPoint.transform.position; //Reset the player's position to the spawnpoint's position
        }
    }
}