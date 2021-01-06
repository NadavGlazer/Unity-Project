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
    public int place, lastP;
    public Material mat;
    public Text mText;
    public Scrollbar r;
    public Scrollbar g;
    public Scrollbar b;
    public GameObject Shop;
    public GameObject CreatingNew;
    public Text totalCoins;
    bool isCreating;
    int rn, gn, bn;
    public Text isUsed;
    public DatabaseReference reference;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;

        reference = FirebaseDatabase.DefaultInstance.RootReference.Child("Users").Child(AuthScript.instance.userID);
        place = AuthScript.instance.user.CurrentColor[4];
        lastP = AuthScript.instance.user.OptionalColors.Count - 4;


        isCreating = false;

        mat.SetColor("_Color", new Color(
            AuthScript.instance.user.CurrentColor[0],
            AuthScript.instance.user.CurrentColor[1],
            AuthScript.instance.user.CurrentColor[2],
            AuthScript.instance.user.CurrentColor[3]));
        place = AuthScript.instance.user.CurrentColor[4];
        SetCoins();
        OnChange();
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
            if (isUsing())
            {
                isUsed.text = "using";
            }
            else
            {
                isUsed.text = "";
            }
            OnChange();
            mat.SetColor("_Color", new Color32(
               (byte)AuthScript.instance.user.OptionalColors[place],
               (byte)AuthScript.instance.user.OptionalColors[place + 1],
                (byte)AuthScript.instance.user.OptionalColors[place + 2],
                (byte)AuthScript.instance.user.OptionalColors[place + 3]));
        }
    }
    public void LeftButtonChange()
    {
        if (place == 1)
        {
            place = lastP;
        }
        else
        {
            place -= 4;
        }
    }
    public void RightButtonChange()
    {
        if (place == lastP)
        {
            place = 1;
        }
        else
        {
            place += 4;
        }
    }
    void OnChange()
    {
        if (isOwned())
        {
            mText.text = "Owned";
        }
        else
        {
            mText.text = "Cost: 200";
        }
    }
    void AddNewColor()
    {
        Shop.SetActive(false);
        CreatingNew.SetActive(true);

        r.GetComponent<Scrollbar>().value = (float)AuthScript.instance.user.CurrentColor[0] / 255;
        g.GetComponent<Scrollbar>().value = (float)AuthScript.instance.user.CurrentColor[1] / 255;
        b.GetComponent<Scrollbar>().value = (float)AuthScript.instance.user.CurrentColor[2] / 255;

        isCreating = true;
    }
    void CreateNewColor()
    {
        AuthScript.instance.user.AddColor(new List<int> { rn, gn, bn, 150 });

        UpdateUser(AuthScript.instance.user);

        lastP += 4;
        place = lastP;

        Back();
    }
    void Buy()
    {
        if (AuthScript.instance.user.coins >= 200 & !isOwned())
        {
            AuthScript.instance.user.coins -= 200;
            AuthScript.instance.user.AddToOwn(new List<int> {
                AuthScript.instance.user.OptionalColors[place],
                AuthScript.instance.user.OptionalColors[place + 1],
                AuthScript.instance.user.OptionalColors[place + 2],
                AuthScript.instance.user.OptionalColors[place + 3] });

            UpdateUser(AuthScript.instance.user);

            SetCoins();

            OnChange();
        }
    }
    bool isUsing()
    {
        return (isOwned()
            && AuthScript.instance.user.CurrentColor[0] == AuthScript.instance.user.OptionalColors[place]
            && AuthScript.instance.user.CurrentColor[1] == AuthScript.instance.user.OptionalColors[place + 1]
            && AuthScript.instance.user.CurrentColor[2] == AuthScript.instance.user.OptionalColors[place + 2]
            && AuthScript.instance.user.CurrentColor[3] == AuthScript.instance.user.OptionalColors[place + 3]);
    }
    void Use()
    {
        if (isOwned())
        {
            for (int i = 0; i < 4; i++)
            {
                AuthScript.instance.user.CurrentColor[i] = AuthScript.instance.user.OptionalColors[place + i];
            }
            AuthScript.instance.user.CurrentColor[4] = place;

            UpdateUser(AuthScript.instance.user);

            isUsed.text = "using";
        }
    }
    public void Back()
    {
        Shop.SetActive(true);
        CreatingNew.SetActive(false);
        isCreating = false;

        mat.SetColor("_Color", new Color(
                 AuthScript.instance.user.OptionalColors[place],
                 AuthScript.instance.user.OptionalColors[place + 1],
                 AuthScript.instance.user.OptionalColors[place + 2],
                 AuthScript.instance.user.OptionalColors[place + 3]));
        OnChange();
    }
    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene");
    }
    void SetCoins()
    {
        totalCoins.text = AuthScript.instance.user.coins.ToString();
    }
    bool isOwned()
    {
        for (int i = 1; i < AuthScript.instance.user.OwnColors.Count; i += 4)
        {
            if (AuthScript.instance.user.OptionalColors[place] == AuthScript.instance.user.OwnColors[i]
                && AuthScript.instance.user.OptionalColors[place + 1] == AuthScript.instance.user.OwnColors[i + 1]
                && AuthScript.instance.user.OptionalColors[place + 2] == AuthScript.instance.user.OwnColors[i + 2]
                && AuthScript.instance.user.OptionalColors[place + 3] == AuthScript.instance.user.OwnColors[i + 3])
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
    public void GoToLeaderBoard()
    {
        SceneManager.LoadScene("LeaderBoard");
    }
}



