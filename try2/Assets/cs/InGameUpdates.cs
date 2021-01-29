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
    public Text CurrentCoins;
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
        int num = 7 - (Math.Round(SetUp.TotalRun) + " ").Length;
        for (int i = 0; i < num; i++)
        {
            temp += "0";
        }
        temp += Math.Round(SetUp.TotalRun) + " ";
        TotalMeters.text = temp;

        CurrentCoins.text = CoinsInCurrentRun.ToString();
    }
}
