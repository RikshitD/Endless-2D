using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauser : MonoBehaviour
{
    public GameObject pausePanel;
    public Text pausedText;

    private bool isPaused = false;

    void Start()
    {
        pausePanel.SetActive(false);
        pausedText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else if (!pausePanel.activeSelf)
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0f; 
        isPaused = true;
        pausePanel.SetActive(true);
        pausedText.gameObject.SetActive(true);
        AudioListener.pause = true; 
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        pausePanel.SetActive(false);
        pausedText.gameObject.SetActive(false);
        AudioListener.pause = false;
    }
}
