using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.isTrigger) return;
        
        Debug.Log(other.name);
        Vector2 direction = new Vector2(other.transform.position.x < _player.transform.position.x ? 1 : -1, 0);
        _player.ApplyPushBack(direction);
    }
}
