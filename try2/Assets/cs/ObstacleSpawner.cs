using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject train;
    public GameObject box;
    public GameObject Lizhol;
    public float nextSpawn;
    public int whatTemplate;
    int whereToSpawn;
    public static Vector3 temp;
    private Vector3 trainColliderSize, boxColliderSize, boxColliderCenter, trainColliderCenter;
    private float trainY, boxY, zhilaY;
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
            if (Time.time > nextSpawn)
            {
                whatTemplate = Random.Range(1, 7);
                switch (whatTemplate)
                {
                    case 1:
                        SpawnTrain(-1);
                        SpawnBox(1);
                        SpawnTrain(0);
                        break;
                    case 2:
                        SpawnTrain(1);
                        SpawnTrain(0);
                        SpawnBox(-1);
                        break;
                    case 3:
                        SpawnTrain(1);
                        SpawnBox(-0);
                        break;
                    case 4:
                        SpawnBox(1);
                        SpawnTrain(0);
                        SpawnTrain(-1);
                        break;
                    case 5:
                        SpawnTrain(1);
                        SpawnTrain(-1);
                        break;
                    case 6:
                        SpawnTrain(1);
                        SpawnTrain(0);
                        SpawnTrain(-1);
                        break;
                }
                if (Time.time < 15f)
                {
                    nextSpawn = Time.time + 3.5f;
                }
                else if (Time.time < 30f)
                {
                    nextSpawn = Time.time + 3.2f;
                }
                else
                {
                    nextSpawn = Time.time + 3f;
                }
            }
        }
    }
    //function that spawn one new  Train object
    void SpawnTrain(int where)
    {
        temp = new Vector3((float)where, trainY, transform.position.z);
        var trainTemp = Instantiate(train, temp, Quaternion.identity) as GameObject;
        trainTemp.AddComponent<BoxCollider>();
        trainTemp.AddComponent<Rigidbody>();
        trainTemp.GetComponent<Rigidbody>().useGravity = false;
        trainTemp.GetComponent<Rigidbody>().isKinematic = true;
        trainTemp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        trainTemp.GetComponent<BoxCollider>().isTrigger = false;
        trainTemp.GetComponent<BoxCollider>().size = trainColliderSize;
        trainTemp.GetComponent<BoxCollider>().center = trainColliderCenter;
        trainTemp.GetComponent<Transform>().localScale = new Vector3((float)1.5, (float)1, (float)1.5);
        trainTemp.AddComponent<ObstacleMovement>();
    }
    //function that spawn one new Box object
    void SpawnBox(int where)
    {
        temp = new Vector3((float)where, boxY, transform.position.z);
        var boxTemp = Instantiate(box, temp, Quaternion.identity) as GameObject;
        boxTemp.AddComponent<BoxCollider>();
        boxTemp.AddComponent<Rigidbody>();
        boxTemp.GetComponent<Rigidbody>().useGravity = false;
        boxTemp.GetComponent<Rigidbody>().isKinematic = true;
        boxTemp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        boxTemp.GetComponent<BoxCollider>().isTrigger = false;
        boxTemp.GetComponent<BoxCollider>().size = boxColliderSize;
        boxTemp.GetComponent<BoxCollider>().center = boxColliderCenter;
        boxTemp.GetComponent<Transform>().localScale = new Vector3((float)0.3, (float)0.3, (float)0.3);
        boxTemp.AddComponent<ObstacleMovement>();
    }

    void SpawnZhila(int where)
    {
        temp = new Vector3((float)where + 0.3f, zhilaY, transform.position.z);
        var zhilaTemp = Instantiate(Lizhol, temp, Quaternion.identity) as GameObject;
        zhilaTemp.AddComponent<MeshCollider>();
        zhilaTemp.AddComponent<Rigidbody>();
        zhilaTemp.GetComponent<MeshCollider>().convex = true;
        zhilaTemp.GetComponent<Rigidbody>().useGravity = false;
        zhilaTemp.GetComponent<Rigidbody>().isKinematic = true;
        zhilaTemp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        zhilaTemp.GetComponent<Transform>().localScale = new Vector3((float)0.04, (float)0.05, (float)0.04);
        zhilaTemp.AddComponent<ObstacleMovement>();
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        nextSpawn = 2f;
        trainColliderSize = new Vector3((float)0.6, (float)0.6, (float)2.55);
        boxColliderSize = new Vector3((float)2.3, (float)2.35, (float)2.3);
        boxColliderCenter = new Vector3((float)0, (float)1.145, (float)0);
        transform.position = new Vector3(0, 0, (float)12);
        trainColliderCenter = new Vector3(0, 0, 0.02f);
        trainY = 0.37f;
        boxY = -0.01f;
        zhilaY = 0.2f;
    }
}
