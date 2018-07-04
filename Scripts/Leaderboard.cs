using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour {

    public Text highScoreText;
    public Text highScoreText2;
    public Text highScoreText3;
    public Text highScoreText4;
    public Text highScoreText5;

    public Text playerName;
    public Text playerName2;
    public Text playerName3;
    public Text playerName4;
    public Text playerName5;


    //constantly checks to see if esc has been pressed
    void Update()
    {
        returnToMenu();
    }

    // Use this for initialization
    //sets the variables to the correspondng playerPref values
    void Start () {
        highScoreText.text = PlayerPrefs.GetInt("highscore").ToString();
        highScoreText2.text = PlayerPrefs.GetInt("highscore2").ToString();
        highScoreText3.text = PlayerPrefs.GetInt("highscore3").ToString();
        highScoreText4.text = PlayerPrefs.GetInt("highscore4").ToString();
        highScoreText5.text = PlayerPrefs.GetInt("highscore5").ToString();

        playerName.text = PlayerPrefs.GetString("name");
        playerName2.text = PlayerPrefs.GetString("name2");
        playerName3.text = PlayerPrefs.GetString("name3");
        playerName4.text = PlayerPrefs.GetString("name4");
        playerName5.text = PlayerPrefs.GetString("name5");
    }

    //clears playerPrefs and resets to default values
    public void resetScores()
    {
        SceneManager.LoadScene("Leaderboard");
        PlayerPrefs.SetInt("highscore", 3000);
        PlayerPrefs.SetInt("highscore2", 1500);
        PlayerPrefs.SetInt("highscore3", 500);
        PlayerPrefs.SetInt("highscore4", 200);
        PlayerPrefs.SetInt("highscore5", 100);

        PlayerPrefs.SetString("name", "Tetris");
        PlayerPrefs.SetString("name2", "PacMan");
        PlayerPrefs.SetString("name3", "Pong");
        PlayerPrefs.SetString("name4", "Space Inavders");
        PlayerPrefs.SetString("name5", "Snake"); 
    }

    //returns to main menu
    void returnToMenu()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
