using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSpawns : MonoBehaviour
{
    public bool exit;
    public float acceleration;
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
            acceleration = SetUp.totalAcceleration;
            if (!exit)
            {
                //checking if the object need to be destroyed, else bring it forward
                if (transform.GetComponent<RectTransform>().position.z < -20)
                {
                    Destroy(this.gameObject);
                    exit = true;
                }
                else if (!PauseMenu.IsPaused)
                {
                    transform.GetComponent<RectTransform>().position -= new Vector3(0, 0, acceleration);
                }

            }
        }
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        exit = false;
        acceleration = SetUp.totalAcceleration;
    }
}
