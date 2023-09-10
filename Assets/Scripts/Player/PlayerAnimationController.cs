using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerAnimationController : MonoBehaviour
{
    private PlayerInput _input;
    private PlayerController _player;
    private SpriteRenderer _renderer;
    private Animator _anim;
    private bool _isFacingRight = true;
    private int _currentAnimState;

    private bool _grounded;
    private bool _jumped;
    private bool _melee;
    private bool _shot;
    private bool _landed;

    private float _lockedAnimTime;

    private const float LANDED_FORCE = 20f;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Walk = Animator.StringToHash("Walk");
    private static readonly int Jump = Animator.StringToHash("Jump");

    void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _player = GetComponent<PlayerController>();
        _renderer = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _player.OnJump += () =>
        {
            _jumped = true;
        };
        _player.OnMelee += () =>
        {
            _melee = true;
        };
        _player.OnGroundedChange += (grounded, force) =>
        {
            _grounded = grounded;
            _landed = force >= LANDED_FORCE;
        };
    }

    private void Update()
    {
        int state = GetState();

        _jumped = false;
        _landed = false;
        _melee = false;

        if (state == _currentAnimState) return;
        _anim.CrossFade(state, 0, 0);
        _currentAnimState = state;
    }

    private int LockState(int state, float t)
    {
        _lockedAnimTime = Time.time + t;
        return state;
    }

    private int GetState()
    {
        if (Time.time < _lockedAnimTime) return _currentAnimState;

        return _input.inputStatus.Move.x == 0 ? Idle : Walk;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TurnCheck();
    }

    private void TurnCheck()
    {
        if (_input.inputStatus.Move.x > 0 && !_isFacingRight)
        {
            Turn();
        }
        else if (_input.inputStatus.Move.x < 0 && _isFacingRight)
        {
            Turn();
        }

    }

    private void Turn()
    {
        Vector3 rot = new Vector3(transform.rotation.x, _isFacingRight ? 180f : 0f, transform.rotation.z);
        transform.rotation = Quaternion.Euler(rot);
        _isFacingRight = !_isFacingRight;
    }
}
