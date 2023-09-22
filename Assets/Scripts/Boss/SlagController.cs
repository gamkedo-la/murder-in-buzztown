using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlagController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Slag hit player");
        }
        if (other.CompareTag("Ground"))
        {
            Debug.Log("Slag hit ground");
        }
    }
}
