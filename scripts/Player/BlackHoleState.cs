using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleState : PlayerStates
{

    private float flyTime = .4f;
    private bool skillUsed = false;
    private float defaultGravity;

    public BlackHoleState(Player _player, StateMachine _stateMachine, string _aniBoolName) : base(_player, _stateMachine, _aniBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        stateTimer = flyTime;
        rb.gravityScale = 0;

        //player.skill.blackHole.UseSkill();
        
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;
        player.MakeTransparent(false);
        player.GetComponentInChildren<Canvas>().enabled = true;// hp bar
        skillUsed = false;
    }

    public override void Update()
    {
        base.Update();


        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);
            cd.enabled = false;
        }

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -.5f);

            if (!skillUsed)
            {
                if (player.skill.blackHole.CanUseSkill())
                {
                    //player.skill.blackHole.UseSkill();

                    skillUsed = true;
                }
            }
        }

        if (player.skill.blackHole.SkillCompleted())
        {
            cd.enabled = true;
            stateMachine.ChangeState(player.airState);
        }


    }
}
