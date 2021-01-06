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
    void Start()
    {
        first = true;
    }
    void Update()
    {
        if (Animations.booliann && first)
        {
            int temp = (int)Math.Round(SetUp.totalRun / 10 + UnityEngine.Random.Range(-SetUp.totalRun / 15, SetUp.totalRun / 15));
            AuthScript.instance.user.coins += temp;
            int score = (int)Math.Round(SetUp.totalRun);
            scoreText.text = "score: " + score;
            coinText.text = "coins earned: " + temp;
            totalCoinsText.text = "coins: " + AuthScript.instance.user.coins;
            first = false;
            if (score > AuthScript.instance.user.bestScore)
            {
                AuthScript.instance.user.bestScore = score;
            }
            UpdateUser(AuthScript.instance.user);
            UpdateLeaderBoard(score, AuthScript.instance.user.name);
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
    private void UpdateUser(User user)
    {
        string json = JsonUtility.ToJson(user);
        FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthScript.instance.userID).SetRawJsonValueAsync(json);
    }
    void UpdateLeaderBoard(int newLead, string newLeads)
    {
        if (newLead < AuthScript.leaderBoards[9].score)
        {
            return;
        }
        else
        {
            AuthScript.leaderBoards[9].score = newLead;
            AuthScript.leaderBoards[9].name = newLeads;
        }
        LeaderBoard temp;
        for (int i = 0; i < AuthScript.leaderBoards.Length - 1; i++)
        {
            for (int j = i + 1; j < AuthScript.leaderBoards.Length; j++)
            {
                if (AuthScript.leaderBoards[i].score < AuthScript.leaderBoards[j].score)
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
}
