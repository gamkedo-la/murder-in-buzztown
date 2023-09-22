using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : MonoBehaviour
{
    private Vector3 _target;
    const float SPEED = 5f;

    public void SetTarget(Vector3 pos)
    {
        _target = pos;
        Debug.Log(_target);
    }

    // Update is called once per frame
    void Update()
    {
        if (_target != Vector3.zero)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, SPEED * Time.deltaTime);
        }
    }
}
