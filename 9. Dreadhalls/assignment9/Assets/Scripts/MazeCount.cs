using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeCount : MonoBehaviour
{
    private Text currentMazeText;
    public Text CurrentMazeText
    {
        get
        {
            if (currentMazeText == null)
                currentMazeText = GameObject.Find("MazeText").GetComponent<Text>();
            return currentMazeText;
        }
    }
    private static int currentMaze = 1;
    public static int CurrentMaze
    {
        set
        {
            currentMaze = value;
            instance.updateMazeText();
        }
        get { return currentMaze; }
    }

    // make this static so it's visible across all instances
    public static MazeCount instance = null;

    private void updateMazeText()
    {
        CurrentMazeText.text = "Maze: " + currentMaze;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        updateMazeText();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
