using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("enter dead state");

        enemy.anim.SetBool(enemy.lastAnimBoolName, true);

        enemy.anim.speed = 0;
        enemy.cd.enabled = false;

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -20);
        }
        else
        {
            rb.velocity = new Vector2(0, 10);
        }
    }
}
