using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] private float returnSpeed = 12;
    private Animator anim;
    [SerializeField] private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale,Player _player)
    {
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
        player = _player;
    }

    public void ReturnSword()
    {
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.velocity;//keep rotating man

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if(Vector2.Distance(transform.position, player.transform.position) < 1)
                player.ClearSword();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collided with: " + collision.gameObject.name);

        canRotate = false;
        circleCollider.enabled = false;
        Debug.Log($"canRotate: {canRotate}, circleCollider.enabled: {circleCollider.enabled}");

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform; //剑是碰撞对象的子对象，跟着碰撞对象的位置变化而变化
    }
}
