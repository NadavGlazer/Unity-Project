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
    public InputField signInEmail;
    public InputField registerEmail;
    public InputField registerPassword;
    public InputField signInPassword;
    public InputField newName;
    public Text errorMessage;
    public GameObject registerP;
    public GameObject signInP;
    bool moveScene;
    public static CurrentUser Instance;
    public static LeaderBoard[] LeaderBoards;
    string signInError;
    string signInSuccess;
    string registerError;
    string registerSuccess;
    string blankInputError;
    // Start is called before the first frame update
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        moveScene = false;
        LeaderBoards = new LeaderBoard[10];
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted)
            {
                return;
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                LeaderBoards[0] = new LeaderBoard(0, "temp", "0");
                while (!CheckIfReadFromDBWasSuccessfull())
                {
                    for (int i = 0; i < 10; i++)
                    {
                        LeaderBoards[i] = new LeaderBoard(int.Parse(snapshot.Child((i + 1).ToString()).Child("Score").Value.ToString()),
                            snapshot.Child((i + 1).ToString()).Child("Name").Value.ToString(),
                            snapshot.Child((i + 1).ToString()).Child("ID").Value.ToString());
                    }
                };
            }
        });
        FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").ValueChanged += HandleValueChanged;
        signInError = "Sign In isn`t successfull";
        signInSuccess = "Signing in";
        registerError = "Register isn`t successfull";
        registerSuccess = "Registering";
        blankInputError = "Input fields cannot be blank";
        errorMessage.text = "";
        if (Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            RememberMe();
        }
    }
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        LeaderBoards[0] = new LeaderBoard(0, "temp", "0");
        while (!CheckIfReadFromDBWasSuccessfull())
        {
            for (int i = 0; i < 10; i++)
            {
                LeaderBoards[i] = new LeaderBoard(int.Parse(args.Snapshot.Child((i + 1).ToString()).Child("Score").Value.ToString()),
                    args.Snapshot.Child((i + 1).ToString()).Child("Name").Value.ToString(),
                    args.Snapshot.Child((i + 1).ToString()).Child("ID").Value.ToString());
            }
        }
        LeaderBoardCS.HasChanged = true;
    }


    // Update is called once per frame
    void Update()
    {
        errorMessage.SetAllDirty();
        errorMessage.SetAllDirty();
        if (moveScene)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    void Register()
    {
        if (!(registerEmail.text.ToString() == "" && registerPassword.text.ToString() == "") && newName.text.ToString() != null)
        {
            auth.CreateUserWithEmailAndPasswordAsync(registerEmail.text.ToString(), registerPassword.text.ToString()).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    errorMessage.text = registerError;

                    return;
                }
                if (task.IsFaulted)
                {
                    errorMessage.text = registerError;
                    return;
                }

                // Firebase user has been created.

                Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                reference = reference.Child("Users").Child(auth.CurrentUser.UserId);

                WriteNewUserInDb(auth.CurrentUser.UserId, newName.text.ToString());
                moveScene = true;
                errorMessage.text = registerSuccess;
            });
        }
        else
        {
            errorMessage.text = blankInputError;
        }
    }
    void SignIn()
    {
        if (!(signInEmail.text.ToString() == "" || signInPassword.text.ToString() == ""))
        {
            auth.SignInWithEmailAndPasswordAsync(signInEmail.text.ToString(), signInPassword.text.ToString()).ContinueWith(task1 =>
            {
                if (task1.IsCanceled)
                {
                    errorMessage.text = signInError;
                    return;
                }
                if (task1.IsFaulted)
                {
                    errorMessage.text = signInError;
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

                    User temp = new User(tempcoins, tempOwned, tempCurrent, tempOptional, tempBest, tempName);
                    Instance = new CurrentUser(temp, auth.CurrentUser.UserId);
                    moveScene = true;

                    errorMessage.text = signInSuccess;
                });
            });
        }
        else
        {
            errorMessage.text = blankInputError;
        }
    }
    void WriteNewUserInDb(string id, string name)
    {
        User user = new User(name);
        user.AddColor(new List<int> { 255, 0, 0, 150 });
        user.AddColor(new List<int> { 0, 255, 0, 150 });
        user.AddColor(new List<int> { 0, 0, 255, 150 });
        user.AddColor(new List<int> { 0, 255, 255, 150 });
        user.AddColor(new List<int> { 76, 0, 153, 150 });
        string json = JsonUtility.ToJson(user);
        reference.SetRawJsonValueAsync(json);

        Instance = new CurrentUser(user, id);
    }
    void GoToCreateNewUser()
    {
        registerP.SetActive(true);
        signInP.SetActive(false);
        signInEmail.text = "";
        signInPassword.text = "";
        signInPassword.contentType = InputField.ContentType.Password;
        errorMessage.text = "";
    }
    void GoToSignIn()
    {
        registerP.SetActive(false);
        signInP.SetActive(true);
        registerEmail.text = "";
        registerPassword.text = "";
        newName.text = "";
        signInPassword.contentType = InputField.ContentType.Password;
        errorMessage.text = "";
    }
    void ShowSignInPassword()
    {
        if (signInPassword.contentType == InputField.ContentType.Password)
        {
            signInPassword.contentType = InputField.ContentType.Standard;
        }
        else
        {
            signInPassword.contentType = InputField.ContentType.Password;
        }
        signInPassword.ForceLabelUpdate();
    }
    void ShowRegisterPassword()
    {
        if (registerPassword.contentType == InputField.ContentType.Password)
        {
            registerPassword.contentType = InputField.ContentType.Standard;
        }
        else
        {
            registerPassword.contentType = InputField.ContentType.Password;
        }
        registerPassword.ForceLabelUpdate();
    }
    bool CheckIfReadFromDBWasSuccessfull()
    {
        for (int i = 0; i < 10; i++)
        {
            if (LeaderBoards[i].GetScore() == 0)
            {
                return false;
            }
        }
        return true;
    }
    void RememberMe()
    {
        errorMessage.text = signInSuccess;
        signInP.SetActive(false);

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

            User temp = new User(tempcoins, tempOwned, tempCurrent, tempOptional, tempBest, tempName);
            Instance = new CurrentUser(temp, auth.CurrentUser.UserId);
            moveScene = true;

        });
    }
}
public class User
{
    public int Coins;
    public List<int> OwnColors;
    public List<int> CurrentColor;
    public List<int> OptionalColors;
    public int BestScore;
    public string Name;
    public User(string name)
    {
        Coins = 200;
        CurrentColor = new List<int> { 25, 108, 133, 255, 1 };
        OwnColors = new List<int> { -1, 25, 108, 133, 255 };
        OptionalColors = new List<int> { -1, 25, 108, 133, 255 };
        BestScore = 0;
        this.Name = name;
    }
    public User(int co, List<int> ow, List<int> cu, List<int> op, int best, string name)
    {
        Coins = co;
        BestScore = best;
        this.Name = name;
        OwnColors = new List<int>();
        CurrentColor = new List<int>();
        OptionalColors = new List<int>();

        OwnColors.AddRange(ow);
        CurrentColor.AddRange(cu);
        OptionalColors.AddRange(op);
    }
    public User(User temp)
    {
        Coins = temp.Coins;
        Name = temp.Name;
        BestScore = temp.BestScore;
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
    public List<int> GetOptional()
    {
        return OptionalColors;
    }
    public int GetCoins()
    {
        return Coins;
    }
    public void ChangeCoins(int coins)
    {
        this.Coins += coins;
    }
    public void SetCurrentPlace(int place)
    {
        CurrentColor[4] = place;
    }
    public List<int> GetOwned()
    {
        return OwnColors;
    }
    public void SetCoins(int coins)
    {
        this.Coins = coins;
    }
    public string GetName()
    {
        return Name;
    }
    public int GetBestScore()
    {
        return BestScore;
    }
    public void SetBestScore(int score)
    {
        BestScore = score;
    }
}
public class LeaderBoard
{
    public int Score;
    public string Name;
    public string ID;
    public LeaderBoard(int score, string name, string ID)
    {
        this.Score = score;
        this.Name = name;
        this.ID = ID;
    }
    public LeaderBoard(LeaderBoard temp)
    {
        this.Score = temp.Score;
        this.Name = temp.Name;
        this.ID = temp.ID;
    }
    public int GetScore()
    {
        return Score;
    }
    public string GetName()
    {
        return Name;
    }
    public void SetName(string name)
    {
        this.Name = name;
    }
    public void SetScore(int score)
    {
        this.Score = score;
    }
    public void SetId(string ID)
    {
        this.ID = ID;
    }
    public string GetId()
    {
        return ID;
    }
}
public class CurrentUser
{
    public User User;
    public string UserID;
    public CurrentUser(List<int> current, List<int> optional, List<int> own, int coins, string id, int best, string name)
    {
        User = new User(coins, own, current, optional, best, name);
        UserID = id;
    }
    public CurrentUser(User user, string id)
    {
        this.User = new User(user);
        UserID = id;
    }
    public void UpdateCurrent(List<int> current)
    {
        User.GetCurrent().Clear();
        User.GetCurrent().AddRange(current);
    }
    public void UpdateOwned(List<int> owned)
    {
        User.GetOwned().Clear();
        User.GetOwned().AddRange(owned);
    }
    public void UpdateOptional(List<int> optional)
    {
        User.GetOptional().Clear();
        User.GetOptional().AddRange(optional);
    }
    public void UpdateCoins(int coins)
    {
        User.SetCoins(coins);
    }
    public User GetUser()
    {
        return User;
    }
    public string GetUserId()
    {
        return UserID;
    }
}

