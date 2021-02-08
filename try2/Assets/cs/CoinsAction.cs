using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CoinsAction : MonoBehaviour
{
    bool exit;
    bool onTrain;
    bool onBox;
    float acceleration;
    float coinX;
    float coinY;
    float distanceFromObstacles;
    string playerName;
    string trainCloneName;
    string boxCloneName;
    RaycastHit hitInfo;
    // Start is called before the first frame update
    void Start()
    {
        UpdateVer();
    }
    // Update is called once per frame
    void Update()
    {
        if (!exit)
        {
            if (!CollCheck.HasLost)
            {
                ToCloseToObstacles();
                acceleration = SetUp.TotalAcceleration;
                if (transform.GetComponent<Transform>().position.z < -7 || transform.GetComponent<Transform>().position.z > 14)
                {
                    Destroy(this.gameObject);
                    exit = true;
                }
                else if (!PauseMenu.IsPaused)
                {
                    if (onTrain)
                    {
                        transform.GetComponent<Transform>().position -= new Vector3(0, 0, acceleration * 1.1f);
                    }
                    else if (onBox)
                    {
                        transform.GetComponent<Transform>().position -= new Vector3(0, 0, acceleration);
                    }
                    else
                    {
                        transform.GetComponent<Transform>().position -= new Vector3(0, 0, acceleration * 1.05f);
                    }
                }
            }
            transform.GetComponent<Transform>().Rotate(2f, 0, 0);
            transform.GetComponent<Transform>().eulerAngles = new Vector3(0, transform.GetComponent<Transform>().eulerAngles.y, 90f);
            transform.GetComponent<Transform>().position = new Vector3(coinX, coinY, transform.GetComponent<Transform>().position.z);
        }
    }
    void ToCloseToObstacles()
    {
        if (Physics.Raycast(transform.GetComponent<Transform>().position, Vector3.forward, out hitInfo, distanceFromObstacles) && !onBox && !onTrain)
        {
            if (transform.GetComponent<Transform>().position.z > 5)
            {
                if (hitInfo.collider.gameObject.name == boxCloneName)
                {
                    transform.GetComponent<Transform>().position = new Vector3(coinX, coinY, transform.GetComponent<Transform>().position.z - 1.3f);
                }
                else if (hitInfo.collider.gameObject.name == trainCloneName)
                {
                    transform.GetComponent<Transform>().position = new Vector3(coinX, coinY, transform.GetComponent<Transform>().position.z - 2.3f);
                }
            }
            else if (hitInfo.collider.gameObject.name == trainCloneName)
            {
                onTrain = true;
            }
        }
        else if (Physics.Raycast(transform.GetComponent<Transform>().position, Vector3.back, out hitInfo, distanceFromObstacles) && !onBox && !onTrain)
        {
            if (transform.GetComponent<Transform>().position.z > 5)
            {
                if (hitInfo.collider.gameObject.name == boxCloneName)
                {
                    transform.GetComponent<Transform>().position = new Vector3(coinX, coinY, transform.GetComponent<Transform>().position.z + 1.3f);
                }
                else if (hitInfo.collider.gameObject.name == trainCloneName)
                {
                    transform.GetComponent<Transform>().position = new Vector3(coinX, coinY, transform.GetComponent<Transform>().position.z + 2.3f);
                }
            }
            else if (hitInfo.collider.gameObject.name == trainCloneName)
            {
                onTrain = true;
            }
        }
        if (Physics.Raycast(transform.GetComponent<Transform>().position, Vector3.down, out hitInfo, 0.45f))
        {
            if (hitInfo.collider.gameObject.name != boxCloneName && hitInfo.collider.gameObject.name != trainCloneName &&
                transform.GetComponent<Transform>().position.y > 0.5f)
            {
                Destroy(this.gameObject);
                exit = true;
            }
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == playerName)
        {
            InGameUpdates.CoinsInCurrentRun++;
            Destroy(this.gameObject);
            exit = true;
        }
        else if (col.gameObject.name == trainCloneName)
        {
            transform.GetComponent<Transform>().position = new Vector3(coinX, 1.55f, transform.GetComponent<Transform>().position.z);
            coinY = 1.55f;
            onTrain = true;
        }
        else if (col.gameObject.name == boxCloneName)
        {
            transform.GetComponent<Transform>().position = new Vector3(coinX, 0.86f, transform.GetComponent<Transform>().position.z);
            coinY = 0.86f;
            onBox = true;
        }
        else
        {
            Destroy(this.gameObject);
            exit = true;
        }
    }
    private void OnCollisionExit(Collision col)
    {
        transform.GetComponent<Transform>().eulerAngles = new Vector3(0, transform.GetComponent<Transform>().eulerAngles.y, 90f);
        transform.GetComponent<Transform>().position = new Vector3(coinX, coinY, transform.GetComponent<Transform>().position.z);
        transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
    }
    void UpdateVer()
    {
        exit = false;
        acceleration = 0;
        playerName = "Adam";
        coinX = (float)Math.Round(transform.GetComponent<Transform>().position.x);
        coinY = 0.45f;
        onTrain = false;
        onBox = false;
        trainCloneName = "Train(Clone)";
        boxCloneName = "Crate(Clone)";
        distanceFromObstacles = 1f;
    }

}


