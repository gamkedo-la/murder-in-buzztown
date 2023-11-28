using System;
using UnityEngine;

public class MenuPlayerController : PlayerController
{
    protected new void FixedUpdate()
    {
        _currentFrame++;
        HandleCameraLerp();
        HandleCollisions();
        HandleJump();
        HandleDash();
        HandleMelee();

        HandleMovement();
        ApplyMovement();
    }

    protected new void Update()
    {
        _inputStatus = _input.inputStatus;

        if (_inputStatus.JumpPushed)
        {
            _hasJump = true;
            _frameJump = _currentFrame;
            AudioManager.Instance.PlayEffect(AudioManager.Instance.jumpAudioClip);
        }

        if (_inputStatus.Move.x != 0)
        {
            _applyFriction = false;
        }

        if (_inputStatus.DashPushed)
        {
            _hasDash = true;
        }

        if (_inputStatus.MeleePushed)
        {
            _hasMelee = true;
        }


        if (_inputStatus.ShootPushed)
        {
            _hasShoot = true;
        }
        if (_inputStatus.InteractPushed)
        {
            _hasInteracted = true;
        }

    }

}
