using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity;

public class DeathButton : MonoBehaviour
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
        if (Animations.deathAnimationFinished && first)
        {
            int temp = (int)Math.Round(SetUp.totalRun / 10 + UnityEngine.Random.Range(-SetUp.totalRun / 15, SetUp.totalRun / 15));
            AuthScript.instance.GetUser().ChangeCoins(temp);

            int score = (int)Math.Round(SetUp.totalRun);
            scoreText.text = textUI[0] + score;
            coinText.text = textUI[1] + temp;
            totalCoinsText.text = textUI[2] + AuthScript.instance.GetUser().GetCoins();

            if (score > AuthScript.instance.GetUser().GetBestScore())
            {
                AuthScript.instance.GetUser().SetBestScore(score);
            }

            UpdateUser(AuthScript.instance.GetUser());
            UpdateLeaderBoard(score, AuthScript.instance.GetUser().GetName());

            first = false;
        }
    }
    //starting new game
    public void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
        //Application.LoadLevel(Application.loadedLevel);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void UpdateUser(User user)
    {
        string json = JsonUtility.ToJson(user);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthScript.instance.GetUserId()).SetRawJsonValueAsync(json);
    }
    void UpdateLeaderBoard(int newScore, string newName)
    {
        if (newScore < AuthScript.leaderBoards[9].GetScore())
        {
            return;
        }
        else
        {
            AuthScript.leaderBoards[9].SetScore(newScore);
            AuthScript.leaderBoards[9].SetName(newName);
            AuthScript.leaderBoards[9].SetId(AuthScript.instance.GetUserId());
        }
        LeaderBoard temp;
        for (int i = 0; i < AuthScript.leaderBoards.Length - 1; i++)
        {
            for (int j = i + 1; j < AuthScript.leaderBoards.Length; j++)
            {
                if (AuthScript.leaderBoards[i].GetScore() < AuthScript.leaderBoards[j].GetScore())
                {
                    temp = new LeaderBoard(AuthScript.leaderBoards[i]);
                    AuthScript.leaderBoards[i] = new LeaderBoard(AuthScript.leaderBoards[j]);
                    AuthScript.leaderBoards[j] = new LeaderBoard(temp);
                }
            }
        }
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("1").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[0]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("2").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[1]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("3").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[2]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("4").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[3]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("5").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[4]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("6").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[5]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("7").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[6]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("8").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[7]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("9").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[8]));
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child("10").SetRawJsonValueAsync(JsonUtility.ToJson(AuthScript.leaderBoards[9]));
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
