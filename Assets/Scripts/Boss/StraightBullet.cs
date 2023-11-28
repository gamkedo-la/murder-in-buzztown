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
    }
}
