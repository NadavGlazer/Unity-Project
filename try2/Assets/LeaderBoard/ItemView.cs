using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemView
{
    public Text NamePlace;
    public Text Score;
    public string ScoreID;
    public ItemView(Transform rootView)
    {
        NamePlace = rootView.Find("NamePlaceText").GetComponent<Text>();
        Score = rootView.Find("ScoreText").GetComponent<Text>();
        ScoreID = "";
    }
    public void CheckBold()
    {
        if (ScoreID == AuthScript.Instance.GetUserId())
        {
            NamePlace.fontStyle = FontStyle.Bold;
            Score.fontStyle = FontStyle.Bold;
        }
        else
        {
            NamePlace.fontStyle = FontStyle.Normal;
            Score.fontStyle = FontStyle.Normal;
        }
    }
}
