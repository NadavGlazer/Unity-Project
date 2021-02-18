using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity;

public class MainMenu : MonoBehaviour
{
    public Text CoinsTxt;
    public GameObject MainMenuPanel;
    public GameObject SettingsPanel;
    public GameObject MassageText;
    public Text NameText;
    float timerCount;
    // Start is called before the first frame update
    void Start()
    {
        if (Time.timeScale == 1.1f)
        {
            MassageText.GetComponent<Text>().text = "Hello " + AuthScript.Instance.GetUser().GetName().ToString();
            timerCount = Time.time + 3f;
        }
        else
        {
            timerCount = Time.time;
        }
        Time.timeScale = 1f;
        CoinsTxt.text = "coins: " + AuthScript.Instance.GetUser().GetCoins();
        NameText.text = AuthScript.Instance.GetUser().GetName().ToString();
    }
    void Update()
    {
        if (timerCount < Time.time)
        {
            MassageText.SetActive(false);
        }
    }
    // Update is called once per frame

    void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }
    void GoToGame()
    {
        //ChangeColor.temp.AddRange(new List<int>() { GetUser.FirstColor[0], GetUser.FirstColor[1], GetUser.FirstColor[2], GetUser.FirstColor[3] });
        SceneManager.LoadScene("Game");
    }
    void GoToLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }
    void GoToSetting()
    {
        MainMenuPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    void GoToMainMenu()
    {
        MainMenuPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
    void LogOut()
    {
        Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene("Authentication");
    }
}
