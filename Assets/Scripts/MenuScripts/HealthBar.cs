using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBar : MonoBehaviour
{
    Slider health_slider;
    [SerializeField]
    private TextMeshProUGUI health_text;

    private void Start() {
        health_slider = GetComponent<Slider>();
    }

    private void UpdateText(int healthValue){
        health_text.text = healthValue.ToString();
    }

    public void SetMaxHealth(int maxHealthValue){
        health_slider.maxValue = maxHealthValue;
        health_slider.value = maxHealthValue;
        UpdateText(maxHealthValue);
    }

    public void SetHealth(int healthValue){
        health_slider.value = healthValue;
        UpdateText(healthValue);
    }

}
