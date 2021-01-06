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
    public static int frames;
    public static float totalAcceleration, totalRun;
    float accBefore, timeWP, maxACC;
    bool[] secCheck;
    bool jump, hasJumped, happend;
    void Start()
    {
        UpdateVer();
        //restarting the ground position
        if (transform.name == "ThePlace")
        {
            transform.position = new Vector3(0, 0, 0);//restarting the x,y,z of the place      
        }
        else
        {
            transform.position = new Vector3(0, 0, (float)14);//restarting the x,y,z of the place      
        }
    }
    void Update()
    {
        if (!CollCheck.HasLost)
        {
            if (PauseMenu.IsPaused && !happend)
            {
                happend = true;
            }
            if (happend && !PauseMenu.IsPaused)
            {
                happend = false;
                totalAcceleration = accBefore;
            }
            if (!happend && !PauseMenu.IsPaused)
            {
                accBefore = totalAcceleration;
            }
            if (!PauseMenu.IsPaused)
            {
                //updating verables
                totalRun += totalAcceleration * (float)3;
                frames++;
                jump = Move.firstJump;

                //accelerating by the time         

                if (totalAcceleration <= maxACC && frames % 10 == 0)
                {
                    totalAcceleration = (totalAcceleration + Time.deltaTime / 60) * Time.timeScale;
                }
                if (totalAcceleration > maxACC && frames % 300 == 0)
                {
                    totalAcceleration = (totalAcceleration + Time.deltaTime / 150) * Time.timeScale;
                }

                //moving the Places        
                transform.position -= new Vector3(0, 0, totalAcceleration);

                //every time the field z is 5- teleports the field to where it began in to create infnite loop
                if (transform.position.z <= -3 && transform.name == "ThePlace")
                {
                    transform.position = new Vector3(0, 0, 0);
                }
                else if (transform.position.z <= 11 && transform.name == "ThePlace (1)")
                {
                    transform.position = new Vector3(0, 0, (float)14);
                }
            }
        }
    }
    //function that increasing the speed
    float acceleration()
    {
        float ret = 0;
        float time = (float)Math.Round(Time.time - timeWP);
        if (PauseMenu.IsPaused && !happend)
        {
            timeWP = Math.Abs(Time.time);
            happend = true;
        }
        if (time == 1 || time == 2 || time == 4 || time == 5)
        {
            if (!secCheck[(int)time])
            {
                ret = 0.001f;
                secCheck[(int)time] = true;
            }
        }
        if ((time > 10 && time < 40) && time % 3 == 0)
        {
            if (!secCheck[(int)time])
            {
                ret = 0.001f;
                secCheck[(int)time] = true;
            }
        }
        if ((time > 50 && time % 2 == 0) && totalAcceleration < 0.04f)
        {
            if (!secCheck[(int)time])
            {
                ret = 0.001f;
                secCheck[(int)time] = true;
            }
        }
        return ret;
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        Application.targetFrameRate = 60;
        frames = 1;
        totalAcceleration = 0.01f;
        secCheck = new bool[1000];
        accBefore = timeWP = totalRun = 0.0f;
        hasJumped = happend = false;
        for (int i = 0; i < secCheck.Length; i++)
        {
            secCheck[i] = false;
        }
        maxACC = 0.045f;
        Time.timeScale = 1f;
    }
}




