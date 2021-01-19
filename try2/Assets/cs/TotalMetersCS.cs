using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TotalMetersCS : MonoBehaviour
{
    public Text totalMeters;
    string temp;

    // Update is called once per frame
    void Update()
    {
        //Updating the score on the screen
        temp = "";
        int num = 7 - (Math.Round(SetUp.TotalRun) + " ").Length;
        for (int i = 0; i < num; i++)
        {
            temp += "0";
        }
        temp += Math.Round(SetUp.TotalRun) + " ";
        totalMeters.text = temp;
    }
}
