using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPose : MonoBehaviour
{
    static DeathPose instance;
     private void Awake()
    {
        DontDestroyOnLoad(this);

    }
}
