using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : PlayerStates
{
    public DashState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.clone.CreateCloneOnDashStart();

        stateTimer = player.dashDuration;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(player.moveState);

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);
    }

    public override void Exit()
    {
        base .Exit();

        player.skill.clone.CreateCloneOnDashOver();
        
    }
}
