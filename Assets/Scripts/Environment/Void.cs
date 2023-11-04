using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Void : MonoBehaviour
{
    [SerializeField] private Transform respawnLocation;
    [SerializeField] private float timeToRespawn = 1f;
    private Transform playerTransform;
    private CinemachineBrain brain;
    private bool isPlayerFallen = false;

    private void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        brain = FindObjectOfType<CinemachineBrain>();
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(
            playerTransform.position.x,
            transform.position.y
        );
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player") || isPlayerFallen) return;
        isPlayerFallen = true;
        StartCoroutine(RespawnPlayerAfterDelay(other.gameObject));
    }

    private IEnumerator RespawnPlayerAfterDelay(GameObject other)
    {
        brain.ActiveVirtualCamera.Follow = null;
        yield return new WaitForSeconds(timeToRespawn);
        other.transform.position = respawnLocation.position;
        isPlayerFallen = false;
        brain.ActiveVirtualCamera.Follow = other.transform;
    }
}
