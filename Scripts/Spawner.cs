using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Spawner : MonoBehaviour {

    public GameObject[] pieces;
    public GameObject[] retroPieces;
    private GameObject previewNextPiece;
    private GameObject nextPiece;
    private GameObject ghostPiece;
    public GameObject destroyPieceOutline;
    private GameObject destroyPieces;
    private bool gameStarted = false;
    private Vector2 previewPiecePosition = new Vector2(-8f, 5);
    private Vector2 savedPiecePosition = new Vector2(-8f, 0);
    private GameObject savedPiece;
    private GameObject holderPiece;
    public bool pieceSavedThisTurn = false;
    public static int gridWidth;
    public static int gridHeight;
    public static bool bombReady = false;
    public static int bombCount = 0;
    Scene currentScene;
    string sceneName;


    // Use this for initialization
    void Start ()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        spawnNext();
        gridWidth = PlayerPrefs.GetInt("gridWidth");
        gridHeight = PlayerPrefs.GetInt("gridHeight");
    }

    public void spawnNext()
    {
        //spawns the pieces depending on the graphics set
        if (PlayerPrefs.GetInt("graphics") == 0)
        {
            //random number up to length of the tetronmino array
            int i = Random.Range(0, retroPieces.Length);
            //checks to see if the scene is classic level compared to extreme
            if (sceneName == "Level")
            {
                //checks to see if preview is enabled
                int x = PlayerPrefs.GetInt("previewPieceValue");
                if (x == 1)
                {
                    previewNext(i);
                }
                // if it isnt
                else
                {
                    //the next piece to spawn is spawned at the top of the grid from the array of tetronimos
                    nextPiece = (GameObject)Instantiate(retroPieces[i], transform.position, Quaternion.identity);
                    //assigned tag so it can be indentified from everything else on screen
                    nextPiece.tag = "currentActivePiece";
                    //spawns ghost tetromino if it's enabled
                    if (PlayerPrefs.GetInt("ghostPieceValue") == 1)
                    {
                        spawnGhostPiece();
                    }
                    //sets saved piece to false to allow saving for this current piece
                    pieceSavedThisTurn = false;
                }
            }
            //if extreme level, then all options ignored as they aren't required
            else
            {
                nextPiece = (GameObject)Instantiate(retroPieces[i], transform.position, Quaternion.identity);
                nextPiece.tag = "currentActivePiece";
                pieceSavedThisTurn = false;
            }
        }
        //same methods as above, except for retro graphics option
        else
        {
            int i = Random.Range(0, pieces.Length);
            if (sceneName == "Level")
            {
                int x = PlayerPrefs.GetInt("previewPieceValue");
                if (x == 1)
                {
                    previewNext(i);
                }
                else
                {
                    nextPiece = (GameObject)Instantiate(pieces[i], transform.position, Quaternion.identity);
                    nextPiece.tag = "currentActivePiece";
                    if (PlayerPrefs.GetInt("ghostPieceValue") == 1)
                    {
                        spawnGhostPiece();
                    }
                    pieceSavedThisTurn = false;
                }
            }
            else
            {
                nextPiece = (GameObject)Instantiate(pieces[i], transform.position, Quaternion.identity);
                nextPiece.tag = "currentActivePiece";
                pieceSavedThisTurn = false;
            }
        }
    }

    public void previewNext(int i)
    {
        //checks to see what graphics set
        if (PlayerPrefs.GetInt("graphics") == 0)
        {
            //gets random number up to length of tetromino array
            int x = Random.Range(0, retroPieces.Length);
            //checks to see if the game has started yet, this part only used at very start of game
            if (!gameStarted)
            {
                //sets the game to started
                gameStarted = true;
                //spawns the next piece from the array with random number generated at spawner position as this is the first piece to spawn
                nextPiece = (GameObject)Instantiate(retroPieces[i], transform.position, Quaternion.identity);
                //assigns tag so piece can be indentified from rest of game
                nextPiece.tag = "currentActivePiece";
                //preview piece spawned in the position determined above
                previewNextPiece = (GameObject)Instantiate(retroPieces[x], previewPiecePosition, Quaternion.identity);
                //stops the preview piece from being controllable
                previewNextPiece.GetComponent<Game>().enabled = false;
                spawnGhostPiece();
            }
            //this part used for rest of game
            else
            {
                //moves the preview piece to the middle of grid width and one above it
                previewNextPiece.transform.localPosition = new Vector2((int)gridWidth / 2, gridHeight + 1);
                //the next piece gameObject takes the properties from the previewPiece gameObject
                nextPiece = previewNextPiece;
                //game script now enabled
                nextPiece.GetComponent<Game>().enabled = true;
                nextPiece.tag = "currentActivePiece";
                //spawns new preview piece
                previewNextPiece = (GameObject)Instantiate(retroPieces[i], previewPiecePosition, Quaternion.identity);
                previewNextPiece.GetComponent<Game>().enabled = false;
                spawnGhostPiece();
            }
            pieceSavedThisTurn = false;
        }
        //same methods as above
        else
        {
            int x = Random.Range(0, pieces.Length);
            if (!gameStarted)
            {
                gameStarted = true;
                nextPiece = (GameObject)Instantiate(pieces[i], transform.position, Quaternion.identity);
                nextPiece.tag = "currentActivePiece";
                previewNextPiece = (GameObject)Instantiate(pieces[x], previewPiecePosition, Quaternion.identity);
                previewNextPiece.GetComponent<Game>().enabled = false;
                spawnGhostPiece();
            }
            else
            {
                previewNextPiece.transform.localPosition = new Vector2((int)gridWidth / 2, gridHeight + 1);
                nextPiece = previewNextPiece;
                nextPiece.GetComponent<Game>().enabled = true;
                nextPiece.tag = "currentActivePiece";
                previewNextPiece = (GameObject)Instantiate(pieces[i], previewPiecePosition, Quaternion.identity);
                previewNextPiece.GetComponent<Game>().enabled = false;
                spawnGhostPiece();
            }
            pieceSavedThisTurn = false;
        }  
    }

    public void spawnGhostPiece()
    {
        //checks to see ghost is enabled
        if (PlayerPrefs.GetInt("ghostPieceValue") == 1)
        {
            //if a gameObject has the current active piece tag then the ghost piece is destroyed to stop multiple multiple ghost pieces
            if (GameObject.FindGameObjectWithTag("currentActivePiece") != null)
            {
                Destroy(GameObject.FindGameObjectWithTag("currentGhostPiece"));
            }
            //spawns new ghost piece
            ghostPiece = (GameObject)Instantiate(nextPiece, nextPiece.transform.position, Quaternion.identity);
            //removes game script and adds ghost script from ghost piece so it behaves as ghost piece and not actual piece
            Destroy(ghostPiece.GetComponent<Game>());
            ghostPiece.AddComponent<ghostPiece>();
        }
    }

    public void destroyPiece()
    {
        //only used for windows system and classic level
        #if UNITY_STANDALONE_WIN
        if(sceneName=="Level")
        {
            //checks to see if the bomb has already been unlocked so bombs don't stack
            if (bombCount == 0)
            {
                //checks to see if bomb is ready to use ie unlocked and spawns it, removing game script and adding bomb script to game object
                if (bombReady)
                {
                    destroyPieces = (GameObject)Instantiate(destroyPieceOutline, new Vector2((int)gridWidth / 2, gridHeight - 1), Quaternion.identity);
                    Destroy(destroyPieces.GetComponent<Game>());
                    destroyPieces.AddComponent<destroyPiece>();
                }
                else
                {
                    //if not bomb ready remove it from view point and destroy it
                    destroyPieces.transform.localPosition = new Vector2(1000, 1000);
                    Destroy(destroyPieces.GetComponent<destroyPiece>());
                }
            }
        } 
        #endif
    }

    public void savePiece()
    {
        if (sceneName == "Level")
        {
            //checks to see if piece saved this turn
            if (!pieceSavedThisTurn)
            {
                //if it hasn't then savedPiece object takes values of nextPiece object and has scripts switched
                if (savedPiece == null)
                {
                    savedPiece = nextPiece;
                    savedPiece.transform.localPosition = savedPiecePosition;
                    savedPiece.GetComponent<Game>().enabled = false;
                    nextPiece.GetComponent<Game>().enabled = true;
                }
                //if it has then the two objects switch
                else
                {
                    nextPiece.GetComponent<Game>().enabled = false;
                    holderPiece = savedPiece;
                    savedPiece = nextPiece;
                    savedPiece.tag = "savedPiece";
                    nextPiece = holderPiece;
                    nextPiece.tag = "currentActivePiece";
                    nextPiece.transform.localPosition = new Vector2((int)gridWidth / 2, gridHeight + 1);
                    nextPiece.GetComponent<Game>().enabled = true;
                    savedPiece.transform.localPosition = savedPiecePosition;
                    spawnGhostPiece();
                }
            }
            pieceSavedThisTurn = true;
        }
    }

    //pauses game script - stops tetromino falling
    public void pause()
    {
        nextPiece.GetComponent<Game>().enabled = false;
    }

    //resumes game script
    public void resume()
    {
        nextPiece.GetComponent<Game>().enabled = true;
    }

    
}
