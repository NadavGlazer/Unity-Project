using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeaderBoardCS : MonoBehaviour
{
    public Text place1;
    public Text place2;
    public Text place3;
    public Text place4;
    public Text place5;
    public Text place6;
    public Text place7;
    public Text place8;
    public Text place9;
    public Text place10;
    public Text bestScore;
    // Start is called before the first frame update
    void Start()
    {
        SetBoldPlayerScores();
        place1.text = "1. name: " + AuthScript.leaderBoards[0].GetName().ToString() + " score: " + AuthScript.leaderBoards[0].GetScore();
        place2.text = "2. name: " + AuthScript.leaderBoards[1].GetName().ToString() + " score: " + AuthScript.leaderBoards[1].GetScore();
        place3.text = "3. name: " + AuthScript.leaderBoards[2].GetName().ToString() + " score: " + AuthScript.leaderBoards[2].GetScore();
        place4.text = "4. name: " + AuthScript.leaderBoards[3].GetName().ToString() + " score: " + AuthScript.leaderBoards[3].GetScore();
        place5.text = "5. name: " + AuthScript.leaderBoards[4].GetName().ToString() + " score: " + AuthScript.leaderBoards[4].GetScore();
        place6.text = "6. name: " + AuthScript.leaderBoards[5].GetName().ToString() + " score: " + AuthScript.leaderBoards[5].GetScore();
        place7.text = "7. name: " + AuthScript.leaderBoards[6].GetName().ToString() + " score: " + AuthScript.leaderBoards[6].GetScore();
        place8.text = "8. name: " + AuthScript.leaderBoards[7].GetName().ToString() + " score: " + AuthScript.leaderBoards[7].GetScore();
        place9.text = "9. name: " + AuthScript.leaderBoards[8].GetName().ToString() + " score: " + AuthScript.leaderBoards[8].GetScore();
        place10.text = "10. name: " + AuthScript.leaderBoards[9].GetName().ToString() + " score: " + AuthScript.leaderBoards[9].GetScore();

        bestScore.text = "score: " + AuthScript.instance.GetUser().GetBestScore();
    }
    void GoToShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Shop");
    }
    void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
    void SetBoldPlayerScores()
    {
        if (AuthScript.leaderBoards[0].GetId() == AuthScript.instance.GetUserId())
        {
            place1.fontStyle = FontStyle.Bold;
        }
        else
        {
            place1.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[1].GetId() == AuthScript.instance.GetUserId())
        {
            place2.fontStyle = FontStyle.Bold;
        }
        else
        {
            place2.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[2].GetId() == AuthScript.instance.GetUserId())
        {
            place3.fontStyle = FontStyle.Bold;
        }
        else
        {
            place3.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[3].GetId() == AuthScript.instance.GetUserId())
        {
            place4.fontStyle = FontStyle.Bold;
        }
        else
        {
            place4.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[4].GetId() == AuthScript.instance.GetUserId())
        {
            place5.fontStyle = FontStyle.Bold;
        }
        else
        {
            place5.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[5].GetId() == AuthScript.instance.GetUserId())
        {
            place6.fontStyle = FontStyle.Bold;
        }
        else
        {
            place6.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[6].GetId() == AuthScript.instance.GetUserId())
        {
            place7.fontStyle = FontStyle.Bold;
        }
        else
        {
            place7.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[7].GetId() == AuthScript.instance.GetUserId())
        {
            place8.fontStyle = FontStyle.Bold;
        }
        else
        {
            place8.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[8].GetId() == AuthScript.instance.GetUserId())
        {
            place9.fontStyle = FontStyle.Bold;
        }
        else
        {
            place9.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.leaderBoards[9].GetId() == AuthScript.instance.GetUserId())
        {
            place10.fontStyle = FontStyle.Bold;
        }
        else
        {
            place10.fontStyle = FontStyle.Normal;
        }
    }
}
