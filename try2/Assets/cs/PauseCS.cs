using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCS : MonoBehaviour
{
    public GameObject GamePanelUI;
    public GameObject pauseMenuUI;
    float pauseTimeScale = 0.0001f;
    //Pausing the game
    void Pause()
    {
        if (!CollCheck.HasLost)
        {
            GamePanelUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            Time.timeScale = pauseTimeScale;
            PauseMenu.IsPaused = true;
        }
    }
}
