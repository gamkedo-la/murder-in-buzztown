using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private Checkpoint currentCheckpoint;
    private CinemachineBrain brain;
    private Transform player;

    private void Start() 
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        brain = FindObjectOfType<CinemachineBrain>();
    }

    public void UpdateCurrentCheckoint(Checkpoint newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public void SpawnPlayerAtCurrentCheckpoint()
    {
        player.position = currentCheckpoint.transform.position;
        brain.ActiveVirtualCamera.Follow = player;
    }
}
