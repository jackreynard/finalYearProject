using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour {

    public Dropdown speed;
    public Dropdown graphics;
    public Scrollbar previewPiece;
    public Scrollbar storePiece;
    public Scrollbar ghostPiece;
    public Scrollbar bomb;
    public int gridWidth;
    public InputField savedGridWidth;
    public int gridHeight;
    public InputField savedGridHeight;
    public Color optionOn;
    public Color optionOff;
    private int number;
    public Canvas mute;
    public Canvas unmute;

    //called between every frame
    void Update()
    {
        returnToMenu();
    }

    //called when scene opens
    //checks values of options set by user to see what enabled/disabled
    void Awake ()
    {
        Mathf.RoundToInt(previewPiece.value);
        previewPiece.value = PlayerPrefs.GetInt("previewPieceValue");
        checkPreviewPieceValue();

        Mathf.RoundToInt(storePiece.value);
        storePiece.value = PlayerPrefs.GetInt("storePieceValue");
        checkStoredPieceValue();

        Mathf.RoundToInt(ghostPiece.value);
        ghostPiece.value = PlayerPrefs.GetInt("ghostPieceValue");
        checkGhostPieceValue();

        //if on windows, then bomb can be enabled if set to be
        #if UNITY_STANDALONE_WIN
            Mathf.RoundToInt(bomb.value);
            bomb.value = PlayerPrefs.GetInt("bombValue");
            checkBombValue();
        #endif

        loadGridWidth();
        loadGridHeight();

        checkSpeedValue();
        checkGraphicsValue();
        checkMute();
    }

    //sets starting speed depending difficulty
    public void setSpeed()
    {
        if(speed.value==0)
        {
            PlayerPrefs.SetInt("speed", 1);
        }
        else if (speed.value == 1)
        {
            PlayerPrefs.SetInt("speed", 3);
        }
        else
        {
            PlayerPrefs.SetInt("speed", 5);
        }
    }

    //sets graphics 
    public void setGraphics()
    {
        if (graphics.value == 0)
        {
            PlayerPrefs.SetInt("graphics", 0);
        }
        else
        {
            PlayerPrefs.SetInt("graphics", 1);
        }
    }

    //sets preview value
    public void setPreview()
    {
        checkPreviewPieceValue();
        PlayerPrefs.SetInt("previewPieceValue", Mathf.RoundToInt(previewPiece.value));
    }

    //sets store piece value
    public void setStore()
    {
        checkStoredPieceValue();
        PlayerPrefs.SetInt("storePieceValue", Mathf.RoundToInt(storePiece.value));
    }

    //sets ghost value
    public void setGhost()
    {
        checkGhostPieceValue();
        PlayerPrefs.SetInt("ghostPieceValue", Mathf.RoundToInt(ghostPiece.value));
    }

    //sets bomb value
    public void setBomb()
    {
        checkBombValue();
        PlayerPrefs.SetInt("bombValue", Mathf.RoundToInt(bomb.value));
    }

    //sets grid width
    public void setGridWidth()
    {
        //checks that the value being parsed is an integer
        if (int.TryParse(savedGridWidth.text, out number))
        {
            int x = int.Parse(savedGridWidth.text);
            if ((x>=3) && (x<=20))
            {
                gridWidth = int.Parse(savedGridWidth.text);
                PlayerPrefs.SetInt("gridWidth", gridWidth);
            }
            else
            {
                savedGridWidth.text = "NA";
                ColorBlock savedGridWidthColour = savedGridWidth.colors;
                savedGridWidthColour.normalColor = optionOff;
                savedGridWidthColour.highlightedColor = optionOff;
                savedGridWidthColour.pressedColor = optionOff;
                savedGridWidth.colors = savedGridWidthColour;
            }
        }
        else
        {
            savedGridWidth.text = "NA";
            ColorBlock savedGridWidthColour = savedGridWidth.colors;
            savedGridWidthColour.normalColor = optionOff;
            savedGridWidthColour.highlightedColor = optionOff;
            savedGridWidthColour.pressedColor = optionOff;
            savedGridWidth.colors = savedGridWidthColour;
        }
    }

    //sets grid height
    public void setGridHeight()
    {
        //checks that the value being parsed is an integer
        if (int.TryParse(savedGridHeight.text, out number))
        {
            int x = int.Parse(savedGridHeight.text);
            if ((x >= 4) && (x <= 20))
            {
                gridHeight = int.Parse(savedGridHeight.text);
                PlayerPrefs.SetInt("gridHeight", gridHeight);
            }
            else
            {
                savedGridHeight.text = "NA";
                ColorBlock savedGridHeightColour = savedGridHeight.colors;
                savedGridHeightColour.normalColor = optionOff;
                savedGridHeightColour.highlightedColor = optionOff;
                savedGridHeightColour.pressedColor = optionOff;
                savedGridHeight.colors = savedGridHeightColour;
            }
        }
        else
        {
            savedGridHeight.text = "NA";
            ColorBlock savedGridHeightColour = savedGridHeight.colors;
            savedGridHeightColour.normalColor = optionOff;
            savedGridHeightColour.highlightedColor = optionOff;
            savedGridHeightColour.pressedColor = optionOff;
            savedGridHeight.colors = savedGridHeightColour;
        }
    }

    //depending on mute value, depnds which button is shown for mute option
    public void checkMute()
    {
        if (PlayerPrefs.GetInt("mute") == 0)
        {
            unmute.enabled = false;
            mute.enabled = true;
        }
        else
        {
            unmute.enabled = true;
            mute.enabled = false;
        }
    }

    //sets position and colour of slider for preview 
    public void checkPreviewPieceValue()
    {
        if (previewPiece.value >= 0.5)
        {
            ColorBlock previewPieceStartColour = previewPiece.colors;
            previewPieceStartColour.normalColor = optionOn;
            previewPieceStartColour.highlightedColor = optionOn;
            previewPieceStartColour.pressedColor = optionOn;
            previewPiece.colors = previewPieceStartColour;
        }
        else
        {
            ColorBlock previewPieceStartColour = previewPiece.colors;
            previewPieceStartColour.normalColor = optionOff;
            previewPieceStartColour.highlightedColor = optionOff;
            previewPieceStartColour.pressedColor = optionOff;
            previewPiece.colors = previewPieceStartColour;
        }
    }

    //sets position and colour of slider for store 
    public void checkStoredPieceValue()
    {
        if (storePiece.value >= 0.5)
        {
            ColorBlock storedPieceStartColour = storePiece.colors;
            storedPieceStartColour.normalColor = optionOn;
            storedPieceStartColour.highlightedColor = optionOn;
            storedPieceStartColour.pressedColor = optionOn;
            storePiece.colors = storedPieceStartColour;
        }
        else
        {
            ColorBlock storedPieceStartColour = storePiece.colors;
            storedPieceStartColour.normalColor = optionOff;
            storedPieceStartColour.highlightedColor = optionOff;
            storedPieceStartColour.pressedColor = optionOff;
            storePiece.colors = storedPieceStartColour;
        }
    }

    //sets position and colour of slider for ghost 
    public void checkGhostPieceValue()
    {
        if (ghostPiece.value >= 0.5)
        {
            ColorBlock ghostPieceStartColour = ghostPiece.colors;
            ghostPieceStartColour.normalColor = optionOn;
            ghostPieceStartColour.highlightedColor = optionOn;
            ghostPieceStartColour.pressedColor = optionOn;
            ghostPiece.colors = ghostPieceStartColour;
        }
        else
        {
            ColorBlock ghostPieceStartColour = ghostPiece.colors;
            ghostPieceStartColour.normalColor = optionOff;
            ghostPieceStartColour.highlightedColor = optionOff;
            ghostPieceStartColour.pressedColor = optionOff;
            ghostPiece.colors = ghostPieceStartColour;
        }
    }

    //sets position and colour of slider for bomb 
    public void checkBombValue()
    {
        if (bomb.value >= 0.5)
        {
            ColorBlock bombStartColour = bomb.colors;
            bombStartColour.normalColor = optionOn;
            bombStartColour.highlightedColor = optionOn;
            bombStartColour.pressedColor = optionOn;
            bomb.colors = bombStartColour;
        }
        else
        {
            ColorBlock bombStartColour = bomb.colors;
            bombStartColour.normalColor = optionOff;
            bombStartColour.highlightedColor = optionOff;
            bombStartColour.pressedColor = optionOff;
            bomb.colors = bombStartColour;
        }
    }

    //sets dropdown option for speed depending on value
    public void checkSpeedValue()
    {
        if (PlayerPrefs.GetInt("speed") == 1)
        {
            speed.value = 0;
        }
        else if (PlayerPrefs.GetInt("speed") == 3)
        {
            speed.value = 1;
        }
        else
        {
            speed.value = 2;
        }
    }

    //sets dropdown graphics for speed depending on value
    public void checkGraphicsValue()
    {
        if (PlayerPrefs.GetInt("graphics") == 0)
        {
            graphics.value = 0;
        }
        else
        {
            graphics.value = 1;
        }
    }

    //sets grid width value in text box
    public void loadGridWidth()
    {
        savedGridWidth.text = PlayerPrefs.GetInt("gridWidth").ToString();
    }

    //sets grid height value in text box
    public void loadGridHeight()
    {
        savedGridHeight.text = PlayerPrefs.GetInt("gridHeight").ToString();
    }

    //loads the options scene depending on platform as screen has to refresh to display that button has been pressed
    public void muteSound()
    {
        #if UNITY_STANDALONE_WIN
            if (PlayerPrefs.GetInt("mute")==0)
            {
                PlayerPrefs.SetInt("mute", 1);
                unmute.enabled = false;
                mute.enabled = true;
                SceneManager.LoadScene("Options");
            }
            else
            {
                PlayerPrefs.SetInt("mute", 0);
                unmute.enabled = true;
                mute.enabled = false;
                SceneManager.LoadScene("Options");
            }
        #else
            if (PlayerPrefs.GetInt("mute") == 0)
            {
                PlayerPrefs.SetInt("mute", 1);
                unmute.enabled = false;
                mute.enabled = true;
                SceneManager.LoadScene("OptionsAndroid");
            }
            else
            {
                PlayerPrefs.SetInt("mute", 0);
                unmute.enabled = true;
                mute.enabled = false;
                SceneManager.LoadScene("OptionsAndroid");
            }
        #endif
    }

    //returns to menu
    void returnToMenu()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
