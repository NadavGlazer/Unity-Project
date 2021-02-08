using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using UnityEngine;

public class SetUp : MonoBehaviour
{
    public static int Frames;
    public static float TotalAcceleration;
    public static float TotalRun;
    float accBefore;
    float timeWP;
    float maxACC;
    float sPlaceStartZ;
    string thePlaceName;
    string theSPlaceName;
    int fPlaceEndPos;
    int sPlaceEndPos;
    void Start()
    {
        UpdateVer();
        //restarting the ground position
        if (transform.name == thePlaceName)
        {
            transform.position = new Vector3(0, 0, 0);//restarting the x,y,z of the place      
        }
        else
        {
            transform.position = new Vector3(0, 0, sPlaceStartZ);//restarting the x,y,z of the place      
        }
    }
    void Update()
    {
        if (!CollCheck.HasLost)
        {

            if (!PauseMenu.IsPaused)
            {
                //updating verables
                TotalRun += TotalAcceleration * (float)3;
                Frames++;

                //accelerating by the time         

                if (TotalAcceleration <= maxACC && Frames % 5 == 0)
                {
                    TotalAcceleration = (TotalAcceleration + Time.deltaTime / 40) * Time.timeScale;
                }
                if (TotalAcceleration > maxACC && Frames % 200 == 0)
                {
                    TotalAcceleration = (TotalAcceleration + Time.deltaTime / 60) * Time.timeScale;
                }

                //moving the Places        
                transform.position -= new Vector3(0, 0, TotalAcceleration);

                //every time the field z is 5- teleports the field to where it began in to create infnite loop
                if (transform.position.z <= fPlaceEndPos && transform.name == thePlaceName)
                {
                    transform.position = new Vector3(0, 0, 0);
                }
                else if (transform.position.z <= sPlaceEndPos && transform.name == theSPlaceName)
                {
                    transform.position = new Vector3(0, 0, sPlaceStartZ);
                }
            }
        }
    }

    //function that sets the starting values of the objects
    void UpdateVer()
    {
        Application.targetFrameRate = 60;
        Frames = 1;
        TotalAcceleration = 0.01f;
        accBefore = timeWP = TotalRun = 0.0f;
        maxACC = 0.045f;
        Time.timeScale = 1f;
        thePlaceName = "ThePlace";
        theSPlaceName = "ThePlace (1)";
        sPlaceStartZ = 14f;
        fPlaceEndPos = -3;
        sPlaceEndPos = 11;
    }
}




