using UnityEngine;
using System.Collections;

public class ghostPiece : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //assigns ghost piece tag to identify it
        tag = "currentGhostPiece";
        //sets colour of tetromino to more translucent
        foreach (Transform pieces in transform)
        {
            pieces.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, .2f);
        }
	}

    // Update is called once per frame
    void Update()
    {
        followActivePiece();
        moveDown();
    }

    //follows the movements and rotations of the actual tetromino the user is controlling
    void followActivePiece()
    {
        Transform currentActivePieceTransform = GameObject.FindGameObjectWithTag("currentActivePiece").transform;
        transform.position = currentActivePieceTransform.position;
        transform.rotation = currentActivePieceTransform.rotation;
    }

    //keeps ghost in grid
    bool isValidPosition()
    {
        foreach (Transform piece in transform)
        {
            Vector2 pos = FindObjectOfType<Grid>().round(piece.position);
            if (FindObjectOfType<Grid>().isInsideGrid(pos) == false)
            {
                return false;
            }
            if ((FindObjectOfType<Grid>().getTransformGridPosition(pos) != null) && (FindObjectOfType<Grid>().getTransformGridPosition(pos).parent.tag == "currentActivePiece"))
            {
                return true;
            }
            if ((FindObjectOfType<Grid>().getTransformGridPosition(pos) != null) && (FindObjectOfType<Grid>().getTransformGridPosition(pos).parent != transform))
            {
                return false;
            }
        }
        return true;
    }

    //moves ghost to bottom of grid when it spawns 
    void moveDown()
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
}
