using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthPack : MonoBehaviour
{
    private LifeManager lifeManager;
    [SerializeField] private GameObject healthFullMessage;
    float messageDelay = 10f;
    float lastMessage = 10f;

    private void Start() {
        lifeManager = FindObjectOfType<LifeManager>();
    }

    private void Update() {
        lastMessage += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (lifeManager.IsLifeFull()){
            if(lastMessage > messageDelay){
                Instantiate(healthFullMessage, transform.position, Quaternion.identity);
                lastMessage = 0;
            }
            return;
        } 

        lifeManager.IncreaseLives();
        Destroy(gameObject);
    }
}
