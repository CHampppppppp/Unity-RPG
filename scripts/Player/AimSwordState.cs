using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSwordState : PlayerStates
{
    public AimSwordState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();
        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(Input.GetKeyUp(KeyCode.Mouse1))
            stateMachine.ChangeState(player.idelState);
    }
}
