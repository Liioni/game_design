using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CooldownScript : MonoBehaviour
{
    private Slider cooldown_slider;
    private ObjectLifetime dash_cooldown_timer;
    private bool cooldown_active = false;
    private int max_value = 1;

    private void Start() {
        cooldown_slider = GetComponent<Slider>();
    }

    public void SetCooldown(ObjectLifetime dash_timer){
        dash_cooldown_timer = dash_timer;
        cooldown_active = true;
    }

    private void Update() {
        if(dash_cooldown_timer != null && cooldown_active){
            float time_percent = (dash_cooldown_timer.timeLeft()/dash_cooldown_timer.getLifeSpan());
            //Debug.Log(time_percent);
            cooldown_slider.value = max_value - time_percent;
        } else if(cooldown_active){
            cooldown_slider.value = max_value;
            cooldown_active = false;
        }
    }

}
