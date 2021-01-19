using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity;

public class OnDeath : MonoBehaviour
{
    public Text scoreText;
    public Text coinText;
    public Text totalCoinsText;
    bool first;
    string[] textUI;
    void Start()
    {
        UpdateVer();
    }
    void Update()
    {
        if (Animations.DeathAnimationFinished && first)
        {
            int temp = (int)Math.Round(SetUp.TotalRun / 10 + UnityEngine.Random.Range(-SetUp.TotalRun / 15, SetUp.TotalRun / 15));
            AuthScript.Instance.GetUser().ChangeCoins(temp);

            int score = (int)Math.Round(SetUp.TotalRun);
            scoreText.text = textUI[0] + score;
            coinText.text = textUI[1] + temp;
            totalCoinsText.text = textUI[2] + AuthScript.Instance.GetUser().GetCoins();

            if (score > AuthScript.Instance.GetUser().GetBestScore())
            {
                AuthScript.Instance.GetUser().SetBestScore(score);
            }

            UpdateUser(AuthScript.Instance.GetUser());
            UpdateLeaderBoard(score, AuthScript.Instance.GetUser().GetName());

            first = false;
        }
    }
    //starting new game
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
        //Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void UpdateUser(User user)
    {
        string json = JsonUtility.ToJson(user);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthScript.Instance.GetUserId()).SetRawJsonValueAsync(json);
    }
    void UpdateLeaderBoard(int newScore, string newName)
    {
        if (newScore < AuthScript.LeaderBoards[9].GetScore())
        {
            return;
        }
        else
        {
            AuthScript.LeaderBoards[9].SetScore(newScore);
            AuthScript.LeaderBoards[9].SetName(newName);
            AuthScript.LeaderBoards[9].SetId(AuthScript.Instance.GetUserId());
        }
        LeaderBoard temp;
        for (int i = 0; i < AuthScript.LeaderBoards.Length - 1; i++)
        {
            for (int j = i + 1; j < AuthScript.LeaderBoards.Length; j++)
            {
                if (AuthScript.LeaderBoards[i].GetScore() < AuthScript.LeaderBoards[j].GetScore())
                {
                    temp = new LeaderBoard(AuthScript.LeaderBoards[i]);
                    AuthScript.LeaderBoards[i] = new LeaderBoard(AuthScript.LeaderBoards[j]);
                    AuthScript.LeaderBoards[j] = new LeaderBoard(temp);
                }
            }
        }
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("1").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[0]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("2").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[1]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("3").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[2]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("4").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[3]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("5").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[4]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("6").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[5]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("7").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[6]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("8").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[7]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("9").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[8]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("10").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.LeaderBoards[9]));
    }
    void UpdateVer()
    {
        first = true;
        textUI = new string[3];
        textUI[0] = "score: ";
        textUI[1] = "coins earned: ";
        textUI[2] = "coins: ";
    }

}
