using UnityEngine;
using System.Collections;

public class destroyPiece : MonoBehaviour {

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


    // Use this for initialization
    void Start ()
    {
        moveAutoDown();
    }
	
	// Update is called once per frame
	void Update ()
    {
        userInput();
    }

    //used to check the keys pressed by the user and call the corresponding methods
    void userInput()
    {
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            moveHorizontal = false;
            horizontalTimer = 0;
            buttonDownWaitTimerHorizontal = 0;
        }

        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            moveVertical = false;
            verticalTimer = 0;
            buttonDownWaitTimerVertical = 0;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetAxis("analogue horizontal") > 0) //analogue horizontal set in Unity and refers to controller right stick. 0 is middle and 
                                                                                    // more than 0 is right
        {
            moveRight();
        }

        if (Input.GetKey(KeyCode.A) || Input.GetAxis("analogue horizontal") < 0)
        {
            moveLeft();
        }

        if (Input.GetKey(KeyCode.S) || Input.GetAxis("analogue vertical") > 0) //analogue vertical set in Unity and refers to controller right stick. 0 is middle and 
                                                                                //greater than refers to down
        {
            moveDown();
        }
        if (Input.GetKey(KeyCode.W) || Input.GetAxis("analogue vertical") < 0)
        {
            moveUp();
        }


        if (Input.GetKey(KeyCode.F) || Input.GetKeyDown(KeyCode.JoystickButton1))//joystick button 1 refers to B on controller
        {
            destroyPieces();
            Spawner.bombCount = 0;
            Spawner.bombReady = false;
            FindObjectOfType<Spawner>().destroyPiece();
        }
    }
   
    //moves bomb to bottom of grid
    void moveAutoDown()
    {
        while (isValidPosition())
        {
            transform.position += new Vector3(0, -1, 0);
        }
        if (!isValidPosition())
        {
            transform.position += new Vector3(0, 1, 0);
        }
    }

    //moves bomb right
    void moveRight()
    {
        //allows for continual holding to move piece instead of repeated presses
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
        //moves piece right by one
        transform.position += new Vector3(1, 0, 0);
        //moves piece back to grid if moved out
        if (!isValidPosition())
        {
            transform.position += new Vector3(-1, 0, 0);
        }
        else
        {
           // FindObjectOfType<Grid>().updateGridDestroyPiece(this);
        }
    }

    //same core code as moveRight except moving piece left
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
            //FindObjectOfType<Grid>().updateGridDestroyPiece(this);
        }
    }

    //same core code as moveRight except moving piece down
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
        }
        else
        {
           // FindObjectOfType<Grid>().updateGridDestroyPiece(this);
        }
    }

    //same core code as moveRight except moving piece up
    void moveUp()
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
        transform.position += new Vector3(0, 1, 0);

        if (!isValidPosition())
        {
            transform.position += new Vector3(0, -1, 0);
        }
        else
        {
            //FindObjectOfType<Grid>().updateGridDestroyPiece(this);
        }
    }

    //finds position of bomb and sends those values to destroyPieces method in grid script
    void destroyPieces()
    {
        foreach (Transform piece in transform)
        {
            Vector2 pos = FindObjectOfType<Grid>().round(piece.position);
            FindObjectOfType<Grid>().destroyPieces(pos);
        }
    }

    //checks to see that bomb is still in grid and keeps it there
    bool isValidPosition()
    {
        foreach (Transform piece in transform)
        {
            Vector2 pos = FindObjectOfType<Grid>().round(piece.position);
            if (FindObjectOfType<Grid>().bombInsideGrid(pos) == false)
            {
                return false;
            }
        }
        return true;
    }
}
