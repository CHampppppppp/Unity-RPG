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

        //Debug.Log("blackhole skill used!");

        GameObject newBlackHole = Instantiate(blackHolePrefab,player.transform.position,Quaternion.identity);

        //if (newBlackHole != null) Debug.Log("newBlackHole created!");

        BlackHoleController newBlackHoleController = newBlackHole.GetComponent<BlackHoleController>();
        //if (newBlackHoleController != null) Debug.Log("newBlackHoleController get!");

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
