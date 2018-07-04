using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public static int gridWidth;
    public static int gridHeight;
    public int singlePiece = 10;
    public int oneLine = 100;
    public int twoLine = 200;
    public int threeLine = 300;
    public int fourLine = 500;
    public static int score = 0;
    public int rowsCompleted = 0;
    public int level = 1;
    public static Transform[,] grid;
    public GameObject spawner;
    public GameObject leftBorder;
    public GameObject rightBorder;
    public GameObject bottomBorder;

    public Text score_Text;
    public Text level_text;
    public Text saved_piece;
    public Text next_piece;

    public Canvas nextPieceCanvas;
    public Canvas savedPieceCanvas;

    public AudioSource audioSource;

    Scene currentScene;
    string sceneName;

    void Start()
    {
        level = 1;
        score = 0;
        gridWidth = PlayerPrefs.GetInt("gridWidth");
        gridHeight = PlayerPrefs.GetInt("gridHeight");
        spawner.transform.position = new Vector3 (gridWidth/2,gridHeight+1);
        grid = new Transform[gridWidth, gridHeight];
        leftBorder.transform.localScale = new Vector3(1, gridHeight); //sets length of  border
        leftBorder.transform.position = new Vector3(-1, (gridHeight / 2)-1); // set position of border
        rightBorder.transform.localScale = new Vector3(1, gridHeight);
        rightBorder.transform.position = new Vector3((gridWidth), (gridHeight / 2) - 1);
        bottomBorder.transform.localScale = new Vector3(gridWidth, 1);
        //positions spawner in middle of grid depnding on width is odd or even
        if(gridWidth % 2 == 0)
        {
            bottomBorder.transform.position = new Vector3((gridWidth / 2)-0.5f, -1);
        }
        else
        {
            bottomBorder.transform.position = new Vector3((gridWidth / 2), -1);
        }
        setGraphics();
        checkMute();
        displayNextPieceText();
        displaySavedPieceText();

        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    void Update()
    {
        //random range that means all pieces spawn within the grid
        int i = Random.Range(4, gridWidth-4);
        if (sceneName == "ExtremeLevel")
        {
            //stops pieces spawning outside of grid 
            if(gridWidth>9)
            {
                //spawns pieces in random position on extreme level
                spawner.transform.position = new Vector3(i, gridHeight + 1);
            }
        }
        updateScore();
        updateUI();
    }

    //shows current score
    public void updateUI()
    {
        score_Text.text = "Score: " + score.ToString();
    }

    //called when game over and destroys objects on screen
    public void gameOver()
    {
        FindObjectOfType<linesAudio>().gameOver();
        Destroy(gameObject);
        SceneManager.LoadScene("GameOver");
    }

    //updates the users score depending upon rows completed
    public void updateScore()
    {
        if (rowsCompleted > 0)
        {
            switch (rowsCompleted)
            {
                case 1:
                    oneLineCompleted();
                    break;
                case 2:
                    twoLineCompleted();
                    break;
                case 3:
                    threeLineCompleted();
                    break;
                case 4:
                    fourLineCompleted();
                    break;
                default:
                    break;
            }
            rowsCompleted = 0;
        }
        checkScore();
    }

    //updates level depending on user score
    //needs improving
    public void checkScore()
    {
        if (score >= 1000 && score < 2000)
        {
            level = 2;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 2000 && score < 3000)
        {
            level = 3;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 3000 && score < 4000)
        {
            level = 4;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 4000 && score < 5000)
        {
            level = 5;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 5000 && score < 6000)
        {
            level = 6;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 6000 && score < 7000)
        {
            level = 7;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 8000 && score < 9000)
        {
            level = 8;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 9000 && score < 10000)
        {
            level = 9;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 10000 && score < 11000)
        {
            level = 10;
            level_text.text = "Level: " + level.ToString();
        }
        else if (score >= 11000)
        {
            level = 11;
            level_text.text = "Level: " + level.ToString();
        }
    }

    public void singlePieceCompleted()
    {
        score += singlePiece * level;
    }

    public void oneLineCompleted()
    {
        score += oneLine*level;
        if(PlayerPrefs.GetInt("mute")==0)
        {
            FindObjectOfType<linesAudio>().oneLine();
        }
    }

    public void twoLineCompleted()
    {
        score += twoLine*level;
        if (PlayerPrefs.GetInt("mute") == 0)
        {
            FindObjectOfType<linesAudio>().twoLines();
        }
        //allows for bomb to be unlocked on easiest difficulty if two lines completed
        if(PlayerPrefs.GetInt("speed")==1)
        {
            checkBombDifficulty();
        }
    }

    public void threeLineCompleted()
    {
        score += threeLine*level;
        if (PlayerPrefs.GetInt("mute") == 0)
        {
            FindObjectOfType<linesAudio>().threeLines();
        }
        //bomb unlocked when three lines cleared on easy or medium
        if (PlayerPrefs.GetInt("speed") <= 3)
        {
            checkBombDifficulty();
        }
    }

    public void fourLineCompleted()
    {
        score += fourLine*level;
        if (PlayerPrefs.GetInt("mute") == 0)
        {
            FindObjectOfType<linesAudio>().fourLines();
        }
        //bomb unlocked on all difficuties if 4 rows cleared
        if (PlayerPrefs.GetInt("speed") <= 5)
        {
            checkBombDifficulty();
        }
    }

    //reads in the pieces position from game script and sees if it is above the height of the grid
    //if it isn't then a new piece can be spawned
    public bool canSpawnPiece(Game pieces)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            foreach (Transform piece in pieces.transform)
            {
                Vector2 pos = round(piece.position);
                if (pos.y > gridHeight - 1)
                {
                    //essentially game over
                    return false;
                }
            }
        }
        singlePieceCompleted();  
        //can spawn new piece  
        return true;
    }

    //reads in height of position and sees if that row is now full
    public bool fullRow(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }
        rowsCompleted += 1;
        return true;
    }

    //deletes all objects at that height
    public void deleteRow(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    //moves rows down by one
    public void updateRow(int y)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }

    //moves rows down by one
    public void updateRows(int y)
    {
        for (int i = y; i < gridHeight; i++) //moved out of updateRows as it would only update row above
        {
            updateRow(i);
        }
    }

    //checks height and if that has a full row, passes the height to methods if row is full
    public void deleteRows()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            if (fullRow(y))
            {
                deleteRow(y);
                updateRows(y + 1);
                y--;
            }
        }
    }

    //updates position on pieces on the grid that are in use by the user. ie the piece currently falling
    public void updateGrid(Game pieces)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == pieces.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform piece in pieces.transform)
        {
            Vector2 pos = round(piece.position);
            if (pos.y < gridHeight)
            {
                grid[(int)pos.x, (int)pos.y] = piece;
            }
        }
    }

    //moves saved piece from the saved position and sets the values to null so other pieces can be stored there
    public void removeSavedPiece(float x, float y)
    {
        int i = (int)x;
        int j = (int)y;
        if (j<gridHeight)
        {
            grid[i, j] = null;
        }
        
    }

    //used to check if piece is inside height of grid for ghost pieces
    public Transform getTransformGridPosition(Vector2 pos)
    {
        if (pos.y > gridHeight - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }

    //used to check that piece is inside width and not below bottom of grid
    public bool isInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0);
    }

    //checks that bomb is inside grid. Different method to isInsideGrid as this requires height as well
    public bool bombInsideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < gridWidth && (int)pos.y >= 0 && (int)pos.y <= gridHeight);
    }

    //destroys gameObject and array values set to null for bomb use. Reads in bomb position and destroys anything also at that position
    public void destroyPieces(Vector2 pos)
    {
        int i = (int)pos.x;
        int j = (int)pos.y;
        if(grid[i, j]!= null)
        {
            Destroy(grid[i, j].gameObject);
            grid[i, j] = null;
        }
    }

    //rounds positions of objects as they can sometimes be slightly off if not rounded
    public Vector2 round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

    //sets UI colours depending on graphic choice
    public void setGraphics()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (PlayerPrefs.GetInt("graphics")==0)
        {
            Camera.main.clearFlags = CameraClearFlags.SolidColor;
            Camera.main.backgroundColor = Color.gray;
            score_Text.color = Color.black;
            level_text.color = Color.black;
            if (sceneName == "Level")
            {
                saved_piece.color = Color.black;
                next_piece.color = Color.black;
            }
        }
        else
        {
            Camera.main.backgroundColor = Color.black;
            score_Text.color = Color.magenta;
            level_text.color = Color.magenta;
            if (sceneName=="Level")
            {
                saved_piece.color = Color.magenta;
                next_piece.color = Color.magenta;
            } 
        }
    }

    public void checkMute()
    {
        if(PlayerPrefs.GetInt("mute")==1)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
    }

    //stops previewPiece being visible in extreme level no matter the value set
    public void displayNextPieceText()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if(sceneName=="Level")
        {
            if (PlayerPrefs.GetInt("previewPieceValue") == 1)
            {
                nextPieceCanvas.enabled = true;
            }
            else
            {
                nextPieceCanvas.enabled = false;
            }
        }
    }

    //stops savePiece being visible in extreme level no matter the value set
    public void displaySavedPieceText()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Level")
        {
            if (PlayerPrefs.GetInt("storePieceValue") == 1)
            {
                savedPieceCanvas.enabled = true;
            }
            else
            {
                savedPieceCanvas.enabled = false;
            }
        }
    }

    //enables bomb to be ready if enough lines cleared depending on difficulty selected
    public void checkBombDifficulty()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        if (sceneName == "Level")
        {
            if (PlayerPrefs.GetInt("bombValue") == 1)
            {
                Spawner.bombReady = true;
                FindObjectOfType<Spawner>().destroyPiece();
                Spawner.bombCount = 1;
            }
        }
    }
}
