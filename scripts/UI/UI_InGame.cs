using System;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image dashImage;
    [SerializeField] private Image parryImage;
    [SerializeField] private Image crystalImage;
    [SerializeField] private Image blackHoleImage;
    [SerializeField] private Image flaskImage;
    [SerializeField] private float dashCooldown; 
    [SerializeField] private float Cooldown; 
    [SerializeField] private float parryCooldown;
    [SerializeField] private float crystalCooldown; 
    [SerializeField] private float blackHoleCooldown; 
    [SerializeField] private float flaskCooldown;

    private void Awake()
    {
        
    }

    void Start()
    {
        if (playerStats != null)
        {
            playerStats.OnHealthChanged += UpdateHealthUI;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(dashCooldown);

        if (Input.GetKeyDown(KeyCode.LeftShift))
            SetCooldownOf(dashImage);

        if(Input.GetKeyDown(KeyCode.Q))
            SetCooldownOf(parryImage);

        if(Input.GetKeyDown(KeyCode.F))
            SetCooldownOf(crystalImage);

        if (Input.GetKeyDown(KeyCode.R))
            SetCooldownOf(blackHoleImage);

        if(Input.GetKeyDown(KeyCode.Alpha1))
            SetCooldownOf(flaskImage);

        CheckCooldownOf(dashImage, dashCooldown);
        CheckCooldownOf(parryImage, parryCooldown);
        CheckCooldownOf(crystalImage, crystalCooldown);
        CheckCooldownOf(blackHoleImage, blackHoleCooldown);
        Inventory inventory = Inventory.instance;
        if (inventory == null)
        {
            Debug.Log("inventory is null");
            return;
        }
        CheckCooldownOf(flaskImage, inventory.flaskCooldown);
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = playerStats.GetMaxHealthValue();
        slider.value = playerStats.currentHealth;
    }

    private void SetCooldownOf(Image _image)
    {
        if (_image.fillAmount <= 0)
            _image.fillAmount = 1;
    }

    private void CheckCooldownOf(Image _image, float _cooldown)
    {
        if(_image.fillAmount > 0)
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
    }

}
