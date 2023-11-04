using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Void : MonoBehaviour
{
    [SerializeField] private Transform respawnLocation;
    [SerializeField] private float timeToRespawn = 1f;
    private Transform playerTransform;
    private bool isPlayerFallen = false;
    private CheckpointManager checkpointManager;
    private LifeManager lifeManager;

    private void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        checkpointManager = FindObjectOfType<CheckpointManager>();
        lifeManager = FindObjectOfType<LifeManager>();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (playerTransform == null) return;

        transform.position = new Vector2(
            playerTransform.position.x,
            transform.position.y
        );
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (!other.CompareTag("Player") || isPlayerFallen) return;
        Debug.Log("Player is falling in the void");

        isPlayerFallen = true;
        lifeManager.KillPlayer();
        StartCoroutine(ResetIsFallen());
    }

    private IEnumerator ResetIsFallen()
    {
        yield return new WaitForSeconds(0.25f);
        isPlayerFallen = false;
    }
}
