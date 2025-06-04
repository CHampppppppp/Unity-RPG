
using UnityEngine;

public class CounterAttackState : PlayerStates
{

    private bool canCreateClone;


    public CounterAttackState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        canCreateClone = true;

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

                    if (canCreateClone)
                    {
                        canCreateClone = false;
                        player.skill.clone.CreateCloneOnCounterAttack(hit.transform);

                    }
                }
            }
        }
            

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idelState);
    }
}
