using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    bool exit;
    public float acceleration;
    string trainCloneName, crateCloneName;
    int trainZOfDestruction, crateZOfDestruction;
    float trainAcc, crateAcc;
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
                if (transform.GetComponent<Transform>().position.z < trainZOfDestruction && transform.name == trainCloneName)
                {
                    Destroy(this.gameObject);
                    exit = true;
                }
                else if (transform.GetComponent<Transform>().position.z < crateZOfDestruction && transform.name == crateCloneName)
                {
                    Destroy(this.gameObject);
                    exit = true;
                }
                else if (!PauseMenu.IsPaused)
                {
                    //giving the object speed
                    if (transform.name == trainCloneName)
                    {
                        transform.GetComponent<Transform>().position -= new Vector3(0, 0, acceleration * trainAcc);
                    }
                    else
                    {
                        transform.GetComponent<Transform>().position -= new Vector3(0, 0, acceleration * crateAcc);
                    }
                }

                if (transform.GetComponent<Transform>().position.z < 8f && transform.name == trainCloneName)
                {
                    ObstacleSpawner.spawn = true;
                }
            }
        }
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        exit = false;
        acceleration = SetUp.totalAcceleration;
        trainCloneName = "Train(Clone)";
        crateCloneName = "Crate(Clone)";
        trainZOfDestruction = -12;
        crateZOfDestruction = -11;
        trainAcc = 1.1f;
        crateAcc = 1f;
    }
}
