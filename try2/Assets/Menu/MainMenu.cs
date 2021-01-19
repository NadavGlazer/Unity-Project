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
    public Text coinsTxt;
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    // Start is called before the first frame update
    void Start()
    {
        coinsTxt.text = "coins: " + AuthScript.Instance.GetUser().GetCoins();
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
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }
    void GoToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }
    void LogOut()
    {
        Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();
        SceneManager.LoadScene("Authentication");
    }
}
