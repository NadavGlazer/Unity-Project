using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
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
                //deleting the object if he is in the right place
                if (transform.GetComponent<Transform>().position.z < -12 && transform.name != "Train(Clone)")
                {
                    Destroy(this.gameObject);
                    exit = true;
                }
                else if (transform.GetComponent<Transform>().position.z < -11 && transform.name == "Create(Clone)")
                {
                    Destroy(this.gameObject);
                    exit = true;
                }
                else if (!PauseMenu.IsPaused)
                {
                    //giving the object speed
                    if (transform.name == "Train(Clone)")
                    {
                        transform.GetComponent<Transform>().position -= new Vector3(0, 0, acceleration * (float)1.1);
                    }
                    else
                    {
                        transform.GetComponent<Transform>().position -= new Vector3(0, 0, acceleration * (float)1);
                    }
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
