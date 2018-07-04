using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {

    //called when game loads and sets playerPrefs for grid if this is the first time loading
    void Start()
    {
        if(PlayerPrefs.GetInt("gridWidth")==0)
        {
            PlayerPrefs.SetInt("gridWidth", 10);
        }
        if (PlayerPrefs.GetInt("gridHeight") == 0)
        {
            PlayerPrefs.SetInt("gridHeight", 10);
        }
        //sets the hghscores to default values when system first loaded
        if(PlayerPrefs.GetInt("highscore") == 0)
        {
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
    }

    void Update()
    {
        returnToMenu();
    }

    //loads level scene
    public void play()
    {
        SceneManager.LoadScene("Level");
    }

    //loads option scene
    public void options()
    {
        #if UNITY_STANDALONE_WIN
            SceneManager.LoadScene("Options");
        #else
            SceneManager.LoadScene("OptionsAndroid");
        #endif
    }

    //loads  leaderboard scene
    public void leaderboard()
    {
        SceneManager.LoadScene("Leaderboard");
    }

    //loads credits scene
    public void credits()
    {
        SceneManager.LoadScene("Credits");
    }

    //loads main menu
    public void loadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //quits application
    public void quit()
    {
        Application.Quit();
    }

    //returns to main menu if scene isn't the actual game or quits if main menu
    void returnToMenu()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))//joystickButton1 is B on controller
        {
            //required as script loaded in multiple screens and stops esc or B doing something unintended
            if ((sceneName == "Credits") || (sceneName == "androidControls") || (sceneName == "Controls") || (sceneName == "Leaderboard") || (sceneName == "Options")|| (sceneName == "LevelSelect"))
            {
                SceneManager.LoadScene("Menu");
            }
            else if (sceneName == "Menu")
            {
                quit();
            }
        }
    }

    //loads different controls scene depending on platform
    public void controls()
    {
        #if UNITY_STANDALONE_WIN
            SceneManager.LoadScene("Controls");
        #else
            SceneManager.LoadScene("androidControls");
        #endif
    }

    //loads extreme version of game
    public void extremeLevel()
    {
        SceneManager.LoadScene("ExtremeLevel");
    }

    //loads level select scene
    public void levelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
        
