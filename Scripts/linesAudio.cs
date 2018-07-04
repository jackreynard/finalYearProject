using UnityEngine;
using System.Collections;

public class linesAudio : MonoBehaviour {

    public AudioClip clearedOneLine;
    public AudioClip clearedTwoLines;
    public AudioClip clearedThreeLines;
    public AudioClip clearedFourLines;
    public AudioClip gameOverSound;
    private AudioSource audioSource;

    // Use this for initialization
    void Start () {
        //assigns the audioSource variable to the object AudioSource
        audioSource = GetComponent<AudioSource>();
    }


    //The audio played depending on what has just happened within the game 
    public void oneLine()
    {
        audioSource.PlayOneShot(clearedOneLine);
    }
    public void twoLines()
    {
        audioSource.PlayOneShot(clearedTwoLines);
    }
    public void threeLines()
    {
        audioSource.PlayOneShot(clearedThreeLines);
    }
    public void fourLines()
    {
        audioSource.PlayOneShot(clearedFourLines);
    }
    public void gameOver()
    {
        audioSource.PlayOneShot(gameOverSound);
    }
}
