using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{

    private Player player;

    protected override void Start()
    {
        base.Start();

        player = GetComponent<Player>();
    }

    public override void TakeDamage(int _damage, float _attackDir)
    {
        base.TakeDamage(_damage, _attackDir);
        player.DamageEffect(_attackDir);
    }

    protected override void Die()
    {
        base.Die();
        
        player.Die();
    }
}
