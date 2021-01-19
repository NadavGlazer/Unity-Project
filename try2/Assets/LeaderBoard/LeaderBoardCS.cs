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
    public static bool HasChanged;
    // Start is called before the first frame update
    void Start()
    {
        SetBoldPlayerScores();
        SetLeaderBoards();

        bestScore.text = "score: " + AuthScript.Instance.GetUser().GetBestScore();
        HasChanged = false;
    }
    void SetLeaderBoards()
    {
        place1.text = "1. name: " + AuthScript.LeaderBoards[0].GetName().ToString() + " score: " + AuthScript.LeaderBoards[0].GetScore();
        place2.text = "2. name: " + AuthScript.LeaderBoards[1].GetName().ToString() + " score: " + AuthScript.LeaderBoards[1].GetScore();
        place3.text = "3. name: " + AuthScript.LeaderBoards[2].GetName().ToString() + " score: " + AuthScript.LeaderBoards[2].GetScore();
        place4.text = "4. name: " + AuthScript.LeaderBoards[3].GetName().ToString() + " score: " + AuthScript.LeaderBoards[3].GetScore();
        place5.text = "5. name: " + AuthScript.LeaderBoards[4].GetName().ToString() + " score: " + AuthScript.LeaderBoards[4].GetScore();
        place6.text = "6. name: " + AuthScript.LeaderBoards[5].GetName().ToString() + " score: " + AuthScript.LeaderBoards[5].GetScore();
        place7.text = "7. name: " + AuthScript.LeaderBoards[6].GetName().ToString() + " score: " + AuthScript.LeaderBoards[6].GetScore();
        place8.text = "8. name: " + AuthScript.LeaderBoards[7].GetName().ToString() + " score: " + AuthScript.LeaderBoards[7].GetScore();
        place9.text = "9. name: " + AuthScript.LeaderBoards[8].GetName().ToString() + " score: " + AuthScript.LeaderBoards[8].GetScore();
        place10.text = "10. name: " + AuthScript.LeaderBoards[9].GetName().ToString() + " score: " + AuthScript.LeaderBoards[9].GetScore();
    }
    void Update()
    {
        if (HasChanged)
        {
            SetLeaderBoards();
            HasChanged = false;
        }
    }
    void GoToShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Shop");
    }
    void Play()
    {
        SceneManager.LoadScene("Game");
    }
    void SetBoldPlayerScores()
    {
        if (AuthScript.LeaderBoards[0].GetId() == AuthScript.Instance.GetUserId())
        {
            place1.fontStyle = FontStyle.Bold;
        }
        else
        {
            place1.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[1].GetId() == AuthScript.Instance.GetUserId())
        {
            place2.fontStyle = FontStyle.Bold;
        }
        else
        {
            place2.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[2].GetId() == AuthScript.Instance.GetUserId())
        {
            place3.fontStyle = FontStyle.Bold;
        }
        else
        {
            place3.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[3].GetId() == AuthScript.Instance.GetUserId())
        {
            place4.fontStyle = FontStyle.Bold;
        }
        else
        {
            place4.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[4].GetId() == AuthScript.Instance.GetUserId())
        {
            place5.fontStyle = FontStyle.Bold;
        }
        else
        {
            place5.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[5].GetId() == AuthScript.Instance.GetUserId())
        {
            place6.fontStyle = FontStyle.Bold;
        }
        else
        {
            place6.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[6].GetId() == AuthScript.Instance.GetUserId())
        {
            place7.fontStyle = FontStyle.Bold;
        }
        else
        {
            place7.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[7].GetId() == AuthScript.Instance.GetUserId())
        {
            place8.fontStyle = FontStyle.Bold;
        }
        else
        {
            place8.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[8].GetId() == AuthScript.Instance.GetUserId())
        {
            place9.fontStyle = FontStyle.Bold;
        }
        else
        {
            place9.fontStyle = FontStyle.Normal;
        }

        if (AuthScript.LeaderBoards[9].GetId() == AuthScript.Instance.GetUserId())
        {
            place10.fontStyle = FontStyle.Bold;
        }
        else
        {
            place10.fontStyle = FontStyle.Normal;
        }
    }
}
