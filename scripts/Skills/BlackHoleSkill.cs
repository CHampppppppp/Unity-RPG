using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skills
{
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneCooldown;

    [SerializeField] private GameObject blackHolePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;


    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackHole = Instantiate(blackHolePrefab);

        BlackHoleController newBlackHoleController = newBlackHole.GetComponent<BlackHoleController>();

        newBlackHoleController.SetupBlackHole(maxSize,growSpeed,shrinkSpeed,amountOfAttack,cloneCooldown);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

}
