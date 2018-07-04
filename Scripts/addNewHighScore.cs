using UnityEngine;
using System.Collections;

public class addNewHighScore : MonoBehaviour {

    private int startingHighScore;
    private int startingHighScore2;
    private int startingHighScore3;
    private int startingHighScore4;
    private int startingHighScore5;
    private string startingName;
    private string startingName2;
    private string startingName3;
    private string startingName4;
    public Canvas newHighScore;
    public Canvas buttons;
    private int score = 0;

    //called when scene is opened
    //sets the variables that are linked to gameObjects in Unity to values within PlayerPrefs
    public void Awake()
    {
        startingHighScore = PlayerPrefs.GetInt("highscore");
        startingHighScore2 = PlayerPrefs.GetInt("highscore2");
        startingHighScore3 = PlayerPrefs.GetInt("highscore3");
        startingHighScore4 = PlayerPrefs.GetInt("highscore4");
        startingHighScore5 = PlayerPrefs.GetInt("highscore5");
        startingName = PlayerPrefs.GetString("name");
        startingName2 = PlayerPrefs.GetString("name2");
        startingName3 = PlayerPrefs.GetString("name3");
        startingName4 = PlayerPrefs.GetString("name4");
        score = Grid.score;
        setNewHighScore();
    }

    //checks to see if the score from the user is better than the worst highscore and if it is the user can enter their name
    //the buttons to restart and return to menu are hidden. else the buttons are shown and name hidden
    public void setNewHighScore()
    {
        if (score > PlayerPrefs.GetInt("highscore5"))
        {
            newHighScore.enabled = true;
            buttons.enabled = false;
        }
        else
        {
            newHighScore.enabled = false;
            buttons.enabled = true;
        }
    }

    //checks to see where the highscore should be entered and reorders values
    public void updateHighScore(string name)
    {
        if (score > startingHighScore)
        {
            PlayerPrefs.SetInt("highscore5", startingHighScore4);
            PlayerPrefs.SetInt("highscore4", startingHighScore3);
            PlayerPrefs.SetInt("highscore3", startingHighScore2);
            PlayerPrefs.SetInt("highscore2", startingHighScore);
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.SetString("name5", startingName4);
            PlayerPrefs.SetString("name4", startingName3);
            PlayerPrefs.SetString("name3", startingName2);
            PlayerPrefs.SetString("name2", startingName);
            PlayerPrefs.SetString("name", name);
        }
        else if (score > startingHighScore2)
        {
            PlayerPrefs.SetInt("highscore5", startingHighScore4);
            PlayerPrefs.SetInt("highscore4", startingHighScore3);
            PlayerPrefs.SetInt("highscore3", startingHighScore2);
            PlayerPrefs.SetInt("highscore2", score);
            PlayerPrefs.SetString("name5", startingName4);
            PlayerPrefs.SetString("name4", startingName3);
            PlayerPrefs.SetString("name3", startingName2);
            PlayerPrefs.SetString("name2", name);
        }
        else if (score > startingHighScore3)
        {
            PlayerPrefs.SetInt("highscore5", startingHighScore4);
            PlayerPrefs.SetInt("highscore4", startingHighScore3);
            PlayerPrefs.SetInt("highscore3", score);
            PlayerPrefs.SetString("name5", startingName4);
            PlayerPrefs.SetString("name4", startingName3);
            PlayerPrefs.SetString("name3", name);
        }
        else if (score > startingHighScore4)
        {
            PlayerPrefs.SetInt("highscore5", startingHighScore4);
            PlayerPrefs.SetInt("highscore4", score);
            PlayerPrefs.SetString("name5", startingName4);
            PlayerPrefs.SetString("name4", name);
        }
        else if (score > startingHighScore5)
        {
            PlayerPrefs.SetInt("highscore5", score);
            PlayerPrefs.SetString("name5", name);
        }
    }

    //only shows buttons for new highscore if the name contains a value
    public void showButtons(string name)
    {
        if (name==null)
        {
            buttons.enabled = false;
        }
        else
        {
            buttons.enabled = true;
        }
    }
}

