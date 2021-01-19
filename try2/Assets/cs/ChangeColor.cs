﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Material playerColor;

    // Start is called before the first frame update
    void Start()
    {
        playerColor.color = new Color32(
            (byte)AuthScript.Instance.GetUser().GetCurrent()[0],
            (byte)AuthScript.Instance.GetUser().GetCurrent()[1],
            (byte)AuthScript.Instance.GetUser().GetCurrent()[2],
            (byte)AuthScript.Instance.GetUser().GetCurrent()[3]);
    }
}
