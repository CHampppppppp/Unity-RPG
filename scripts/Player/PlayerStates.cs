using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerStates
{
    protected Player player;
    protected StateMachine stateMachine;
    private string aniBoolName;

    protected float xInput;
    protected float yInput;
    protected Rigidbody2D rb;
    protected CapsuleCollider2D cd;

    protected float stateTimer;
    protected bool triggerCalled;

    public PlayerStates(Player _player, StateMachine _stateMachine, string _aniBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.aniBoolName = _aniBoolName;
    }

    public virtual void Enter()
    {
        this.rb = this.player.rb;
        this.cd = PlayerManager.instance.player.GetComponent<CapsuleCollider2D>();
        player.anim.SetBool(aniBoolName, true);

        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        CheckDashInput();

        //if (stateTimer < -2)
        //{
        //    stateTimer = -3;//不忍心看它一直减下去-_-
        //}

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);

    }
    public virtual void Exit()
    {
        player.anim.SetBool(aniBoolName, false);

    }
    protected void CheckDashInput()
    {
        if (player.IsWallDetected())
            return;

        player.dashDir = Input.GetAxisRaw("Horizontal");

        if (player.dashDir == 0)//默认往前冲刺
        {
            player.dashDir = player.facingDir;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill())
        {
            stateMachine.ChangeState(player.dashState);
        }
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
