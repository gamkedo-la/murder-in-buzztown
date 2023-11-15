using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundScroller : MonoBehaviour
{

    private float length, startpos;
    public float scrollSpeedMultiplier = 0.5f;

    void Start()
    {
        startpos = transform.position.x;
    }

    void Update()
    {
        float dist = (Camera.main.transform.position.x * (1-scrollSpeedMultiplier));

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

    }
}

