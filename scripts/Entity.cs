using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [Header("Collision info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;

    [Header("Knockback info")]
    [SerializeField] protected Vector2 knockbackDirection;
    protected bool isKnocked;

    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    #region components
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    public EntityFX fx { get; private set; }

    public SpriteRenderer sr { get; private set; }

    public CharacterStats stats { get; private set; }

    public CapsuleCollider2D cd { get; private set; }

    #endregion




    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFX>();
        stats = GetComponent<CharacterStats>();
        cd = GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update()
    {

    }




    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + facingDir * wallCheckDistance, wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion


    #region Flip
    public virtual void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }
    #endregion

    #region Velocity

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {

        if (isKnocked)
            return;

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }

    public void SetZeroVelocity()
    {
        if(isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(0, 0);
    }
    #endregion

    public virtual void DamageEffect(float _attackDir)//攻击方的朝向=受击方被击飞的方向
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockback",_attackDir);
    }


    protected virtual IEnumerator HitKnockback(float _attackDir)
    {
        isKnocked = true;

        rb.velocity = new Vector2(knockbackDirection.x * _attackDir, knockbackDirection.y);

        yield return new WaitForSeconds(0.1f); //0.07 default
        isKnocked = false;
    }

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }

    public virtual void Die()
    {

    }
}
