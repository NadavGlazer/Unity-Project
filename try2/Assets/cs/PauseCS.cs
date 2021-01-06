using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCS : MonoBehaviour
{
    public GameObject GamePanelUI;
    public GameObject pauseMenuUI;
    //Pausing the game
    public void Pause()
    {
        if (!CollCheck.HasLost)
        {
            GamePanelUI.SetActive(false);
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0.0001f;
            PauseMenu.IsPaused = true;
        }
    }
}
