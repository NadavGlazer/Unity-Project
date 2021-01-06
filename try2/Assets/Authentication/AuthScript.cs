using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity;

public class AuthScript : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    DatabaseReference reference;
    public InputField email;
    public InputField password;
    public Text errortext;
    bool moveScene;
    public static Singleton instance;
    public static LeaderBoard[] leaderBoards;
    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        moveScene = false;

        leaderBoards = new LeaderBoard[10];
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                leaderBoards[0] = new LeaderBoard(0, "temp");
                while (leaderBoards[0].score == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        leaderBoards[i] = new LeaderBoard(int.Parse(snapshot.Child((i + 1).ToString()).Child("score").Value.ToString()), snapshot.Child((i + 1).ToString()).Child("name").Value.ToString());
                    }
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        errortext.SetAllDirty();
        if (moveScene)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
    public void Register()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text.ToString(), password.text.ToString()).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                errortext.text = "Register isn`t successfully";

                return;
            }
            if (task.IsFaulted)
            {
                errortext.text = "Register isn`t successfully";
                return;
            }

            // Firebase user has been created.
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            reference = reference.Child("Users").Child(auth.CurrentUser.UserId);

            writeNewUser(auth.CurrentUser.UserId, email.text.ToString().Substring(0, email.text.ToString().IndexOf("@")));
            moveScene = true;
        });
    }
    public void SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text.ToString(), password.text.ToString()).ContinueWith(task1 =>
        {
            if (task1.IsCanceled)
            {
                errortext.text = "Sign In isn`t successfully";
                return;
            }
            if (task1.IsFaulted)
            {
                errortext.text = "Sign In isn`t successfully";
                return;
            }
            Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
            reference = reference.Child("Users").Child(auth.CurrentUser.UserId);
            //

            reference.GetValueAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    return;
                }
                List<int> tempCurrent = new List<int>();
                List<int> tempOptional = new List<int>();
                List<int> tempOwned = new List<int>();
                int tempcoins = 0;
                int tempBest = 0;
                string tempName;
                DataSnapshot snapshot = task.Result;
                for (int i = 0; i < 5; i++)
                {
                    tempCurrent.Add(int.Parse(snapshot.Child("CurrentColor").Child(i.ToString()).Value.ToString()));
                }
                for (int i = 0; i < snapshot.Child("OptionalColors").ChildrenCount; i++)
                {
                    tempOptional.Add(int.Parse(snapshot.Child("OptionalColors").Child(i.ToString()).Value.ToString()));
                }
                for (int i = 0; i < snapshot.Child("OwnColors").ChildrenCount; i++)
                {
                    tempOwned.Add(int.Parse(snapshot.Child("OwnColors").Child(i.ToString()).Value.ToString()));
                }
                tempcoins = int.Parse(snapshot.Child("coins").Value.ToString());
                tempBest = int.Parse(snapshot.Child("bestScore").Value.ToString());
                tempName = snapshot.Child("name").Value.ToString();
                User temp = new User(tempcoins, tempOwned, tempCurrent, tempOptional, tempBest, email.text.ToString().Substring(0, email.text.ToString().IndexOf("@")));
                instance = new Singleton(temp, auth.CurrentUser.UserId);
                moveScene = true;
            });
        });
    }
    private void writeNewUser(string id, string name)
    {
        User user = new User(name);
        user.AddColor(new List<int> { 255, 0, 0, 150 });
        user.AddColor(new List<int> { 0, 255, 0, 150 });
        user.AddColor(new List<int> { 0, 0, 255, 150 });
        user.AddColor(new List<int> { 0, 255, 255, 150 });
        user.AddColor(new List<int> { 76, 0, 153, 150 });
        string json = JsonUtility.ToJson(user);
        reference.SetRawJsonValueAsync(json);


        instance = new Singleton(user, id);
    }
}
public class User
{
    public int coins;
    public List<int> OwnColors;
    public List<int> CurrentColor;
    public List<int> OptionalColors;
    public int bestScore;
    public string name;
    public User(string name)
    {
        coins = 200;
        CurrentColor = new List<int> { 25, 108, 133, 255, 1 };
        OwnColors = new List<int> { -1, 25, 108, 133, 255 };
        OptionalColors = new List<int> { -1, 25, 108, 133, 255 };
        bestScore = 0;
        this.name = name;
    }
    public User(int co, List<int> ow, List<int> cu, List<int> op, int best, string name)
    {
        coins = co;
        bestScore = best;
        this.name = name;
        OwnColors = new List<int>();
        CurrentColor = new List<int>();
        OptionalColors = new List<int>();

        OwnColors.AddRange(ow);
        CurrentColor.AddRange(cu);
        OptionalColors.AddRange(op);
    }
    public User(User temp)
    {
        coins = temp.coins;
        name = temp.name;
        bestScore = temp.bestScore;
        OwnColors = new List<int>();
        CurrentColor = new List<int>();
        OptionalColors = new List<int>();

        OwnColors.AddRange(temp.OwnColors);
        CurrentColor.AddRange(temp.CurrentColor);
        OptionalColors.AddRange(temp.OptionalColors);
    }
    public void AddColor(List<int> temp)
    {
        OptionalColors.AddRange(temp);
    }
    public void AddExColor(Color color)
    {
        OptionalColors.AddRange(new List<int> { (int)color.r, (int)color.g, (int)color.b, (int)color.a });
    }
    public void AddToOwn(List<int> temp)
    {
        OwnColors.AddRange(temp);
    }
    public void ChangeCurrent(List<int> temp)
    {
        CurrentColor = new List<int>(temp);
    }
    public List<int> GetCurrent()
    {
        return CurrentColor;
    }
}
public class LeaderBoard
{
    public int score;
    public string name;
    public LeaderBoard(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
    public LeaderBoard(LeaderBoard temp)
    {
        this.score = temp.score;
        this.name = temp.name;
    }
}
public class Singleton
{
    public User user;
    public string userID;
    public Singleton(List<int> current, List<int> optional, List<int> own, int coins, string id, int best, string name)
    {
        user = new User(coins, own, current, optional, best, name);
        userID = id;
    }
    public Singleton(User user, string id)
    {
        this.user = new User(user);
        userID = id;
    }
    public void UpdateCurrent(List<int> current)
    {
        user.CurrentColor.Clear();
        user.CurrentColor.AddRange(current);
    }
    public void UpdateOwned(List<int> owned)
    {
        user.OwnColors.Clear();
        user.OwnColors.AddRange(owned);
    }
    public void UpdateOptional(List<int> optional)
    {
        user.OptionalColors.Clear();
        user.OptionalColors.AddRange(optional);
    }
    public void UpdateCoins(int coins)
    {
        user.coins = coins;
    }
}

