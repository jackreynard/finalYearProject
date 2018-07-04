using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.ImageEffects;


public class pauseMenu : MonoBehaviour {

    public Button resumeGameButton;
    public Button menuButton;
    public Canvas pauseMenuCanvas;
    public Canvas playerInfoCanvas;
    public Canvas nextPieceCanvas;
    public Canvas savedPieceCanvas;
    public Text pausedText;
    Scene currentScene;
    string sceneName;

    //called when the scene opens
    void Awake()
    {
        //hides the pauseMenucanvas
        pauseMenuCanvas.enabled = false;
        //positions menu in middle of screen
        pauseMenuCanvas.transform.position = new Vector3(Screen.width/2, Screen.height/2);
        playerInfoCanvas.enabled = true;
        //gets name of current scene
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    //enables the pause menu
    public void loadMenu()
    {
        pauseMenuCanvas.enabled = true;
        playerInfoCanvas.enabled = false;
        if(sceneName=="Level")
        {
            nextPieceCanvas.enabled = false;
            savedPieceCanvas.enabled = false;
        }
        
        //adds blur component to camera in Level scene to blur background
        Camera.main.GetComponent<BlurOptimized>().enabled = true;

        //changes background colour of camera depending on graphic option
        if (PlayerPrefs.GetInt("graphics") == 0)
        {
            Camera.main.backgroundColor = Color.gray;
            pausedText.color = Color.black;
        }
        else
        {
            Camera.main.backgroundColor = Color.black;
            pausedText.color = Color.magenta;
        }
    }

    public void resumeGame()
    {
        //calls resume method from spawner script
        FindObjectOfType<Spawner>().resume();
        pauseMenuCanvas.enabled = false;
        playerInfoCanvas.enabled = true;
        //checks values of preview piece to see if option should be enabled when resuming
        if (PlayerPrefs.GetInt("previewPieceValue") == 1)
        {
            if(sceneName=="Level")
            {
                nextPieceCanvas.enabled = true;
            }
        }
        //checks values of save piece to see if option should be enabled when resuming
        if (PlayerPrefs.GetInt("storePieceValue") == 1)
        {
            if (sceneName == "Level")
            {
                savedPieceCanvas.enabled = true;
            }
        }
        Camera.main.GetComponent<BlurOptimized>().enabled = false;
    }

    //restarts the game on the type the user was previously playing
    public void restartGame()
    {
        if(sceneName=="Level")
        {
            SceneManager.LoadScene("Level");
        }
        else
        {
            SceneManager.LoadScene("ExtremeLevel");
        }
    }

    //loads menu
    public void returnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //quits application
    public void quit()
    {
        Application.Quit();
    }

}
