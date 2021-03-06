﻿using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class BackGroundSpawn : MonoBehaviour
{
    public GameObject lamp;
    public GameObject tamrur;
    int whatSpawn;
    int lane;
    int numLamps;
    int numTamrur;
    float spawnRate;
    float nextSpawn;
    float spawnerZ;

    // Start is called before the first frame update
    void Start()
    {
        UpdateVer();
    }
    // Update is called once per frame
    void Update()
    {
        if (!CollCheck.HasLost)
        {
            if (Time.time < 20)
            {
                spawnRate = 3f;
            }
            if (Time.time < 30)
            {
                spawnRate = 2.5f;
            }
            else
            {
                spawnRate = 2f;
            }

            if (Time.time > nextSpawn)
            {
                //spawn random object in random place
                lane = UnityEngine.Random.Range(1, 3);
                whatSpawn = UnityEngine.Random.Range(1, 3);
                switch (whatSpawn)
                {
                    case 1:
                        numLamps++;
                        SpawnLamp(lane, numLamps);
                        break;
                    case 2:
                        numTamrur++;
                        SpawnTamrur(lane, numTamrur);
                        break;
                }
                nextSpawn = Time.time + spawnRate;
            }
        }
    }
    //spawn lamp in given place
    void SpawnLamp(int lane, int numLamps)
    {

        Vector3 temp = new Vector3(0, 0, spawnerZ);
        int rotation = 0;
        if (lane == 1)
        {
            temp += new Vector3((float)-2.3, 0, 0);
            rotation = 90;
        }
        else
        {
            temp += new Vector3((float)2.3, 0, 0);
            rotation = -90;
        }
        var templamp = Instantiate(lamp, temp, Quaternion.Euler(new Vector3(0, rotation, 0))) as GameObject;
        GameObject spot = GameObject.Find("Spot");
        spot.GetComponent<Light>().range = 5f;
        spot.GetComponent<Light>().intensity = 0.007f;
        spot.GetComponent<Light>().spotAngle = 8000;
        Color l = new Color((float)200, (float)207, (float)97);
        spot.GetComponent<Light>().color = l;
        spot.gameObject.name = "spot" + numLamps;
        templamp.GetComponent<Transform>().localScale = new Vector3((float)1.2, (float)1.2, (float)1.2);
        templamp.AddComponent<RectTransform>();
        templamp.AddComponent<BackGroundSpawns>();

    }
    //spawns tamrur in given place
    void SpawnTamrur(int lane, int numTamrur)
    {
        Vector3 temp = new Vector3(0, 0, spawnerZ);
        int rotation = 90;
        if (lane == 1)
        {
            temp += new Vector3((float)-1.9, 0, 0);
        }
        else
        {
            temp += new Vector3((float)1.9, 0, 0);
        }
        var temptamrur = Instantiate(tamrur, temp, Quaternion.Euler(new Vector3((float)-90, rotation, 0))) as GameObject;
        temptamrur.AddComponent<RectTransform>();
        temptamrur.AddComponent<BackGroundSpawns>();
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        nextSpawn = 0;
        spawnerZ = 7f;
        transform.position = new Vector3(0, 0, spawnerZ);
        numLamps = 0;
        numTamrur = 0;
        spawnRate = 0f;
    }
}
