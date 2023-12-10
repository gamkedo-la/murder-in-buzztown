using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSM : StateMachine
{
    [HideInInspector] public BossIdle idleState;
    // [HideInInspector] public BossMoving movingState;
    [HideInInspector] public BossDashing dashingState;
    [HideInInspector] public BossVertical verticalState;
    [HideInInspector] public BossShooting shootingState;
    public AudioClip bulletClip;
    public AudioClip jetpackClip;
    BossSM _sm;
    public Transform playerTransform;
    public bool movedLeftLast;
    public bool movedUpLast;
    public GameObject bulletPrefab;
    public GameObject slagPrefab;
    public BossLifeController lifeController;
    public bool finishedTalking;
    // public Animator anim;

    private void Awake()
    {
        idleState = new BossIdle(this);
        dashingState = new BossDashing(this);
        verticalState = new BossVertical(this);
        shootingState = new BossShooting(this);
        // anim = GetComponent<Animator>();
        _sm = this;
        movedLeftLast = false;
        movedUpLast = false;
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }

    public void Restart()
    {
        transform.position = new Vector3(-320, 2, 0f);
        lifeController.Restart();
        movedLeftLast = false;
        movedUpLast = false;
        finishedTalking = false;
        _sm.ChangeState(idleState);
    }


}

