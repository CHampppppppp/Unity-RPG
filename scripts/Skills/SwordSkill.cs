using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SwordSkill : Skills
{
    [Header("Skill info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 lauchForce;
    [SerializeField] public float swordGravity;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;

    protected override void Start()
    {
        base.Start();

        GenerateDots();
    }

    protected override void Update()
    {
        
        if(Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i *  spaceBetweenDots);
            }
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalDir = new Vector2(AimDirection().normalized.x * lauchForce.x, AimDirection().normalized.y * lauchForce.y);
        }
    }

    public void CreateSword()
    {
        if (swordPrefab == null)
        {
            Debug.LogError("swordPrefab is not assigned!");
            return;
        }

        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController swordController = newSword.GetComponent<SwordSkillController>();

        if (swordController == null)
        {
            Debug.LogError("SwordSkillController component is missing on swordPrefab!");
            return;
        }
        swordController.SetupSword(finalDir, swordGravity,player);

        player.AssignNewSword(newSword);

        DotsActive(false);

    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;

    }


    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {

            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent); ;
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimDirection().normalized.x * lauchForce.x, 
            AimDirection().normalized.y * lauchForce.y
            ) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
        Debug.Log("DotsActive " + _isActive);
    }
}
