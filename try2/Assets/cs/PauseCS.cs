using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCS : MonoBehaviour
{
    public GameObject gamePanelUI;
    public GameObject pauseMenuUI;
    float pauseTimeScale;
    //Pausing the game
    void Start()
    {
        pauseTimeScale = 0.0001f;
    }
    void Pause()
    {
        if (!CollCheck.HasLost)
        {
            gamePanelUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            Time.timeScale = pauseTimeScale;
            PauseMenu.IsPaused = true;
        }
    }
}
