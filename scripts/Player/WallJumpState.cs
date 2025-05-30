using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpState : PlayerStates
{
    public WallJumpState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = .4f;
        player.SetVelocity(4 * -player.facingDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0) 
            stateMachine.ChangeState(player.airState);

        if(player.IsGroundDetected())
            stateMachine.ChangeState(player.idelState);
    }
}
