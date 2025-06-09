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
    public float counterAttackDuration = .2f;
    public float throwDir;



    public bool isBusy {  get; private set; }  

    [Header("Move info")]
    public float moveSpeed = 10.5f;
    public float jumpForce = 8.52f;
    public float jumpSpeed = 8.53f;
    public float swordReturnImpact = 7;
    private float defaultMoveSpeed;
    private float defaultJumpForce;

    

    [Header("Dash info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir;
    private float defaultDashSpeed;
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

    public CounterAttackState counterAttackState { get; private set; }
    public AimSwordState aimSwordState { get; private set; }

    public CatchSwordState catchSwordState { get; private set; }

    public BlackHoleState blackHoleState { get; private set; }

    public DeadState deadState { get; private set; }    
    #endregion

    public SkillManager skill {  get; private set; }
    public GameObject sword {  get; private set; }

                                                                                        
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
        counterAttackState = new CounterAttackState(this, stateMachine, "counterAttack");
        aimSwordState = new AimSwordState(this, stateMachine, "aimSword");
        catchSwordState = new CatchSwordState(this, stateMachine, "catchSword");
        blackHoleState = new BlackHoleState(this, stateMachine, "jump");
        deadState = new DeadState(this, stateMachine,"die");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.StartMachine(airState);
        skill = SkillManager.instance;

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.F))
        {
            skill.crystal.CanUseSkill();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
            Inventory.instance.UseFlask();
    }

    public override void SlowEntityBy(float _slowPercentage, float _slowDuration)
    {
        moveSpeed = moveSpeed * (1 - _slowPercentage);
        jumpForce = jumpForce * (1 - _slowPercentage);
        dashSpeed = dashSpeed * (1 - _slowPercentage);

        anim.speed = anim.speed * (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", _slowDuration);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();//set anim default speed inside this function

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }


    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchSwordState);
        Destroy(sword);
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }
}
