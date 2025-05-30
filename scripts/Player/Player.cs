using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    #region Variables
    //ÉùÃ÷
    [Header("Attack details")]
    public Vector2[] attackMovement;
       
    public bool isBusy {  get; private set; }  

    [Header("Move info")]
    public float moveSpeed = 6f;
    public float jumpForce = 4f;
    public float jumpSpeed;

    

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir;
    #endregion


    #region states
    public StateMachine stateMachine { get; private set; }
    
    public IdelState idelState { get; private set; }
    public MoveState moveState { get; private set; }
    public JumpState jumpState { get; private set; }
    public DashState dashState { get; private set; }
    public AirState airState { get; private set; }

    public WallSlideState wallSlideState { get; private set; }
    public WallJumpState wallJumpState { get; private set; }    
    public PrimaryAttackState primaryAttackState { get; private set; }
    #endregion


                                                                                        
    //¶¨Òå
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new StateMachine();
        idelState = new IdelState(this, stateMachine,"idel");
        moveState = new MoveState(this, stateMachine,"move");
        jumpState = new JumpState(this, stateMachine, "jump");
        airState = new AirState(this, stateMachine, "jump");
        dashState = new DashState(this, stateMachine, "dash");
        wallSlideState = new WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new WallJumpState(this, stateMachine, "jump");
        primaryAttackState = new PrimaryAttackState(this, stateMachine, "attack");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.StartMachine(airState);
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
    }



    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
}
