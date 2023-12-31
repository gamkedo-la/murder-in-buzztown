using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private CheckpointManager checkpointManager;

    private void Start()
    {
        checkpointManager = FindObjectOfType<CheckpointManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        checkpointManager.UpdateCurrentCheckoint(this);
    }
}
