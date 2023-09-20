using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDashing : BaseState
{

    private BossSM _sm;

    const float SPEED = 5f;
    const int ENDPOINT = 10;
    private Vector3 _targetPosition;


    public BossDashing(BossSM stateMachine) : base("BossDashing", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _targetPosition = _sm.transform.position + new Vector3(_sm.movedLeftLast ? ENDPOINT : -ENDPOINT, 0, 0);
        _sm.movedLeftLast = !_sm.movedLeftLast;
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.transform.position = Vector3.MoveTowards(_sm.transform.position, _targetPosition, SPEED * Time.deltaTime);

        if (Vector3.Distance(_sm.transform.position, _targetPosition) < 0.001f)
        {
            UnityEngine.Debug.Log("finishedDashing");
            _sm.ChangeState(_sm.idleState);
        }
    }
}
