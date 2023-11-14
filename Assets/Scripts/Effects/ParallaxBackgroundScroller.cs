using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackgroundScroller : MonoBehaviour
{

    private float length, startpos;
    public float scrollSpeedMultiplier = 0.5f;

    // this is a bit buggy - we just make the sprites really large instead
    public Boolean loopBasedOnSpriteWidth = false; 


    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (Camera.main.transform.position.x * (1 - scrollSpeedMultiplier));
        float dist = (Camera.main.transform.position.x * scrollSpeedMultiplier);

        // FIXME: this is buggy! backwards from reality, where background moves SLOWER than foreground
        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (loopBasedOnSpriteWidth) {
            if (temp > startpos + length) startpos += length;
            else if (temp < startpos - length) startpos -= length;
        }
    }
}

