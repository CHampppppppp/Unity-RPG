using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{

    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private float blackHoleTimer;

    private bool canGrow = true;
    private bool canShrink = false;
    public bool canCreateHotKey = true;
    private bool cloneAttackReleased = false;
    private bool playerCanDisapper = true;

    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;

    public List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotKey = new List<GameObject>();

    public bool playerCanExitState {  get; private set; }

    [SerializeField] private GameObject healthUI;


    public void SetupBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCooldown,float _blackHoleDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttack;
        cloneAttackCooldown = _cloneAttackCooldown;
        blackHoleTimer = _blackHoleDuration;
    }


    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackHoleTimer -= Time.deltaTime;

        if (blackHoleTimer < 0 && !cloneAttackReleased)//不按R攻击，时间到了黑洞消失 或者 攻击完成后延迟一秒消失
        {
            blackHoleTimer = Mathf.Infinity;

            Invoke("FinishBlackHoleAbility", 1f);
        }

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
        DestroyHotKeys();
        cloneAttackReleased = true;
        canCreateHotKey = false;

        if(playerCanDisapper)
        {
            playerCanDisapper = false;
            Player player = PlayerManager.instance.player;
            player.MakeTransparent(true);
            Canvas canvas = player.GetComponentInChildren<Canvas>();    
            canvas.enabled = false;
        }
    }

    private void FinishBlackHoleAbility()
    {
        DestroyHotKeys();
        playerCanExitState = true;
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
                xOffset = 1;
            else
                xOffset = -1;

            if(targets.Count > 0)
                SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xOffset, 0));
            amountOfAttacks--;

            if (amountOfAttacks <= 0)
            {
                cloneAttackReleased = false;

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
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.enabled = false;
            CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            enemy.enabled = true;
        }

    }

    public void AddEnemyToList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
