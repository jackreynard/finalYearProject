using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameOverMenu : MonoBehaviour {

    private int score = 0;
    public Text scoreText;
    Scene currentScene;
    string sceneName;

    //called when the scene opens
    public void Awake()
    {
        score = Grid.score;
        scoreText.text = "Score: " + score;
        //calls the Awake method from the addNewHighScore script
        FindObjectOfType<addNewHighScore>().Awake();
        //gets name of current scene
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    //loads level select scene for play again
    public void playAgain()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    //loads menu for return to menu button
    public void toMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
