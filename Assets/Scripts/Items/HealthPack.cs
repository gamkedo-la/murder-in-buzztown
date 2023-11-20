using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    private LifeManager lifeManager;

    private void Start() {
        lifeManager = FindObjectOfType<LifeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (lifeManager.IsLifeFull()) return;

        lifeManager.IncreaseLives();
        Destroy(gameObject);
    }
}
