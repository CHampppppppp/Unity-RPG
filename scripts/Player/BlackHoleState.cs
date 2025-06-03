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
    }

    public override void Update()
    {
        base.Update();


        if (stateTimer > 0)
            rb.velocity = new Vector2(0, 15);

        if (stateTimer < 0)
        {
            rb.velocity = new Vector2(0, -1f);

            if (!skillUsed)
            {
                if (player.skill.blackHole.CanUseSkill())
                {
                    player.skill.blackHole.UseSkill();

                    skillUsed = true;
                }
            }
        }

        // 当黑洞中的所有攻击结束后，退出当前状态（blackHoleController.cs）


    }
}
