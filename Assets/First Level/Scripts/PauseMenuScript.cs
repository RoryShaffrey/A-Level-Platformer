using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{

    public static bool paused = false;
    public GameObject pauseMenu;
    [SerializeField] GameObject ConfirmationWindow;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //if the escape key is pressed
        {
            if (paused) //if paused is true
            {
                Resume(); //call the "Resume" function
            }
            else //if paused is false
            {
                Pause(); //call the "Pause" function
            }
        }
    }
    void Pause() //when the escape key is pressed
    {
        Time.timeScale = 0f; //pause time in the game
        pauseMenu.SetActive(true); //makes the pause menu appear
        paused = true; //set the variable 'paused' to true
    }
    public void Resume() //called either when the escape button is pressed again or the resume button is pressed
    {
        //direct opposite of Pause()
        Time.timeScale = 1f; //set time to normal
        pauseMenu.SetActive(false); //make the pause menu disappear
        paused = false; //set the variable 'paused' to false
    }

    public void Quit() //when the 'Quit' button is pressed
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
