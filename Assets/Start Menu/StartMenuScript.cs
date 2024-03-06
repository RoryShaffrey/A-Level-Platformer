using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //for moving between scenes
using UnityEditor; //allows SeralizedObject & SerializedProperty
using UnityEngine.UI; //allows 'Image'
using TMPro; //allows TextMeshProUGUI
using System.IO; //allows File

public class StartMenuScript : MonoBehaviour
{
    #region variables
    [SerializeField] GameObject ConfirmationWindow;
    [SerializeField] GameObject SettingsMenu;
    [SerializeField] GameObject LoginMenu;
    [SerializeField] GameObject SignUpMenu;
    
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

    string[] inputtedKeys = {"LeftShift", "LeftControl", "RightShift", "RightControl", "LeftAlt", "RightAlt", "Mouse1"}; //string keys that are pressed by the user
    string[] recognisedKeys = {"left shift", "left ctrl", "right shift", "right ctrl", "left alt", "right alt", "mouse 1"}; //translated keys that are recognised by the input manager
    
    public TMP_InputField usernameLoginInput;
    public TMP_InputField passwordLoginInput;
    public TMP_InputField usernameRegisterInput;
    public TMP_InputField passwordRegisterInput;
    public TMPro.TextMeshProUGUI invalidLoginText;
    public TMPro.TextMeshProUGUI invalidRegisterText;
    private string jsonFile = "C:\\Users\\rorys\\Documents\\GitHub\\A-Level-Platformer\\Assets\\Start Menu\\profiles.json";
    private bool isLoggedIn = false;
    #endregion
    
    void Start() 
    {
        PlayerPrefs.SetString("isLoggedIn", "false");
        
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

    #region Start Menu (and confirmation window)
    // Start is called before the first frame update
    public void Play() //can't use "start" as it is a keyword which will instantly run this code as soon as the game loads
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //goes to the next scene (the first level)
    }

    public void Quit() //when the 'Confirm' button is pressed
    {
        PlayerPrefs.DeleteAll(); //deletes all player preferences
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

    public void OnDashClick() //when the button is clicked
    {
        dashText.text = "..."; //replaces text in button with "..."
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

    #region Login Menu
    public void OpenLoginMenu() //when the 'Login' button is pressed
    {
        LoginMenu.SetActive(true); //show the login menu
    }

    public void CloseLoginMenu() //when the 'Back' button is pressed
    {
        LoginMenu.SetActive(false); //hide the login menu
    }

    [System.Serializable]
    public class Profile //represents one profile
    {
        public string username;
        public string password;
        public int highScore;
        public int rating;
        public string comment;
    }

    [System.Serializable]
    public class ProfileList //list of profiles
    {
        public List<Profile> profiles;
    }

    public void Login() //when the 'Submit' button is pressed
    {
        string jsonString = File.ReadAllText(jsonFile); //stores the current JSON file contents
        ProfileList profileList = JsonUtility.FromJson<ProfileList>(jsonString);

        string username = usernameLoginInput.text;
        string password = passwordLoginInput.text;

        foreach (Profile profile in profileList.profiles) //iterates through all profiles
        {
            if (username == profile.username && password == profile.password) //if there is a profile with a matching username and password
            {
                isLoggedIn = true;
                PlayerPrefs.SetString("username", username);
                PlayerPrefs.SetInt("highScore", profile.highScore);
                PlayerPrefs.SetString("isLoggedIn", "true");
                invalidLoginText.color = new Color32(0, 255, 0, 255); //make the text green
                invalidLoginText.text = "Logged in as " + username;
                StartCoroutine(ClearInvalidText());
                break; //break the loop
            }
        }
        if (!isLoggedIn) { //if the user did not enter correct details
            invalidLoginText.color = new Color32(255, 0, 0, 255); //make the text red
            invalidLoginText.text = "Incorrect username or password";
            StartCoroutine(ClearInvalidText());
        }
    }
    #endregion

    #region Sign-Up Menu

    public void OpenSignUpMenu() //when the 'Sign-Up' button is pressed
    {
        SignUpMenu.SetActive(true); //show the sign-up menu
    }

    public void CloseSignUpMenu() //when the 'Back' button is pressed
    {
        SignUpMenu.SetActive(false); //hide the sign-up menu
    }

    public void Register() //when the 'Register' button is pressed
    {
        string jsonString = File.ReadAllText(jsonFile);
        ProfileList profileList = JsonUtility.FromJson<ProfileList>(jsonString);

        string username = usernameRegisterInput.text;
        string password = passwordRegisterInput.text;

        #region validation
        if (username == "" || password == "") //if the username or password is empty
        {
            invalidRegisterText.color = new Color32(255, 0, 0, 255); //make the text red
            invalidRegisterText.text = "Username or password cannot be empty";
            StartCoroutine(ClearInvalidText());
            return;
        }
        foreach (Profile profile in profileList.profiles) //iterate through all current profiles
        {
            if (username == profile.username) //if the username is already taken
            {
                invalidRegisterText.color = new Color32(255, 0, 0, 255); //make the text red
                invalidRegisterText.text = "Username already taken";
                StartCoroutine(ClearInvalidText());
                return;
            }
        }
        if (password.Length < 8) //if the password is less than 8 characters
        {
            invalidRegisterText.color = new Color32(255, 0, 0, 255); //make the text red
            invalidRegisterText.text = "Password must be at least 8 characters";
            StartCoroutine(ClearInvalidText());
            return;
        }
        #endregion

        Profile newProfile = new Profile(); //create new profile
        newProfile.username = username; //assign username
        newProfile.password = password; //assign password
        newProfile.highScore = 0; //assign high score
        newProfile.rating = 0; //assign rating
        newProfile.comment = ""; //assign comment

        profileList.profiles.Add(newProfile); //add the new profile to the list of profiles

        string json = JsonUtility.ToJson(profileList);
        File.WriteAllText(jsonFile, json); //update the file with the new list
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetInt("highScore", newProfile.highScore);

        invalidRegisterText.color = new Color32(0, 255, 0, 255); //make the text green
        invalidRegisterText.text = "Account created, logged in as " + username;
        PlayerPrefs.SetString("isLoggedIn", "true");
        StartCoroutine(ClearInvalidText());
    }
    #endregion

    private IEnumerator ClearInvalidText()
    {
        yield return new WaitForSeconds(2);
        invalidRegisterText.text = ""; //clear the register text
        if (invalidRegisterText.color == new Color32(0, 255, 0, 255)) //if the register text is green
        {
            SignUpMenu.SetActive(false);
        }
        invalidLoginText.text = ""; //clear the login text
        if (invalidLoginText.color == new Color32(0, 255, 0, 255)) //if the login text is green
        {
            LoginMenu.SetActive(false);
        }
    }
}