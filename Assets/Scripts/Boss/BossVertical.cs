using UnityEngine;

public class BossVertical : BaseState
{

    private BossSM _sm;

    const float SPEED = 3f;
    const int ENDPOINT = 4;
    private Vector3 _targetPosition;


    public BossVertical(BossSM stateMachine) : base("BossVertical", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _targetPosition = _sm.transform.position + new Vector3(0, _sm.movedUpLast ? -ENDPOINT : ENDPOINT, 0);
        _sm.movedUpLast = !_sm.movedUpLast;
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        _sm.transform.position = Vector3.MoveTowards(_sm.transform.position, _targetPosition, SPEED * Time.deltaTime);

        if (Vector3.Distance(_sm.transform.position, _targetPosition) < 0.001f)
        {
            // UnityEngine.Debug.Log("finishedVertical");
            _sm.ChangeState(_sm.shootingState);
        }
    }
}
