using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class JumpState : PlayerStates
{

    public JumpState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(xInput * player.jumpSpeed, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.jumpSpeed, rb.velocity.y); 

        if (rb.velocity.y < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if(player.IsWallDetected())
            stateMachine.ChangeState(player.wallSlideState);
    }
}
