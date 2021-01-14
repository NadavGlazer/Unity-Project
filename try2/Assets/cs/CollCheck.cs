using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CollCheck : MonoBehaviour
{
    public static bool HasLost;
    public GameObject DeathPanel;
    public GameObject GamePanel;
    double topOfTrain, topOfCrate;
    string trainCloneName, crateCloneName;
    // Start is called before the first frame update
    void Start()
    {
        UpdateVer();
    }
    // Update is called once per frame
    void Update()
    {
        //waiting untill the death animation ends
        if (Animations.deathAnimationFinished)
        {
            Time.timeScale = 0f;
            DeathPanel.SetActive(true);
            GamePanel.SetActive(false);
        }
    }
    //checking for collisions
    private void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.name == trainCloneName && transform.position.y <= topOfTrain))
        {
            HasLost = true;
        }
        if (col.gameObject.name == crateCloneName && transform.position.y <= topOfCrate)
        {
            HasLost = true;
        }
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        HasLost = false;
        topOfCrate = 0.41;
        topOfTrain = 0.69;
        trainCloneName = "Train(Clone)";
        crateCloneName = "Crate(Clone)";
    }
}
