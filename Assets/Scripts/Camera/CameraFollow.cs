using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _smoothTime;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _checkDistance;
    [SerializeField] private float _checkSpeed;

    private Vector3 _velocityOffset;
    private Vector3 _velocity;
    private Vector3 _checkVelocity;


    private void LateUpdate()
    {

        Vector2 nextPos = _player.GetComponent<Rigidbody2D>().velocity.normalized * _checkDistance;
        _velocityOffset = Vector3.SmoothDamp(_velocityOffset, nextPos, ref _checkVelocity, _checkSpeed);


        Vector3 target = _player.position + _offset + _velocityOffset;
        target.z = -10;
        transform.position = Vector3.SmoothDamp(transform.position, target, ref _velocity, _smoothTime);
    }
}
