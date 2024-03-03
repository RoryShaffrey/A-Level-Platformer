using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; //allows SeralizedObject & SerializedProperty
using UnityEngine.UI; //allows Image - for editing UI

public class PauseMenuScript : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseMenu;
    [SerializeField] GameObject ConfirmationWindow;
    [SerializeField] GameObject SettingsMenu;
    private string pauseKey = "Escape";

    public TMPro.TextMeshProUGUI dashText;
    public GameObject dashButton;
    private bool awaitingDashInput = false;
    private bool validDashKey = true;
    public TMPro.TextMeshProUGUI pauseText;
    public GameObject pauseButton;
    private bool awaitingPauseInput = false;
    private bool validPauseKey = true;
    public GameObject backButton;

    private bool pauseVisible = true;

    string[] inputtedKeys = {"LeftShift", "LeftControl", "RightShift", "RightControl", "LeftAlt", "RightAlt", "Mouse1"}; //string keys that are pressed by the user
    string[] recognisedKeys = {"left shift", "left ctrl", "right shift", "right ctrl", "left alt", "right alt", "mouse 1"}; //translated keys that are recognised by the input manager

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
                    for (int j = 0; j < inputtedKeys.Length; j++) //iterate through all possible 'buggy' keys
                    {
                        if (Pkey == inputtedKeys[j]) //if the inputted key is the same as a key in the array
                        {
                            Pkey = recognisedKeys[j]; //translate the inputted key to a key the input manager can recognise
                        }
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

    public void OnDashClick() //when the dash button is clicked
    {
        dashText.text = "...";
        awaitingDashInput = true;
        dashButton.GetComponent<Image>().color = new Color32(212, 0, 0, 255); //change the button's colour to red
        backButton.SetActive(false); //deactivate back button
    }

    public void OnPauseClick() //when the pause button is clicked
    {
        pauseText.text = "...";
        awaitingPauseInput = true;
        pauseButton.GetComponent<Image>().color = new Color32(212, 0, 0, 255); //change the button's colour to red
        backButton.SetActive(false); //deactivate back button
    }

    public void OpenSettingsMenu() //when the 'Settings' button is pressed
    {
        SettingsMenu.SetActive(true); //show the settings menu
        pauseVisible = false;
    }

    public void CloseSettingsMenu() //when the 'Back' button is pressed
    {
        SettingsMenu.SetActive(false); //hide the settings menu
        pauseVisible = true;
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        //if (loggedIn) {get player prefs}
        if (PlayerPrefs.GetString("dashKey") == "") //if no dash key has been previously set
        {
            PlayerPrefs.SetString("dashKey", "LeftShift"); //set dash text to "LeftShift"
        }
        dashText.text = PlayerPrefs.GetString("dashKey"); //set dash text to saved key
        if (PlayerPrefs.GetString("pauseKey") == "") //if no pause key has been previously set
        {
            PlayerPrefs.SetString("pauseKey", "Escape"); //set pause text to "Escape"
        }
        pauseText.text = PlayerPrefs.GetString("pauseKey"); //set pause text to saved key
    }

    // Update is called once per frame
    private void Update()
    {
        KeyCode pauseKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), pauseKey); //translate pauseKey to a keycode
        if (Input.GetKeyDown(pauseKeyCode) && pauseVisible) //if the pause key is pressed
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
                        PlayerPrefs.SetString("dashKey", keyPressed); //save the new key to PlayerPrefs
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
                        PlayerPrefs.SetString("pauseKey", keyPressed); //save the new key to PlayerPrefs
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

    #region Pause Menu (and confirmation window)
    void Pause() //when the escape key is pressed
    {
        pauseMenu.SetActive(true); //makes the pause menu appear
        Time.timeScale = 0f; //pause time in the game
        paused = true; //set the variable 'paused' to true
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Quit() //when the 'Yes' button is pressed
    {
        PlayerPrefs.DeleteAll(); //deletes all player preferences
        Application.Quit(); //quit the game
    }

    public void OpenConfirmationWindow() //when the 'Quit' button is pressed
    {
        ConfirmationWindow.SetActive(true); //show the confirmation window
        pauseVisible = false;
    }
    public void CloseConfirmationWindow() //when the 'Cancel' button is pressed
    {
        ConfirmationWindow.SetActive(false); //hide the confirmation window
        pauseVisible = true;
    }
    #endregion
}