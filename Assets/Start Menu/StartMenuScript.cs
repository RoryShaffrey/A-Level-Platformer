using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //for moving between scenes

public class StartMenuScript : MonoBehaviour
{
    public void Play() //can't use "start" as it is a keyword which will instantly run this code as soon as the game loads
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit() //when the 'Quit' button is pressed
    {
        Application.Quit(); //quits the application
    }
}
