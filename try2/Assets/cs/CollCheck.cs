using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CollCheck : MonoBehaviour
{
    public static bool HasLost;
    public GameObject DeathPanel;
    public GameObject GamePanel;
    // Start is called before the first frame update
    void Start()
    {
        UpdateVer();
    }
    // Update is called once per frame
    void Update()
    {
        //waiting untill the death animation ends
        if (Animations.booliann)
        {
            Time.timeScale = 0f;
            DeathPanel.SetActive(true);
            GamePanel.SetActive(false);
        }
    }
    //checking for collisions
    private void OnCollisionEnter(Collision col)
    {
        if ((col.gameObject.name == "Train(Clone)" && transform.position.y <= 0.69))
        {
            HasLost = true;
            print("true");
        }
        if (col.gameObject.name == "Crate(Clone)" && transform.position.y <= 0.41)
        {
            HasLost = true;
            print("true");
        }
    }
    //function that sets the starting values of the objects
    void UpdateVer()
    {
        HasLost = false;
    }
}
