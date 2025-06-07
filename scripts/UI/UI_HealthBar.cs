using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Entity entity;

    private RectTransform myTransform;

    private Slider slider;

    private CharacterStats myStats;

    private void Start()
    {
        entity = GetComponentInParent<Entity>();
        myTransform = GetComponent<RectTransform>();
        slider = GetComponentInChildren<Slider>();
        myStats = GetComponentInParent<CharacterStats>();

        entity.OnFilpped += FlipUI;

        UpdateHealthUI();
        myStats.OnHealthChanged += UpdateHealthUI;
    }

    private void UpdateHealthUI()
    {
        slider.maxValue = myStats.GetMaxHealthValue();
        slider.value = myStats.currentHealth;
    }


    private void OnEnable()
    {
           
    }

    private void OnDisable()
    {
        entity.OnFilpped -= FlipUI; 
        myStats.OnHealthChanged -= UpdateHealthUI;
       
    }

    private void FlipUI() => myTransform.Rotate(0, 180, 0);
}
