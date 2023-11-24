using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using TMPro;

public class HealthFullMessage : MonoBehaviour
{

    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private float verticalSpeed = 2f;
    float timeAlive;
    [SerializeField] private TextMeshPro textContainer;

    void Awake()
    {
        timeAlive = 0;
    }

    void Update()
    {
        Vector3 newPos = transform.position;
        newPos.y += verticalSpeed * Time.deltaTime;
        transform.position = newPos;

        timeAlive += Time.deltaTime;

        textContainer.alpha = 1 - 2 * (timeAlive/verticalSpeed);

        if(timeAlive > lifeTime){
            Destroy(gameObject);
        }
    }
}
