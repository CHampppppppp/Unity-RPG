using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : GroundedState
{
    public MoveState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idelState);
        }

        if(!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

    }

    public override void Exit()
    {
        base.Exit();   
    }
}
