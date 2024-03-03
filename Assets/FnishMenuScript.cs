using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; //for moving between scenes
using System.IO; //allows File
using UnityEditor; //allows SeralizedObject & SerializedProperty
using TMPro; //allows TextMeshProUGUI

public class FnishMenuScript : MonoBehaviour
{
    #region variables
    public TextMeshProUGUI congratulationsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public GameObject reviewSection;
    public TMP_InputField commentField;
    private int score;

    private string jsonFile = "C:\\Users\\rorys\\Documents\\GitHub\\A-Level-Platformer\\Assets\\Start Menu\\profiles.json";
    private Profile profile;
    private string jsonString;
    private ProfileList profileList;
    private bool starPressed = false; //to ensure that the user cannot submit a review without a rating
    public GameObject[] stars; //an array of the star images
    private int starRating;
    #endregion

    // Start is called before the first frame update
    void Start() //used to load the values
    {
        jsonString = File.ReadAllText(jsonFile); //stores the current JSON file contents
        profileList = JsonUtility.FromJson<ProfileList>(jsonString);
        foreach (Profile profile in profileList.profiles) //iterates through all profiles
        {
            if (PlayerPrefs.GetString("username") == profile.username) //if there is a profile with a matching username and password
            {
                this.profile = profile; //set the profile to the current profile
            }
        }

        score = PlayerPrefs.GetInt("score"); //loads the score
        if (PlayerPrefs.GetString("isLoggedIn") == "true") {
            congratulationsText.text = "Congratulations " + PlayerPrefs.GetString("username") + "!"; //changes text at top of screen

            if (score > PlayerPrefs.GetInt("highScore")) { //if the player's score is higher than their current high score
                PlayerPrefs.SetInt("highScore", score); //update their high score
                scoreText.text = "Your score: " + score + " (High Score!)"; //show the user that they got a new highscore
            }
            else {scoreText.text = "Your score: " + score;}
            highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highScore");
        }
        else {
            congratulationsText.text = "Congratulations Guest!";
            scoreText.text = "Your score: " + score;
            highScoreText.text = ""; //hides the high score text
            reviewSection.SetActive(false); //hides the review section
        }
    }

    #region json
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
    #endregion

    public void Submit() {
        if (starPressed) { //if a star has been pressed
            profile.rating = starRating; //add the user's rating
            for (int i = 0; i < 5; i++) {
                stars[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Star"); //make all stars empty
            }
            if (commentField.text != "") { //if the user has written a comment
                profile.comment = commentField.text; //add the user's comment
                commentField.text = ""; //clear the user's comment
            }
            string json = JsonUtility.ToJson(profileList);
            File.WriteAllText(jsonFile, json); //update the file with the new list
        }
        else {
        }
    }

    public void Continue() {
        profile.highScore = PlayerPrefs.GetInt("highScore"); //update the high score
        string json = JsonUtility.ToJson(profileList);
        File.WriteAllText(jsonFile, json); //update the file with the new list
        SceneManager.LoadScene(0); //loads the start menu
    }

    public void Stars(int rating) {
        starPressed = true;
        for (int i = 0; i < 5; i++) {
            stars[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Star"); //make all stars empty
        }
        for (int i = 0; i < rating; i++) {
            stars[i].GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Star_filled"); //fill certain stars
        }
        starRating = rating;
    }
}