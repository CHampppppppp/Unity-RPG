using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skills
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _cloneTransform)//传入玩家所在位置
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetupClone(_cloneTransform,cloneDuration,canAttack);
    }


}
