using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skills
{
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalDuration;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;

    [Header("Explosive crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi crystal")]
    [SerializeField] private bool canUseMultiCrystal;
    [SerializeField] private int amountOfCrystal;
    [SerializeField] private float multiCrystalCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();


    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal())//如果
            return;

        if(currentCrystal == null)//创建水晶
        {
            currentCrystal = Instantiate(crystalPrefab,player.transform.position,Quaternion.identity);
            CrystalController currentCrystalController = currentCrystal.GetComponent<CrystalController>();

            currentCrystalController.SetupCrystal(crystalDuration,canExplode,canMoveToEnemy,moveSpeed,FindClosestEnemy(currentCrystal.transform));
        }
        else//如果有了水晶，移形换影
        {
            if (canMoveToEnemy)//如果水晶能动就不能移形换位（payback）
                return;

            Vector2 playerPos = player.transform.position;
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerPos;

            if(cloneInsteadOfCrystal)
            {
                SkillManager.instance.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                currentCrystal.GetComponent<CrystalController>()?.FinishCrystal();
            }

        }

    }

    private void RefillCrystal()
    {
        int amountToAdd = amountOfCrystal - crystalLeft.Count;

        for (int i = 0; i < amountToAdd; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private bool CanUseMultiCrystal()
    {
        if(canUseMultiCrystal)
        {
            if (crystalLeft.Count == amountOfCrystal)
                Invoke("ResetAbility", useTimeWindow);
            //Respawn crystals

            if (crystalLeft.Count > 0)
            {
                cooldown = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];

                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                CrystalController crystalController = newCrystal.GetComponent<CrystalController>();

                crystalController.SetupCrystal(crystalDuration, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform));

                if(crystalLeft.Count <= 0)
                {
                    cooldown = multiCrystalCooldown;
                    RefillCrystal();
                }
             
                return true;

            }

        }
        return false;
    }

    private void ResetAbility()
    {
        if (cooldownTimer > 0)
            return;
        
        cooldownTimer = multiCrystalCooldown;
        RefillCrystal();
    }
}
