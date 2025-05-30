using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryAttackState : PlayerStates
{

    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = 2;
    public PrimaryAttackState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("comboCounter",comboCounter);

        float attackDir = player.facingDir;

        if(xInput != 0)
            attackDir = xInput;
        
        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);


        player.anim.speed = 1.2f;

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.SetZeroVelocity();
        if(triggerCalled)
            stateMachine.ChangeState(player.idelState);
    }
}
