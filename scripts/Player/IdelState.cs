using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdelState : GroundedState
{
    public IdelState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug
        player.SetZeroVelocity();//冰面的话另说
    }

    public override void Update()
    {
        base.Update();
        //player.SetVelocity(xInput * player.moveSpeed, player.rb.velocity.y); 不需要
        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    public override void Exit()
    {

        base.Exit();
    }
}
