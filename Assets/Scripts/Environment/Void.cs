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
    private CheckpointManager checkpointManager;
    private LifeManager lifeManager;

    private void Start() 
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        brain = FindObjectOfType<CinemachineBrain>();
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
        isPlayerFallen = true;
        brain.ActiveVirtualCamera.Follow = null;
        lifeManager.KillPlayer();
    }
}
