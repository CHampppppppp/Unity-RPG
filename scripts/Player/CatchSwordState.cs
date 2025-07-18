using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchSwordState : PlayerStates
{

    private Transform sword;

    public CatchSwordState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        sword = player.sword.transform;

        if ((player.transform.position.x > sword.position.x && player.facingDir == 1) ||
            (player.transform.position.x < sword.position.x && player.facingDir == -1))
            player.Flip();

        rb.velocity = new Vector2(player.swordReturnImpact * -player.facingDir,rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .1f);
    }

    public override void Update()
    {
        base.Update();

        if(triggerCalled)
            stateMachine.ChangeState(player.idelState);
    }
}
