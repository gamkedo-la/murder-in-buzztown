using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Void : MonoBehaviour
{
    [SerializeField] private Transform respawnLocation;
    private Transform playerTransform;
    private bool isPlayerFallen = false;

    private void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            playerTransform.position.x,
            transform.position.y
        );
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (!other.CompareTag("Player")) return;
        other.transform.position = respawnLocation.position;
        isPlayerFallen = false;
    }
}
