using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused, isFirst;
    public GameObject pauseMenuUI;
    public GameObject GamePanelUI;
    public GameObject ResumeCountDownPanel;
    public GameObject BeforeStartPanel;
    float countDown, countGo;
    bool startCount, isGO;
    Text CountDownText;
    Transform TextTR;
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
        ResumeCountDownPanel.SetActive(true);
        pauseMenuUI.SetActive(false);
        GamePanelUI.SetActive(true);
    }
    //Pausing the game
    void Pause()
    {
        if (!CollCheck.HasLost)
        {
            pauseMenuUI.SetActive(true);
            GamePanelUI.SetActive(false);
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
                CountDownText.text = "" + Math.Round(10000 * (countDown - Time.time) - 1);
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
                CountDownText.text = goText;
            }
            else
            {
                ResumeCountDownPanel.SetActive(false);
                startCount = false;
                isGO = false;
            }
            Time.timeScale = 1f;
            IsPaused = false;
        }
    }
    void UpdateVer()
    {
        IsPaused = isFirst = false;
        countDown = countGo = 0.0f;
        startCount = isGO = false;
        TextTR = ResumeCountDownPanel.transform.Find("ResumeCountDown");
        CountDownText = TextTR.GetComponent<Text>();
        goText = "GO!";
    }
    void Shop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Shop");
    }
    void GoToLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }

}
