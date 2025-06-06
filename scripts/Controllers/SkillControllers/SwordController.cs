using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private bool canRotate = true;
    private bool isReturning;

    private Player player;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale,Player _player)
    {

        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        anim.SetBool("isRotating", true);

        player = _player;
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        //rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
        canRotate = true;
        anim.SetBool("isRotating", true);

    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;//keep rotating man
        }

        if (isReturning)
        {
            ReturnHit();
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 1)
                player.CatchSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (isReturning)
        {
            return;//����;�в����ٴ���ײ
        }

        anim.SetBool("isRotating", false);


        canRotate = false;
        circleCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform; //������ײ������Ӷ��󣬸�����ײ�����λ�ñ仯���仯

        ThrowHit();
        
    }

    private void ThrowHit()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position,2);
        if (hit != null)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
                player.stats.DoDamage(enemy.GetComponent<CharacterStats>(), player.throwDir);
        }
    }

    private void ReturnHit()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position,.1f);//ֻ�Ը����ĵ���ɱ��һ�Σ���Ϊ�ص��뾶С������һֱ��⵽��ײ
        if (collision != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
                player.stats.DoDamage(enemy.GetComponent<CharacterStats>(), player.throwDir);

        }
    }
}
