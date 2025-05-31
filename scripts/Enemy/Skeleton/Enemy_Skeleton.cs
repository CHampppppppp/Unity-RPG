using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Skeleton : Enemy
{

    #region States
    public SkeletonIdelState idelState {  get; private set; }
    public SkeletonMoveState moveState { get; private set; }
    public SkeletonBattleState battleState { get; private set; }
    public SkeletonStunState stunState { get; private set; }
    public SkeletonAttackState attackState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        idelState = new SkeletonIdelState(this, stateMachine, "idel", this);
        moveState = new SkeletonMoveState(this, stateMachine, "move", this);
        battleState = new SkeletonBattleState(this, stateMachine, "move", this);
        attackState = new SkeletonAttackState(this, stateMachine, "attack", this);
        stunState = new SkeletonStunState(this, stateMachine, "stun", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.StartMachine(idelState);
    }

    protected override void Update()
    {
        base.Update();

        if(Input.GetKeyDown(KeyCode.U))
            stateMachine.ChangeState(stunState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }
}
