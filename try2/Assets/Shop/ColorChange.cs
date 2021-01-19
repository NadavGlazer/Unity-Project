using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity;

public class ColorChange : MonoBehaviour
{
    DatabaseReference reference;
    public Text isUsed;
    public Material mat;
    public Text mText;
    public Scrollbar r;
    public Scrollbar g;
    public Scrollbar b;
    public GameObject shopPanel;
    public GameObject creatingNewPanel;
    public Text totalCoins;
    public GameObject buyButton;
    public GameObject useButton;
    bool isCreating;
    int rn;
    int gn;
    int bn;
    int placeChange;
    int place;
    int lastP;
    int firstP;
    string[] textUI;
    int costOfColor;
    // Start is called before the first frame update
    void Start()
    {
        UpdateVer();
    }
    void UpdateVer()
    {
        costOfColor = 200;
        textUI = new string[4];
        textUI[0] = "";
        textUI[1] = "using";
        textUI[2] = "Owned";
        textUI[3] = "Cost: " + costOfColor;
        Time.timeScale = 1f;

        reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthScript.Instance.GetUserId());
        place = AuthScript.Instance.GetUser().GetCurrent()[4];
        lastP = AuthScript.Instance.GetUser().GetOptional().Count - 4;
        firstP = 1;
        placeChange = 4;

        isCreating = false;

