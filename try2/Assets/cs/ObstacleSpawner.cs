using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static bool CanSpawn;
    public GameObject Train;
    public GameObject Box;
    public GameObject Coin;
    float nextSpawn;
    float nextCoinSpawn;
    float trainY;
    float boxY;
    float coinY;
    float coinZRotation;
    float coinLightRange;
    float coinLightIntensity;
    int whatTemplate;
    int whereToSpawn;
    bool first;
    Vector3 temp;
    Vector3 trainColliderSize;
    Vector3 boxColliderSize;
    Vector3 coinColliderSize;
    Vector3 boxColliderCenter;
    Vector3 trainColliderCenter;
    Vector3 coinColliderCenter;
    Vector3 trainLocalScale;
    Vector3 boxLocalScale;
    Vector3 coinLocalScale;
    Color32 coinLightColor;
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
            if (Time.time > nextSpawn && CanSpawn)
            {
                whatTemplate = Random.Range(1, 6);
                switch (whatTemplate)
                {
                    case 1:
                        SpawnTrain(-1f);
                        SpawnCrate(1f);
                        SpawnTrain(0);
                        break;
                    case 2:
                        SpawnTrain(1f);
                        SpawnTrain(0);
                        SpawnCrate(-1f);
                        break;
                    case 3:
                        SpawnTrain(1f);
                        SpawnCrate(0);
                        break;
                    case 4:
                        SpawnCrate(1f);
                        SpawnTrain(0);
                        SpawnTrain(-1f);
                        break;
                    case 5:
                        SpawnTrain(1f);
                        SpawnTrain(-1f);
                        break;
                }

                if (first)
                {
                    nextSpawn = Time.time + 4f;
                    first = false;
                }
                else
                {
                    nextSpawn = Time.time + Random.Range(1f, 3.5f);
                }

                CanSpawn = false;
            }
            else if (Time.time > nextCoinSpawn)
            {
                switch (whatTemplate)
                {
                    case 1:
                        SpawnCoin(0);
                        break;
                    case 2:
                        SpawnCoin((float)Random.Range(-1, 2));
                        break;
                    case 3:
                        SpawnCoin(-1f);
                        break;
                    case 4:
                        SpawnCoin((float)Random.Range(-1, 1));
                        break;
                    case 5:
                        SpawnCoin(0);
                        break;
                }
                nextCoinSpawn = Time.time + 2f;
            }
        }
    }
    //function that spawn one new  Train object
    void SpawnTrain(float where)
    {
        temp = new Vector3(where, trainY, transform.position.z);
        var trainTemp = Instantiate(Train, temp, Quaternion.identity) as GameObject;
        trainTemp.AddComponent<BoxCollider>();
        trainTemp.AddComponent<Rigidbody>();
        trainTemp.GetComponent<Rigidbody>().useGravity = false;
        trainTemp.GetComponent<Rigidbody>().isKinematic = true;
        trainTemp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        trainTemp.GetComponent<BoxCollider>().isTrigger = false;
        trainTemp.GetComponent<BoxCollider>().size = trainColliderSize;
        trainTemp.GetComponent<BoxCollider>().center = trainColliderCenter;
        trainTemp.GetComponent<Transform>().localScale = trainLocalScale;
        trainTemp.AddComponent<ObstacleMovement>();
    }
    //function that spawn one new Box object
    void SpawnCrate(float where)
    {
        temp = new Vector3(where, boxY, transform.position.z);
        var boxTemp = Instantiate(Box, temp, Quaternion.identity) as GameObject;
        boxTemp.AddComponent<BoxCollider>();
        boxTemp.AddComponent<Rigidbody>();
        boxTemp.GetComponent<Rigidbody>().useGravity = false;
        boxTemp.GetComponent<Rigidbody>().isKinematic = true;
        boxTemp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        boxTemp.GetComponent<BoxCollider>().isTrigger = false;
        boxTemp.GetComponent<BoxCollider>().size = boxColliderSize;
        boxTemp.GetComponent<BoxCollider>().center = boxColliderCenter;
        boxTemp.GetComponent<Transform>().localScale = boxLocalScale;
        boxTemp.AddComponent<ObstacleMovement>();
    }
    void SpawnCoin(float where)
    {
        temp = new Vector3(where, coinY, transform.position.z);
        var coinTemp = Instantiate(Coin, temp, Quaternion.identity) as GameObject;
        coinTemp.AddComponent<BoxCollider>();
        coinTemp.GetComponent<BoxCollider>().isTrigger = false;
        coinTemp.GetComponent<BoxCollider>().size = coinColliderSize;
        coinTemp.GetComponent<BoxCollider>().center = coinColliderCenter;
        coinTemp.AddComponent<Rigidbody>();
        coinTemp.GetComponent<Rigidbody>().useGravity = false;
        coinTemp.GetComponent<Rigidbody>().isKinematic = false;
        coinTemp.GetComponent<Transform>().localScale = coinLocalScale;
        coinTemp.GetComponent<Transform>().Rotate(0, 0, coinZRotation);
        coinTemp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        GameObject light = GameObject.Find("Point");
        light.GetComponent<Light>().range = coinLightRange;
        light.GetComponent<Light>().intensity = coinLightIntensity;
        light.GetComponent<Light>().color = coinLightColor;
        light.gameObject.name = "fixedLight";
        coinTemp.AddComponent<CoinsAction>();

    }

    //function that sets the starting values of the objects
    void UpdateVer()
    {
        trainColliderSize = new Vector3((float)0.6, (float)0.6, (float)2.55);
        trainColliderCenter = new Vector3(0, 0, 0.02f);
        trainLocalScale = new Vector3((float)1.8, (float)2, (float)1.8);
        trainY = 0.63f;

        boxColliderSize = new Vector3((float)2.3, (float)2.35, (float)2.3);
        boxColliderCenter = new Vector3((float)0, (float)1.145, (float)0);
        boxLocalScale = new Vector3((float)0.3, (float)0.3, (float)0.3);
        boxY = -0.01f;

        coinColliderSize = new Vector3(0.03f, 0.03f, 0.005f);
        coinColliderCenter = new Vector3(0, 0, 0);
        coinLocalScale = new Vector3(20f, 20f, 20f);
        coinY = 0.45f;
        coinZRotation = 90f;
        coinLightColor = new Color32((byte)125, (byte)116, (byte)51, (byte)255);
        coinLightRange = 1f;
        coinLightIntensity = 3f;
        transform.position = new Vector3(0, 0, (float)12);

        nextSpawn = 0f;
        nextSpawn = 1f;
        CanSpawn = true;
        first = true;
    }
}
