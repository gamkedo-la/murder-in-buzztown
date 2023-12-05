using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    public event UnityAction activateLever;

    [SerializeField] bool isVisible = false;
    private bool hasBeenActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (hasBeenActivated) return;
        if (!isVisible) return;
        if (!other.CompareTag("PlayerBullet")) return;

        Destroy(other.gameObject);
        activateLever();
        hasBeenActivated = true;
    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
}
