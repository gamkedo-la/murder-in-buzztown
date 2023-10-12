using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicWater : MonoBehaviour
{
    [SerializeField] Transform respawnLocation;
    [SerializeField] float timeToRespawn = 1f;
    [SerializeField] ChangeCamera cameraChangeZone;
    private bool isPlayerFallen = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || isPlayerFallen) return;
        isPlayerFallen = true;
        cameraChangeZone.SetIsPlayerDying(true);
        StartCoroutine(RespawnPlayerAfterDelay(other.gameObject));
    }

    private IEnumerator RespawnPlayerAfterDelay(GameObject other)
    {
        yield return new WaitForSeconds(timeToRespawn);
        other.transform.position = respawnLocation.position;
        cameraChangeZone.SetIsPlayerDying(false);
        isPlayerFallen = false;
    }
}

