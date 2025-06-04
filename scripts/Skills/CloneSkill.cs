using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skills
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;
    [SerializeField] private bool canAttack;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool canCreateCloneOnCounterAttack;

    public void CreateClone(Transform _cloneTransform,Vector3 _offset)//传入玩家所在位置
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneController>().SetupClone(_cloneTransform,cloneDuration,canAttack,_offset,FindClosestEnemy(newClone.transform));
    }

    public void CreateCloneOnDashStart()
    {
        if (createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if(createCloneOnDashOver)
            CreateClone(player.transform,Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform)
    {
        if (canCreateCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(_enemyTransform, new Vector3(1 * player.facingDir, 0)));
    }

    private IEnumerator CreateCloneWithDelay(Transform _enemyTransform,Vector3 _offset)
    {
        yield return new WaitForSeconds(.4f);
        CreateClone(_enemyTransform,_offset);
    }
}
