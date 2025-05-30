using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlideState : PlayerStates
{

    private float wallSlideSpeed = -1f;
    public WallSlideState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
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

        if (Input.GetKey(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if ((xInput != 0 && player.facingDir != xInput) || player.IsGroundDetected())
            stateMachine.ChangeState(player.idelState);

        if (xInput != 0 && xInput == player.facingDir)
        {
            rb.velocity = new Vector2(0, wallSlideSpeed);
            return;
        }

        if (xInput == 0) rb.velocity = new Vector2(0, wallSlideSpeed * 4f);

    }
}
