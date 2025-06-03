using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    public float maxSize;
    public float growSpeed;
    public float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink = false;
    public bool canCreateHotKey = true;
    private bool cloneAttackReleased;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    public List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();


    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCooldown)
    {
        Debug.Log("enter SetupBlackHole!");
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;

    }


    private void Update()
    {
        //Debug.Log("BlackHoleController updating!");
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            RealeaseCloneAttack();
        }



        CloneAttackLogic();


        if (canGrow && !canShrink)
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x < 0)
                Destroy(gameObject);
        }
    }
    private void RealeaseCloneAttack()
    {
        //Debug.Log("destroy hotkeys!");
        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKey = false;

        PlayerManager.instance.player.MakeTransparent(true);
    }

    private void FinishBlackHoleAbility()
    {
        PlayerManager.instance.player.ExitBlackHole();
        cloneAttackReleased = false;
        canShrink = true;
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && cloneAttackReleased)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float xOffset;

            if (Random.Range(0, 100) > 50)
                xOffset = 2;
            else
                xOffset = -2;

            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;
            //Debug.Log("amountOfAttack " + amountOfAttacks);

            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackHoleAbility",1.5f);

                //Debug.Log("canShrink " + canShrink);
            }
        }
    }

    private void DestroyHotKeys()
    {
        if (createdHotKey.Count <= 0)
            return;

        for (int i = 0; i < createdHotKey.Count; i++)
        {
            Destroy(createdHotKey[i]);
        }
    }

    private void CreateHotKey(Collider2D collision)
    {

        if (!canCreateHotKey)
            return;


        if (keyCodeList.Count <= 0)
        {
            Debug.LogWarning("not enough hot keys in the key code list");
            return;
        }

        if (cloneAttackReleased)
            return;

        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotKey.Add(newHotKey);


        KeyCode choosenKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenKey);

        BlackHoleHotKeyController blackHoleHotKeyController = newHotKey.GetComponent<BlackHoleHotKeyController>();

        blackHoleHotKeyController.SetupHotKey(choosenKey, collision.transform, this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().enabled = false;

            CreateHotKey(collision);


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Enemy>() != null)
            collision.GetComponent <Enemy>().enabled = true;
    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
