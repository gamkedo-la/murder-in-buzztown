using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicWater : MonoBehaviour
{
    private bool isPlayerFallen = false;
    private LifeManager lifeManager;

    private void Start()
    {
        lifeManager = FindObjectOfType<LifeManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isPlayerFallen) return;
        isPlayerFallen = true;
        StartCoroutine(DrownAndDie());
    }

    private IEnumerator DrownAndDie()
    {
        yield return new WaitForSeconds(0.5f);

        lifeManager.KillPlayer();
        isPlayerFallen = false;
    }
}

