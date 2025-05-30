using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkeletonIdelState : SkeletonGroundedState
{
    public SkeletonIdelState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName, enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Enter idelState!");
        enemy.SetZeroVelocity();
        stateTimer = enemy.idelTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            enemy.Flip();
            stateMachine.ChangeState(enemy.moveState);
        }
    }
}
