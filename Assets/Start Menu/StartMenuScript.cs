using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //for moving between scenes

public class StartMenuScript : MonoBehaviour
{
    [SerializeField] GameObject ConfirmationWindow;

    public void Play() //can't use "start" as it is a keyword which will instantly run this code as soon as the game loads
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //goes to the next scene (the first level)
    }

    public void Quit() //when the 'Confirm' button is pressed
    {
        Application.Quit(); //quits the application
    }

    public void OpenConfirmationWindow() //when the 'Quit' button is pressed
    {
        ConfirmationWindow.SetActive(true); //show the confirmation window
    }
    public void CloseConfirmationWindow() //when the 'Cancel' button is pressed
    {
        ConfirmationWindow.SetActive(false); //hide the confirmation window
    } 
}
