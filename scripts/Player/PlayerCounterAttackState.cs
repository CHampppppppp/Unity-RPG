using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCounterAttackState : PlayerStates
{
    public PlayerCounterAttackState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;

        player.anim.SetBool("successfulCounterAttack", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                if (hit.GetComponent<Enemy>().CanBeStunned())
                {
                    stateTimer = 10;
                    player.anim.SetBool("successfulCounterAttack", true);
                }
            }
        }
            

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idelState);
    }
}
