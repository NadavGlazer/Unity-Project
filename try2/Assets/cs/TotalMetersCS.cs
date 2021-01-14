using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System;
public class TotalMetersCS : MonoBehaviour
{
    public Text TotalMeters;
    string temp;

    // Start is called before the first frame update
    void Start()
    {
        TotalMeters = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Updating the score on the screen
        temp = "";
        int num = 7 - (Math.Round(SetUp.totalRun) + " ").Length;
        for (int i = 0; i < num; i++)
        {
            temp += "0";
        }
        temp += Math.Round(SetUp.totalRun) + " ";
        TotalMeters.text = temp;
    }
}
