using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DespawnOnHeight : MonoBehaviour
{
    [SerializeField] [Range(-5, 0f)] private int minHeight = -1;

    private bool despawned = false;

    private void Start()
    {
        despawned = false;
    }

    void Update()
    {
        if (!despawned && transform.position.y < minHeight)
        {
            despawned = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        Debug.Log("GAME OVER");
        Destroy(GameObject.Find("WhisperSource"));
        MazeCount.CurrentMaze = 1;
        SceneManager.LoadScene("GameOver");
    }
}
