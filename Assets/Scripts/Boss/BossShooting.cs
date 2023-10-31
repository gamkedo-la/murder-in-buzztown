using UnityEngine;
using System.Diagnostics;

public class BossShooting : BaseState
{

    private BossSM _sm;
    private Stopwatch _sw;

    private int _bulletCount = 0;
    const int BULLET_COOLDOWN = 600;
    const int MAX_BULLETS_SHOT = 3;

    public BossShooting(BossSM stateMachine) : base("BossShooting", stateMachine)
    {
        _sm = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        _bulletCount = 0;
        _sw = new Stopwatch();
        _sw.Start();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (_sw.ElapsedMilliseconds > BULLET_COOLDOWN && _bulletCount < MAX_BULLETS_SHOT)
        {
            // UnityEngine.Debug.Log("shooting");
            GameObject bullet = GameObject.Instantiate(_sm.bulletPrefab, _sm.transform.position, Quaternion.identity);
            bullet.GetComponent<StraightBullet>().SetTarget(_sm.playerTransform.position);
            _sw.Restart();
            _bulletCount++;
        }
        if (_bulletCount == MAX_BULLETS_SHOT)
        {
            _sm.ChangeState(_sm.idleState);
        }
    }
}

