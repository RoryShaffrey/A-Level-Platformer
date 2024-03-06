using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; //for TimeSpan

public class StopwatchScript : MonoBehaviour
{
    private float currentTime = 0f;
    public TMPro.TextMeshProUGUI stopwatchText;
    public bool playing = true;
    private int score;

    // Update is called once per frame
    void Update()
    {
        if (playing) {
        currentTime += Time.deltaTime; //the total of how much time has been played in this scene
        TimeSpan passedTime = TimeSpan.FromSeconds(currentTime); //allows me to use a nicer format for the stopwatch
        stopwatchText.text = passedTime.ToString(@"mm\:ss\:ff"); //formats the time with minutes, then seconds then milliseconds
        }
        else {
            score = 150000 - Mathf.RoundToInt(currentTime * 500f); //gets the score of the player
            if (score < 0) {score = 0;} //prevents a negative score
            PlayerPrefs.SetInt("score", score); //saves the score
        }
    }
}