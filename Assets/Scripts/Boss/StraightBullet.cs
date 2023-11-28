using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : MonoBehaviour
{
    private Vector3 _target;
    const float SPEED = 10f;

    public void SetTarget(Vector3 pos)
    {
        _target = pos;
        Vector2 direction = _target - transform.position;
        GetComponent<Rigidbody2D>().velocity = direction.normalized * SPEED;
        Invoke("DestroySelf", 8f); // in case it doesn't collide with anything just kill it
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss")) return;
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.DecreaseLives();
            Vector2 direction = new Vector2(transform.position.x < other.transform.position.x ? 1 : -1, 0);
            other.GetComponent<PlayerController>().ApplyPushBack(direction, true);
        }
        DestroySelf();
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
