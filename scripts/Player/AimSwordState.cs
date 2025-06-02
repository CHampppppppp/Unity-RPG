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

        player.skill.sword.DotsActive(true);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .2f);
    }

    public override void Update()
    {
        base.Update();
        player.SetZeroVelocity();

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            stateMachine.ChangeState(player.idelState);
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if ((player.transform.position.x > mousePosition.x && player.facingDir == 1) ||
            (player.transform.position.x < mousePosition.x && player.facingDir == -1))
            player.Flip();
    }
}
