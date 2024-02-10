using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //for moving between scenes
using UnityEditor; //allows SeralizedObject & SerializedProperty
using UnityEngine.UI; //allows Image - for editing UI

public class StartMenuScript : MonoBehaviour
{
    [SerializeField] GameObject ConfirmationWindow;
    
    public GameObject SettingsMenu;
    public TMPro.TextMeshProUGUI dashText;
    public GameObject dashButton;
    private bool awaitingDashInput = false;
    private bool validDashKey = true;
    public TMPro.TextMeshProUGUI pauseText;
    public GameObject pauseButton;
    private bool awaitingPauseInput = false;
    private bool validPauseKey = true;
    private string pauseKey = "Escape";
    public GameObject backButton;
    
    #region Start Menu (and confirmation window)
    // Start is called before the first frame update
    public void Start() //used to fix this bug bug where the settings icon is not visible
    {
        Canvas.ForceUpdateCanvases(); //force the canvas to update
    }

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
    #endregion

    #region Settings Menu
    public void KeyRebind(string name, string Pkey)
    {
        SerializedObject inputManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0]);
        SerializedProperty axesProperty = inputManager.FindProperty("m_Axes");

        // Iterate through each input setting
        for (int i = 0; i < axesProperty.arraySize; i++) //iterate through each axes/input setting
        {
            SerializedProperty axis = axesProperty.GetArrayElementAtIndex(i); //stores the current 'i' input setting
            SerializedProperty axisName = axis.FindPropertyRelative("m_Name"); //stores the name of the current input setting

            if (axisName.stringValue == "Dash" && name == "Dash") //only changes "Dash" input
            {
                if (Pkey != "Mouse0" && Pkey != pauseText.text) //prevents bugs
                {
                    SerializedProperty keyToPress = axis.FindPropertyRelative("positiveButton");
                    if(Pkey == "LeftShift") //fixes bug
                    {
                        Pkey = "left shift";
                    }
                    keyToPress.stringValue = Pkey.ToLower(); //sets the new input key - must be lowercase
                }
                else
                {
                    validDashKey = false;
                }
            }
            else if (name == "Pause")
            {
                if (Pkey != "Mouse0" && Pkey != dashText.text) //prevents bugs
                {
                    pauseKey = Pkey;
                }
                else
                {
                    validPauseKey = false;
                }
            }
        }

        // Apply the modifications
        inputManager.ApplyModifiedProperties();
    }

    // Update is called once per frame
    private void Update()
    {
        if (awaitingDashInput)
        {
            foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode))) //Search through all possible keys that can be pressed
            {
                if (Input.GetKeyDown(keycode)) //if the user presses a key and the pause menu is visible
                {
                    string keyPressed = keycode.ToString(); //Convert the pressed key to a string
                    KeyRebind("Dash", keyPressed);
                    if (validDashKey) //if the user presses a key that is not the mouse button or the pause key
                    {
                        dashText.text = keyPressed; //update the button text with the new key
                        awaitingDashInput = false;
                        dashButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255); //change colour of button back
                        backButton.SetActive(true); //reactivate back button
                        break; //break the loop
                    }
                    else //restart the process
                    {
                        validDashKey = true;
                    }
                }
            }
        }

        if (awaitingPauseInput)
        {
            foreach (KeyCode keycode in System.Enum.GetValues(typeof(KeyCode))) //Search through all possible keys that can be pressed
            {
                if (Input.GetKeyDown(keycode))
                {
                    string keyPressed = keycode.ToString(); //Convert the pressed key to a string
                    KeyRebind("Pause", keyPressed);
                    if (validPauseKey) //if the user presses a key that is not the mouse button or the dash key
                    {
                        pauseText.text = keyPressed; //update the button text with the new key
                        awaitingPauseInput = false;
                        pauseButton.GetComponent<Image>().color = new Color32(255, 255, 255, 255); //change colour of button back
                        backButton.SetActive(true); //reactivate back button
                        break; //break the loop
                    }
                    else //restart the process
                    {
                        validPauseKey = true;
                    }
                }
            }
        }
    }

    public void OnDashClick() //when the button is clicked
    {
        dashText.text = "...";
        awaitingDashInput = true;
        dashButton.GetComponent<Image>().color = new Color32(212, 0, 0, 255); //change the button's colour to red
        backButton.SetActive(false); //deactivate back button
    }

    public void OnPauseClick() //when the button is clicked
    {
        pauseText.text = "...";
        awaitingPauseInput = true;
        pauseButton.GetComponent<Image>().color = new Color32(212, 0, 0, 255); //change the button's colour to red
        backButton.SetActive(false); //deactivate back button
    }

    public void OpenSettingsMenu() //when the 'Settings' button is pressed
    {
        SettingsMenu.SetActive(true); //show the settings menu
    }

    public void CloseSettingsMenu() //when the 'Back' button is pressed
    {
        SettingsMenu.SetActive(false); //hide the settings menu
    }
    #endregion
}