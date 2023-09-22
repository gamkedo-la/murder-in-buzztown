using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 spinSpeeds = new Vector3(0,0,100);
    
    void Update()
    {
        transform.Rotate(spinSpeeds * Time.deltaTime); 
    }
}
