using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView
{
    public Text Name;
    public Text Place;
    public Text Score;
    public string ScoreID;
    public ItemView(Transform rootView)
    {
        Name = rootView.Find("NameText").GetComponent<Text>();
        Place = rootView.Find("PlaceText").GetComponent<Text>();
        Score = rootView.Find("ScoreText").GetComponent<Text>();
        ScoreID = "";
    }
    public void CheckBold()
    {
        if (ScoreID == AuthScript.Instance.GetUserId())
        {
            Name.fontStyle = FontStyle.Bold;
            //Score.fontStyle = FontStyle.Bold;
        }
        else
        {
            Name.fontStyle = FontStyle.Normal;
            //Score.fontStyle = FontStyle.Normal;
        }
    }
}
