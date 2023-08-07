using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerInput _input; 
    private bool _isFacingRight = true;
    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TurnCheck();
    }

    private void TurnCheck()
    {
        if(_input.inputStatus.Move.x > 0  && !_isFacingRight)
        {
            Turn();
        } else if (_input.inputStatus.Move.x < 0 && _isFacingRight)
        {
            Turn();
        }

    }

    private void Turn()
    {
        Debug.Log("volteo");
        Vector3 rot = new Vector3(transform.rotation.x, _isFacingRight ? 180f : 0f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(rot);
        _isFacingRight = !_isFacingRight;
    }
}
