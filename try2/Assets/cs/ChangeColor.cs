using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Material playerColor;
    
    // Start is called before the first frame update
    void Start()
    {
        playerColor.color = new Color32(
            (byte)AuthScript.instance.user.CurrentColor[0],
            (byte)AuthScript.instance.user.CurrentColor[1],
            (byte)AuthScript.instance.user.CurrentColor[2],
            (byte)AuthScript.instance.user.CurrentColor[3]);       
    }

    // Update is called once per frame

}
