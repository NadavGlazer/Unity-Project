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
    public InputField SignInEmail;
    public InputField RegisterEmail;
    public InputField RegisterPassword;
    public InputField SignInPassword;
    public InputField NewName;
    public Text SignInResultMessage;
    public Text RegisterResultMessage;
    public Text RememberMeName;
    public GameObject RegisterP;
    public GameObject SignInP;
    public GameObject RememberMeP;
    public GameObject SignInErrorImage;
    public GameObject RegisterErrorImage;
    public GameObject SignInSHButton;
    public GameObject RegisterSHButton;
    public Sprite ShowPasswordImage;
    public Sprite HidePasswordImage;
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
        //Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();

        //for (int i = 1; i < 11; i++)
        //{
        //    string json = JsonUtility.ToJson(new LeaderBoard(1, "temp", "0"));
        //    FirebaseDatabase.DefaultInstance.RootReference.Child("LeaderBoard").Child(i.ToString()).SetRawJsonValueAsync(json);
        //}
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
        SignInResultMessage.text = "";

        RememberMe();
    }

    // Update is called once per frame
    void Update()
    {
        SignInResultMessage.SetAllDirty();
        RegisterResultMessage.SetAllDirty();
        RememberMeName.SetAllDirty();
        if (moveScene)
        {
            SceneManager.LoadScene("MainMenu");
            Time.timeScale = 1.1f;
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
        ScrollViewAdapter.HasChanged = true;
    }
    void Register()
    {
        if (!(RegisterEmail.text.ToString() == "" && RegisterPassword.text.ToString() == "") && NewName.text.ToString() != null)
        {
            auth.CreateUserWithEmailAndPasswordAsync(RegisterEmail.text.ToString(), RegisterPassword.text.ToString()).ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    RegisterErrorShow();
                    return;
                }
                if (task.IsFaulted)
                {
                    RegisterErrorShow();

                    return;
                }


                // Firebase user has been created.

                Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                reference = reference.Child("Users").Child(auth.CurrentUser.UserId);

                WriteNewUserInDb(auth.CurrentUser.UserId, NewName.text.ToString());
                moveScene = true;
                RegisterResultMessage.text = registerSuccess;

            });
        }
        else
        {
            RegisterResultMessage.text = blankInputError;
            RegisterErrorImage.SetActive(true);
        }
    }
    void SignIn()
    {
        if (!(SignInEmail.text.ToString() == "" || SignInPassword.text.ToString() == ""))
        {
            auth.SignInWithEmailAndPasswordAsync(SignInEmail.text.ToString(), SignInPassword.text.ToString()).ContinueWith(task1 =>
            {
                if (task1.IsCanceled)
                {
                    SignInErrorShow();

                    return;
                }
                if (task1.IsFaulted)
                {
                    SignInErrorShow();

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

                    tempcoins = int.Parse(snapshot.Child("Coins").Value.ToString());
                    tempBest = int.Parse(snapshot.Child("BestScore").Value.ToString());
                    tempName = snapshot.Child("Name").Value.ToString();

                    User temp = new User(tempcoins, tempOwned, tempCurrent, tempOptional, tempBest, tempName);
                    Instance = new CurrentUser(temp, auth.CurrentUser.UserId);
                    moveScene = true;
                    SignInResultMessage.text = signInSuccess;

                });
            });
        }
        else
        {
            SignInResultMessage.text = blankInputError;
            SignInErrorImage.SetActive(true);
        }
    }
    void SignInErrorShow()
    {
        SignInResultMessage.text = signInError;
        SignInErrorImage.SetActive(true);
    }
    void RegisterErrorShow()
    {
        RegisterResultMessage.text = registerError;
        RegisterErrorImage.SetActive(true);
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
    public void GoToRegister()
    {
        RegisterP.SetActive(true);
        SignInP.SetActive(false);
        SignInEmail.text = "";
        SignInPassword.text = "";
        SignInPassword.contentType = InputField.ContentType.Password;
        RegisterResultMessage.text = "";
        SignInErrorImage.SetActive(false);
        RegisterErrorImage.SetActive(false);
    }
    void GoToSignIn()
    {
        RegisterP.SetActive(false);
        SignInP.SetActive(true);
        RegisterEmail.text = "";
        RegisterPassword.text = "";
        NewName.text = "";
        SignInPassword.contentType = InputField.ContentType.Password;
        SignInResultMessage.text = "";
        SignInErrorImage.SetActive(false);
        RegisterErrorImage.SetActive(false);
    }
    void ShowSignInPassword()
    {
        if (SignInPassword.contentType == InputField.ContentType.Password)
        {
            SignInPassword.contentType = InputField.ContentType.Standard;
            SignInSHButton.GetComponent<Image>().sprite = HidePasswordImage;
        }
        else
        {
            SignInPassword.contentType = InputField.ContentType.Password;
            SignInSHButton.GetComponent<Image>().sprite = ShowPasswordImage;
        }
        SignInPassword.ForceLabelUpdate();
    }
    public void ShowRegisterPassword()
    {
        if (RegisterPassword.contentType == InputField.ContentType.Password)
        {
            RegisterPassword.contentType = InputField.ContentType.Standard;
            RegisterSHButton.GetComponent<Image>().sprite = HidePasswordImage;
        }
        else
        {
            RegisterPassword.contentType = InputField.ContentType.Password;
            RegisterSHButton.GetComponent<Image>().sprite = ShowPasswordImage;
        }
        RegisterPassword.ForceLabelUpdate();
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
        if (Firebase.Auth.FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            SignInP.SetActive(false);
            RememberMeP.SetActive(true);

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
                tempcoins = int.Parse(snapshot.Child("Coins").Value.ToString());
                tempBest = int.Parse(snapshot.Child("BestScore").Value.ToString());
                tempName = snapshot.Child("Name").Value.ToString();

                User temp = new User(tempcoins, tempOwned, tempCurrent, tempOptional, tempBest, tempName);
                Instance = new CurrentUser(temp, auth.CurrentUser.UserId);
                moveScene = true;
                RememberMeName.text = tempName;
            });
        }
    }
    public void OnFixInputFieldSignIn()
    {
        SignInResultMessage.text = "";
        SignInErrorImage.SetActive(false);
    }
    public void OnFixInputFieldRegister()
    {
        RegisterResultMessage.text = "";
        RegisterErrorImage.SetActive(false);
    }
    public void ContinueAsUser()
    {
        if (RememberMeName.text != "")
        {
            moveScene = true;
        }
    }
    public void DontContinueAsUser()
    {
        if (RememberMeName.text != "")
        {
            SignInP.SetActive(true);
            RememberMeP.SetActive(false);
            Firebase.Auth.FirebaseAuth.DefaultInstance.SignOut();
            Instance = new CurrentUser();
        }
    }
}


