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
    int place, lastP, firstP;
    public Material mat;
    public Text mText;
    public Scrollbar r;
    public Scrollbar g;
    public Scrollbar b;
    public GameObject Shop;
    public GameObject CreatingNew;
    public Text totalCoins;
    bool isCreating;
    int rn, gn, bn, placeChange;
    public Text isUsed;
    public DatabaseReference reference;
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

        reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthScript.instance.GetUserId());
        place = AuthScript.instance.GetUser().GetCurrent()[4];
        lastP = AuthScript.instance.GetUser().GetOptional().Count - 4;
        firstP = 1;
        placeChange = 4;

        isCreating = false;

        mat.SetColor("_Color", new Color(
            AuthScript.instance.GetUser().GetCurrent()[0],
            AuthScript.instance.GetUser().GetCurrent()[1],
            AuthScript.instance.GetUser().GetCurrent()[2],
            AuthScript.instance.GetUser().GetCurrent()[3]));
        place = AuthScript.instance.GetUser().GetCurrent()[4];
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
            }
            else
            {
                isUsed.text = textUI[0];
            }
            UpdateIfOwned();
            mat.SetColor("_Color", new Color32(
               (byte)AuthScript.instance.GetUser().GetOptional()[place],
               (byte)AuthScript.instance.GetUser().GetOptional()[place + 1],
                (byte)AuthScript.instance.GetUser().GetOptional()[place + 2],
                (byte)AuthScript.instance.GetUser().GetOptional()[place + 3]));
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
        }
        else
        {
            mText.text = textUI[3];
        }
    }
    void GoToCreateNewColor()
    {
        Shop.SetActive(false);
        CreatingNew.SetActive(true);

        r.GetComponent<Scrollbar>().value = (float)AuthScript.instance.GetUser().GetCurrent()[0] / 255;
        g.GetComponent<Scrollbar>().value = (float)AuthScript.instance.GetUser().GetCurrent()[1] / 255;
        b.GetComponent<Scrollbar>().value = (float)AuthScript.instance.GetUser().GetCurrent()[2] / 255;

        isCreating = true;
    }
    void AddNewColorToDB()
    {
        AuthScript.instance.GetUser().AddColor(new List<int> { rn, gn, bn, 150 });

        UpdateUser(AuthScript.instance.GetUser());

        lastP += 4;
        place = lastP;

        BackToMainPage();
    }
    void BuyColor()
    {
        if (AuthScript.instance.GetUser().GetCoins() >= costOfColor & !IsCurrentColorOwned())
        {
            AuthScript.instance.GetUser().ChangeCoins(costOfColor * -1);
            AuthScript.instance.GetUser().AddToOwn(new List<int> {
                AuthScript.instance.GetUser().GetOptional()[place],
                AuthScript.instance.GetUser().GetOptional()[place + 1],
                AuthScript.instance.GetUser().GetOptional()[place + 2],
                AuthScript.instance.GetUser().GetOptional()[place + 3] });

            UpdateUser(AuthScript.instance.GetUser());

            SetCoins();

            UpdateIfOwned();
        }
    }
    bool IsCurrentColorUsed()
    {
        return (IsCurrentColorOwned()
            && AuthScript.instance.GetUser().GetCurrent()[0] == AuthScript.instance.GetUser().GetOptional()[place]
            && AuthScript.instance.GetUser().GetCurrent()[1] == AuthScript.instance.GetUser().GetOptional()[place + 1]
            && AuthScript.instance.GetUser().GetCurrent()[2] == AuthScript.instance.GetUser().GetOptional()[place + 2]
            && AuthScript.instance.GetUser().GetCurrent()[3] == AuthScript.instance.GetUser().GetOptional()[place + 3]);
    }
    void UseCurrentColor()
    {
        if (IsCurrentColorOwned())
        {
            for (int i = 0; i < 4; i++)
            {
                AuthScript.instance.GetUser().GetCurrent()[i] = AuthScript.instance.GetUser().GetOptional()[place + i];
            }
            AuthScript.instance.GetUser().SetCurrentPlace(place);

            UpdateUser(AuthScript.instance.GetUser());

            isUsed.text = textUI[1];
        }
    }
    void BackToMainPage()
    {
        Shop.SetActive(true);
        CreatingNew.SetActive(false);
        isCreating = false;

        mat.SetColor("_Color", new Color(
                 AuthScript.instance.GetUser().GetOptional()[place],
                 AuthScript.instance.GetUser().GetOptional()[place + 1],
                 AuthScript.instance.GetUser().GetOptional()[place + 2],
                 AuthScript.instance.GetUser().GetOptional()[place + 3]));
        UpdateIfOwned();
    }
    void SetCoins()
    {
        totalCoins.text = AuthScript.instance.GetUser().GetCoins().ToString();
    }
    bool IsCurrentColorOwned()
    {
        for (int i = 1; i < AuthScript.instance.GetUser().GetOwned().Count; i += 4)
        {
            if (AuthScript.instance.GetUser().GetOptional()[place] == AuthScript.instance.GetUser().GetOwned()[i]
                && AuthScript.instance.GetUser().GetOptional()[place + 1] == AuthScript.instance.GetUser().GetOwned()[i + 1]
                && AuthScript.instance.GetUser().GetOptional()[place + 2] == AuthScript.instance.GetUser().GetOwned()[i + 2]
                && AuthScript.instance.GetUser().GetOptional()[place + 3] == AuthScript.instance.GetUser().GetOwned()[i + 3])
            {
                return true;
            }
        }
        return false;
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
        SceneManager.LoadScene("SampleScene");
    }
}