        mat.SetColor("_Color", new Color(
            AuthScript.Instance.GetUser().GetCurrent()[0],
            AuthScript.Instance.GetUser().GetCurrent()[1],
            AuthScript.Instance.GetUser().GetCurrent()[2],
            AuthScript.Instance.GetUser().GetCurrent()[3]));
        place = AuthScript.Instance.GetUser().GetCurrent()[4];
        SetCoins();
        UpdateIfOwned();
    }
    // Update is called once per frame
    void Update()
    {
        totalCoins.SetAllDirty();
        mText.SetAllDirty();
        isUsed.SetAllDirty();
        if (isCreating)
        {
            rn = (int)Math.Round(r.GetComponent<Scrollbar>().value * 255);
            gn = (int)Math.Round(g.GetComponent<Scrollbar>().value * 255);
            bn = (int)Math.Round(b.GetComponent<Scrollbar>().value * 255);
            mat.SetColor("_Color", new Color32((byte)rn, (byte)gn, (byte)bn, 150));
        }
        else
        {
            if (IsCurrentColorUsed())
            {
                isUsed.text = textUI[1];
                useButton.SetActive(false);
            }
            else
            {
                isUsed.text = textUI[0];
                useButton.SetActive(true);
            }
            UpdateIfOwned();
            mat.SetColor("_Color", new Color32(
               (byte)AuthScript.Instance.GetUser().GetOptional()[place],
               (byte)AuthScript.Instance.GetUser().GetOptional()[place + 1],
                (byte)AuthScript.Instance.GetUser().GetOptional()[place + 2],
                (byte)AuthScript.Instance.GetUser().GetOptional()[place + 3]));
        }
    }
    void MoveLeftInShop()
    {
        if (place == firstP)
        {
            place = lastP;
        }
        else
        {
            place -= placeChange;
        }
    }
    void MoveRightInShop()
    {
        if (place == lastP)
        {
            place = firstP;
        }
        else
        {
            place += placeChange;
        }
    }
    void UpdateIfOwned()
    {
        if (IsCurrentColorOwned())
        {
            mText.text = textUI[2];
            buyButton.SetActive(false);
        }
        else
        {
            mText.text = textUI[3];
            buyButton.SetActive(true);
        }
    }
    void GoToCreateNewColor()
    {
        shopPanel.SetActive(false);
        creatingNewPanel.SetActive(true);

        r.GetComponent<Scrollbar>().value = (float)AuthScript.Instance.GetUser().GetOptional()[place] / 255;
        g.GetComponent<Scrollbar>().value = (float)AuthScript.Instance.GetUser().GetOptional()[place + 1] / 255;
        b.GetComponent<Scrollbar>().value = (float)AuthScript.Instance.GetUser().GetOptional()[place + 2] / 255;

        isCreating = true;
    }
    void AddNewColorToDB()
    {
        if (IsColorOptional(new List<int> { rn, gn, bn, 150 }) == -1)
        {
            AuthScript.Instance.GetUser().AddColor(new List<int> { rn, gn, bn, 150 });

            UpdateUser(AuthScript.Instance.GetUser());

            lastP += 4;
            place = lastP;
        }
        else
        {
            place = IsColorOptional(new List<int> { rn, gn, bn, 150 });
        }
        BackToMainPage();
    }
    void BuyColor()
    {
        if (AuthScript.Instance.GetUser().GetCoins() >= costOfColor & !IsCurrentColorOwned())
        {
            AuthScript.Instance.GetUser().ChangeCoins(costOfColor * -1);
            AuthScript.Instance.GetUser().AddToOwn(new List<int> {
                AuthScript.Instance.GetUser().GetOptional()[place],
                AuthScript.Instance.GetUser().GetOptional()[place + 1],
                AuthScript.Instance.GetUser().GetOptional()[place + 2],
                AuthScript.Instance.GetUser().GetOptional()[place + 3] });

            UpdateUser(AuthScript.Instance.GetUser());

            SetCoins();

            UpdateIfOwned();
        }
    }
    bool IsCurrentColorUsed()
    {
        return (IsCurrentColorOwned()
            && AuthScript.Instance.GetUser().GetCurrent()[0] == AuthScript.Instance.GetUser().GetOptional()[place]
            && AuthScript.Instance.GetUser().GetCurrent()[1] == AuthScript.Instance.GetUser().GetOptional()[place + 1]
            && AuthScript.Instance.GetUser().GetCurrent()[2] == AuthScript.Instance.GetUser().GetOptional()[place + 2]
            && AuthScript.Instance.GetUser().GetCurrent()[3] == AuthScript.Instance.GetUser().GetOptional()[place + 3]);
    }
    void UseCurrentColor()
    {
        if (IsCurrentColorOwned())
        {
            for (int i = 0; i < 4; i++)
            {
                AuthScript.Instance.GetUser().GetCurrent()[i] = AuthScript.Instance.GetUser().GetOptional()[place + i];
            }
            AuthScript.Instance.GetUser().SetCurrentPlace(place);

            UpdateUser(AuthScript.Instance.GetUser());

            isUsed.text = textUI[1];
        }
    }
    void BackToMainPage()
    {
        shopPanel.SetActive(true);
        creatingNewPanel.SetActive(false);
        isCreating = false;

        mat.SetColor("_Color", new Color(
                 AuthScript.Instance.GetUser().GetOptional()[place],
                 AuthScript.Instance.GetUser().GetOptional()[place + 1],
                 AuthScript.Instance.GetUser().GetOptional()[place + 2],
                 AuthScript.Instance.GetUser().GetOptional()[place + 3]));
        UpdateIfOwned();
    }
    void SetCoins()
    {
        totalCoins.text = AuthScript.Instance.GetUser().GetCoins().ToString();
    }
    bool IsCurrentColorOwned()
    {
        for (int i = 1; i < AuthScript.Instance.GetUser().GetOwned().Count; i += 4)
        {
            if (AuthScript.Instance.GetUser().GetOptional()[place] == AuthScript.Instance.GetUser().GetOwned()[i]
                && AuthScript.Instance.GetUser().GetOptional()[place + 1] == AuthScript.Instance.GetUser().GetOwned()[i + 1]
                && AuthScript.Instance.GetUser().GetOptional()[place + 2] == AuthScript.Instance.GetUser().GetOwned()[i + 2]
                && AuthScript.Instance.GetUser().GetOptional()[place + 3] == AuthScript.Instance.GetUser().GetOwned()[i + 3])
            {
                return true;
            }
        }
        return false;
    }
    int IsColorOptional(List<int> color)
    {
        for (int i = 1; i < AuthScript.Instance.GetUser().GetOptional().Count; i += 4)
        {
            if (color[0] == AuthScript.Instance.GetUser().GetOptional()[i]
               && color[1] == AuthScript.Instance.GetUser().GetOptional()[i + 1]
               && color[2] == AuthScript.Instance.GetUser().GetOptional()[i + 2]
               && color[3] == AuthScript.Instance.GetUser().GetOptional()[i + 3])
            {
                return i;
            }
        }
        return -1;
    }
    void UpdateUser(User user)
    {
        string json = JsonUtility.ToJson(user);
        reference.SetRawJsonValueAsync(json);
    }
    void GoToLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }
    void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }
    void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}



