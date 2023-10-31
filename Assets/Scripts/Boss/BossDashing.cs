using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class BossDashing : BaseState
{

    private BossSM _sm;

    const float GROUND_SPEED = 10f;
    const float AIR_SPEED = 7f;
    const int SLAG_COOLDOWN = 800;
    const int ENDPOINT = 30;
    private Vector3 _targetPosition;
    private float _currentSpeed;
    private Stopwatch _sw;

    public BossDashing(BossSM stateMachine) : base("BossDashing", stateMachine)
    {
        _sm = stateMachine;
        _sw = new Stopwatch();
    }

    public override void Enter()
    {
        base.Enter();
        _targetPosition = _sm.transform.position + new Vector3(_sm.movedLeftLast ? ENDPOINT : -ENDPOINT, 0, 0);
        _sm.movedLeftLast = !_sm.movedLeftLast;
        _currentSpeed = _sm.movedUpLast ? AIR_SPEED : GROUND_SPEED;
        if (_sm.movedUpLast)
        {
            _sw.Restart();
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.transform.position = Vector3.MoveTowards(_sm.transform.position, _targetPosition, _currentSpeed * Time.deltaTime);

        if (Vector3.Distance(_sm.transform.position, _targetPosition) < 0.001f)
        {
            // UnityEngine.Debug.Log("finishedDashing");
            _sm.ChangeState(_sm.verticalState);
        }

        if (_sm.movedUpLast)
        {
            if (_sw.ElapsedMilliseconds > SLAG_COOLDOWN)
            {
                GameObject.Instantiate(_sm.slagPrefab, _sm.transform.position, Quaternion.identity);
                _sw.Restart();
            }
        }
    }
}
