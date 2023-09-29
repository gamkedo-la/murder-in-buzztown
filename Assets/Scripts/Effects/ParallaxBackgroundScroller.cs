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
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (Camera.main.transform.position.x * (1 - scrollSpeedMultiplier));
        float dist = (Camera.main.transform.position.x * scrollSpeedMultiplier);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}

