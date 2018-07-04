using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    float lastFall = 0;

    public bool allowRotation;
    public bool limitRotation;
    public static int speed;
    double fallSpeed;
    private float continuousDownSpeed = 0.05f;
    private float continuousHorizontalSpeed = 0.1f;
    private float verticalTimer = 0;
    private float horizontalTimer = 0;
    private float buttonDownWaitMax = 0.2f;
    private float buttonDownWaitTimerHorizontal = 0;
    private float buttonDownWaitTimerVertical = 0;
    private bool moveHorizontal = false;
    private bool moveVertical = false;
    private int x;
    private bool vAxisInUse = false;
    

    public AudioClip moveSound;
    public AudioClip rotateSound;
    public AudioClip landSound;
    public AudioClip storeSound;
    private AudioSource audioSource;



    private int touchSensitivityHorizontal = 8;
    private int touchSensitivityVertical = 4;
    Vector2 previousUnitPosition = Vector2.zero;
    Vector2 direction = Vector2.zero;
    bool moved = false;
    Scene currentScene;
    string sceneName;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        checkMute();
    }

    void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        x = PlayerPrefs.GetInt("speed");
    }

    // Update is called once per frame
    //constantly checking for user input, and what level the game is on to set correct speed
    void Update ()
    {
        userInput();
        if (sceneName == "Level")
        {
            if (x <= FindObjectOfType<Grid>().level)
            {
                speed = FindObjectOfType<Grid>().level;
            }
            else
            {

                speed = x;
            }
            fallSpeed = 1.0 / speed;
        }
        else // speed for extreme level
        {
            speed = 10;
        }
    }

    //checks what input user has given and calls corresponding methods
    void userInput()
    {
        //stops piece being held indefinitely if key is held down
        if (Input.GetKeyUp(KeyCode.RightArrow)||Input.GetKeyUp(KeyCode.LeftArrow))
        {
            moveHorizontal = false;
            horizontalTimer = 0;
            buttonDownWaitTimerHorizontal = 0;
        }
        //stops piece being held indefinitely if key is held down
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            moveVertical = false;
            verticalTimer = 0;
            buttonDownWaitTimerVertical = 0;
        }

        if (Input.GetKey(KeyCode.RightArrow)||Input.GetAxis("dPad") > 0) //dpad set in Unity and refers to position of left stick
        {
            moveRight();
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetAxis("dPad") < 0) //dpad set in Unity and refers to position of left stick
        {
            moveLeft();
        }

        //calls moveDown method if down pressed or enough time elapsed since it was last auto moved down by 1
        if (Input.GetKey(KeyCode.DownArrow) || (Time.time - lastFall >= fallSpeed) || Input.GetAxis("dPad Down") < 0)
        {
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetAxis("dPad Down") < 0)
            {
                playMoveAudio();
            }
            moveDown();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotate();
        }

        if(Input.GetAxisRaw("dPad Down") > 0)
        {
            //only rotates piece one instead of constantly. requires more than one press to rotate more than once
            if (vAxisInUse == false)
            {
                rotate();
                vAxisInUse = true;
            }
        }

        if (Input.GetAxisRaw("dPad Down") == 0)
        {
            vAxisInUse = false;
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift)) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.JoystickButton3))//joystick3 refers to Y on controller
        {
            storePiece();
        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))//joystickbutton7 refers to start on controller
        {
            pauseGame();
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))//button0 refers to A on controller
        {
            hardfall();
        }

        //android controls. Controls not system dependent at the moment as android requires code from windows code above
        //checks that user touched screen
        if (Input.touchCount > 0)
        {
            //if screen is double tapped call storePiece
            Touch touch = Input.GetTouch(0);
            foreach (Touch test in Input.touches)
            {
                if (test.tapCount == 2)
                {
                    storePiece();
                }
            }
            //if user is touching save original position
            if (touch.phase == TouchPhase.Began)
            {
                previousUnitPosition = new Vector2(touch.position.x, touch.position.y);
            }
            //if user swipes screen
            else if (touch.phase == TouchPhase.Moved)
            {
                //directin of swipe
                Vector2 touchDeltaPosition = touch.deltaPosition;
                direction = touchDeltaPosition.normalized;

                //checks direction and position of swipe and then moves left, right or down
                //position for future development to improve android controls
                if (Mathf.Abs(touch.position.x - previousUnitPosition.x) >= touchSensitivityHorizontal && direction.x < 0 && touch.deltaPosition.y > -10
                    && touch.deltaPosition.y < 10)
                {
                    moveLeft();
                    //moves piece to swiped position
                    previousUnitPosition = touch.position;
                    moved = true;
                }
                else if (Mathf.Abs(touch.position.x - previousUnitPosition.x) >= touchSensitivityHorizontal && direction.x > 0 && touch.deltaPosition.y > -10
                    && touch.deltaPosition.y < 10)
                {
                    moveRight();
                    previousUnitPosition = touch.position;
                    moved = true;
                }
                else if (Mathf.Abs(touch.position.y - previousUnitPosition.y) >= touchSensitivityVertical && direction.y < 0 && touch.deltaPosition.x > -10
                    && touch.deltaPosition.x < 10)
                {
                    moveDown();
                    previousUnitPosition = touch.position;
                    moved = true;
                    playMoveAudio();
                }
            }
            //if single tap instead of swiping
            else if (touch.phase == TouchPhase.Ended)
            {
                //stops rotating and moving happening together
                if (!moved)
                {
                    rotate();
                }
                moved = false;
            }
        }
    }

    void moveRight()
    {
        //allows for holding of key to move piece
        if (moveHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }
            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveHorizontal)
        {
            moveHorizontal = true;
        }

        horizontalTimer = 0;
        //moves piece by one
        transform.position += new Vector3(1, 0, 0);
        //checks that piece is inside grid
        if (!isValidPosition())
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        else//if validPosition then updateGrid called in grid script, passing position
        {
            FindObjectOfType<Grid>().updateGrid(this);
            playMoveAudio();
        }
    }

    //same as moveRight
    void moveLeft()
    { 
        if (moveHorizontal)
        {
            if (buttonDownWaitTimerHorizontal < buttonDownWaitMax)
            {
                buttonDownWaitTimerHorizontal += Time.deltaTime;
                return;
            }
            if (horizontalTimer < continuousHorizontalSpeed)
            {
                horizontalTimer += Time.deltaTime;
                return;
            }
        }

        if (!moveHorizontal)
        {
            moveHorizontal = true;
        }

        horizontalTimer = 0;
        transform.position += new Vector3(-1, 0, 0);
        if (!isValidPosition())
        {
            transform.position += new Vector3(1, 0, 0);
        }
        else
        {
            FindObjectOfType<Grid>().updateGrid(this);
            playMoveAudio();
        }
    }

    //same as moveRight
    void moveDown()
    {
        if (moveVertical)
        {
            if (buttonDownWaitTimerVertical < buttonDownWaitMax)
            {
                buttonDownWaitTimerVertical += Time.deltaTime;
                return;
            }
            if (verticalTimer < continuousDownSpeed)
            {
                verticalTimer += Time.deltaTime;
                return;
            }
        }
        if (!moveVertical)
        {
            moveVertical = true;
        }

        verticalTimer = 0;
        transform.position += new Vector3(0, -1, 0);

        if (!isValidPosition())
        {
            transform.position += new Vector3(0, 1, 0);
            FindObjectOfType<Grid>().deleteRows();
            if (!FindObjectOfType<Grid>().canSpawnPiece(this))
            {
                FindObjectOfType<Grid>().gameOver();
            }
            enabled = false;
            GameObject.FindGameObjectWithTag("currentActivePiece").tag = "setPiece";
            if(sceneName=="ExtremeLevel")
            {
                gameObject.transform.localPosition = new Vector2(10000, 100000);
            }

            FindObjectOfType<Spawner>().spawnNext();
            //playLandAudio();
        }
        else
        {
            FindObjectOfType<Grid>().updateGrid(this);
        }
        lastFall = Time.time;
    }

    void rotate()
    {
        //checks to see if current piece is allowed to rotate, square is not
        if (allowRotation)
        {
            //if it can rotate, can it rotate freely, T can freely rotate
            //if free rotation then rotate in one direction, else rotate clockwise then anti clockwise
            //stops odd rotation happening
            if (limitRotation)
            {
                if (transform.rotation.eulerAngles.z >= 90)
                {
                    transform.Rotate(0, 0, -90);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }

            //stops piece rotating outside of grid. does limit pieces if near edges however
            if (!isValidPosition())
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, -90);
                }
            }
            //if valid grid updated with pieces new position
            else
            {
                FindObjectOfType<Grid>().updateGrid(this);
                playRotateAudio();
            }
        }
    }

    void storePiece()
    {
        //if storing enabled then piece stored as long as it is within the grid when shift pressed
        int x = PlayerPrefs.GetInt("storePieceValue");
        if(x == 1)
        {
            if (isValidPosition())
            {
                foreach (Transform piece in transform)
                {
                    //passes the position of the piece that was saved to remove it from grid
                    Vector2 pos = FindObjectOfType<Grid>().round(piece.position);
                    FindObjectOfType<Grid>().removeSavedPiece(pos.x, pos.y);
                }
                FindObjectOfType<Spawner>().savePiece();
                playStoreAudio();
            }
        }
    }

    void pauseGame()
    {
        FindObjectOfType<pauseMenu>().loadMenu();
        FindObjectOfType<Spawner>().pause();
    }

    void hardfall()
    {
        //moves piece to bottom of grid
        while (isValidPosition())
        {
            transform.position += new Vector3(0, -1, 0);
            
        }
        if (!isValidPosition())
        {
            //stops piece falling out of grid
            transform.position += new Vector3(0, 1, 0);
            
            //checks to see if new piece can spawn with passing current pieces position
            if (!FindObjectOfType<Grid>().canSpawnPiece(this))
            {
                FindObjectOfType<Grid>().gameOver();
            }
            enabled = false;
            GameObject.FindGameObjectWithTag("currentActivePiece").tag = "setPiece";

            FindObjectOfType<Spawner>().spawnNext();
        }
        //grid updated with pieces new position
        FindObjectOfType<Grid>().updateGrid(this);
        FindObjectOfType<Grid>().deleteRows();

        //moves the graphics off screen if hardfall in extreme level
        if (sceneName == "ExtremeLevel")
        {
            gameObject.transform.localPosition = new Vector2(10000, 100000);
        }
    }

    //checks to see piece is inside grid
    bool isValidPosition()
    {
        foreach (Transform piece in transform)
        {
            Vector2 pos = FindObjectOfType<Grid>().round(piece.position);
            if (FindObjectOfType<Grid>().isInsideGrid(pos) == false)
            {
                return false;
            }
            if ((FindObjectOfType<Grid>().getTransformGridPosition(pos) != null) && (FindObjectOfType<Grid>().getTransformGridPosition(pos).parent != transform))
            {
                return false;
            }
        }
        return true;
    }

    void playMoveAudio()
    {
        audioSource.PlayOneShot(moveSound);
    }
    void playRotateAudio()
    {
        audioSource.PlayOneShot(rotateSound);
    }
    void playLandAudio()
    {
        audioSource.PlayOneShot(landSound);
    }
    void playStoreAudio()
    {
        audioSource.PlayOneShot(storeSound);
    }

    //if mute is enabled then sounds isn't played
    public void checkMute()
    {
        if (PlayerPrefs.GetInt("mute") == 1)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
    }
}
