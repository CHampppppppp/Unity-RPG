using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnimationTrigger : MonoBehaviour
{
    private Enemy_Skeleton enemy => GetComponentInParent<Enemy_Skeleton>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
            if(hit.GetComponent<Player>() != null)
                hit.GetComponent<Player>().Damaged(enemy.facingDir);
    }

    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();

    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
