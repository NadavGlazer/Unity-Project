using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System;
public class InGameUpdates : MonoBehaviour
{
    public static int CoinsInCurrentRun;
    public Text TotalMeters;
    public Text TotalMetersOnPause;
    public Text CurrentCoins;
    public Text CurrentCoinsOnPause;
    string temp;

    void Start()
    {
        temp = "";
        CoinsInCurrentRun = 0;
    }
    // Update is called once per frame
    void Update()
    {
        temp = "";
        //Updating the score on the screen
        int num = 6 - (Math.Round(SetUp.TotalRun) + "").Length;
        for (int i = 0; i < num; i++)
        {
            temp += "0";
        }
        temp += Math.Round(SetUp.TotalRun).ToString();
        TotalMeters.text = temp;
        TotalMetersOnPause.text = temp;

        CurrentCoins.text = CoinsInCurrentRun.ToString();
        CurrentCoinsOnPause.text = CoinsInCurrentRun.ToString();
    }
}
