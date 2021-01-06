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
        place1.text = "1. name: " + AuthScript.leaderBoards[0].name.ToString() + " score: " + AuthScript.leaderBoards[0].score;
        place2.text = "2. name: " + AuthScript.leaderBoards[1].name.ToString() + " score: " + AuthScript.leaderBoards[1].score;
        place3.text = "3. name: " + AuthScript.leaderBoards[2].name.ToString() + " score: " + AuthScript.leaderBoards[2].score;
        place4.text = "4. name: " + AuthScript.leaderBoards[3].name.ToString() + " score: " + AuthScript.leaderBoards[3].score;
        place5.text = "5. name: " + AuthScript.leaderBoards[4].name.ToString() + " score: " + AuthScript.leaderBoards[4].score;
        place6.text = "6. name: " + AuthScript.leaderBoards[5].name.ToString() + " score: " + AuthScript.leaderBoards[5].score;
        place7.text = "7. name: " + AuthScript.leaderBoards[6].name.ToString() + " score: " + AuthScript.leaderBoards[6].score;
        place8.text = "8. name: " + AuthScript.leaderBoards[7].name.ToString() + " score: " + AuthScript.leaderBoards[7].score;
        place9.text = "9. name: " + AuthScript.leaderBoards[8].name.ToString() + " score: " + AuthScript.leaderBoards[8].score;
        place10.text = "10. name: " + AuthScript.leaderBoards[9].name.ToString() + " score: " + AuthScript.leaderBoards[9].score;

        bestScore.text = "score: " + AuthScript.instance.user.bestScore;
    }
    public void GoToShop()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Shop");
    }
    public void Play()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
