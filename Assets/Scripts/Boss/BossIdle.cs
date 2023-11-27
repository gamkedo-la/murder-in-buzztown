using System.Diagnostics;

public class BossIdle : BaseState
{
    private BossSM _sm;
    private Stopwatch sw;


    public BossIdle(BossSM stateMachine) : base("BossIdle", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sw = new Stopwatch();
        sw.Start();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (!_sm.finishedTalking) return;
        if (sw.ElapsedMilliseconds > 400)
        {
            // UnityEngine.Debug.Log("finishedIdle");
            _sm.ChangeState(_sm.dashingState);
        }
    }
}