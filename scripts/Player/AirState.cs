using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirState : PlayerStates
{
    public AirState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.jumpSpeed, rb.velocity.y);
        

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idelState);
        }

        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);
    }
}
