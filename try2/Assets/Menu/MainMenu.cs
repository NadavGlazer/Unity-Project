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
    // Start is called before the first frame update
    void Start()
    {
        coinsTxt.text = "coins: " + AuthScript.instance.user.coins;
    }

    // Update is called once per frame
    
    public void GoToShop()
    {
        SceneManager.LoadScene("Shop");
    }
    public void GoToGame()
    {
        //ChangeColor.temp.AddRange(new List<int>() { GetUser.FirstColor[0], GetUser.FirstColor[1], GetUser.FirstColor[2], GetUser.FirstColor[3] });
        SceneManager.LoadScene("SampleScene");
    }
    public void GoToLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }
}
