using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused;
    public static bool IsFirst;
    public GameObject pauseMenuUI;
    public GameObject gamePanelUI;
    public GameObject resumeCountDownPanel;
    public Text countDownText;
    float countDown;
    float countGo;
    bool startCount;
    bool isGO;
    string goText;
    // Update is called once per frame
    void Start()
    {
        UpdateVer();
    }
    void Update()
    {
        UpdatingPauseCountDown();
    }
    //Resuming the game
    void StartResumeCount()
    {
        countDown = (float)Math.Round(Time.time, 4) + 0.0004f - (int)(Time.time % 0.001);
        startCount = true;
        resumeCountDownPanel.SetActive(true);
        pauseMenuUI.SetActive(false);
        gamePanelUI.SetActive(true);
    }
    //Pausing the game
    void Pause()
    {
        if (!CollCheck.HasLost)
        {
            pauseMenuUI.SetActive(true);
            gamePanelUI.SetActive(false);
            Time.timeScale = 0.0001f;
            IsPaused = true;
        }
    }
    void UpdatingPauseCountDown()
    {
        if (startCount)
        {
            if (Math.Round(10000 * (countDown - Time.time)) >= 1)
            {
                countDownText.text = "" + Math.Round(10000 * (countDown - Time.time) - 1);
            }
        }
        if (startCount && Math.Round(10000 * (countDown - Time.time)) <= 1)
        {
            if (!isGO)
            {
                countGo = Time.time + 1.5f;
                isGO = true;
            }

            if (Time.time <= countGo)
            {
                countDownText.text = goText;
            }
            else
            {
                resumeCountDownPanel.SetActive(false);
                startCount = false;
                isGO = false;
            }
            Time.timeScale = 1f;
            IsPaused = false;
        }
    }
    void UpdateVer()
    {
        IsPaused = false;
        countDown = countGo = 0.0f;
        startCount = isGO = false;
        goText = "GO!";
    }
    void GoToShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Shop");
    }
    void GoToLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

}
