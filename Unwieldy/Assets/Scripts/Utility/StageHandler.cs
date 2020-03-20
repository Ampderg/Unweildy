using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageHandler : MonoBehaviour
{

    public static StageHandler instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public Vector3 GetRespawnPosition()
    {
        return Vector3.up * 20;
    }
}
