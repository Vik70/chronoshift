using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider healthSlider;

    public TMP_Text healthBarText;

    Damagable playerDamageable;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if(player != null )
        {
            Debug.Log("No player found in scene - ensure tag 'Player' ");
        }
        playerDamageable = player.GetComponent<Damagable>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        healthSlider.value = CalculateSliderPercentage(playerDamageable.Health, playerDamageable.MaxHealth);
        healthBarText.text = "HP:    " + playerDamageable.Health + " / " + playerDamageable.MaxHealth;
    }


    private void OnEnable()
    {
        playerDamageable.healthChanged.AddListener(OnPlayerHealthChanged);
    }

    private void OnDisable()
    {
        playerDamageable.healthChanged.RemoveListener(OnPlayerHealthChanged);
    }
    private float CalculateSliderPercentage(float currentHealth, float MaxHealth)
    {
        return currentHealth / MaxHealth;
    }

    private void OnPlayerHealthChanged(int newHealth, int maxHealth)
    {
        healthSlider.value = CalculateSliderPercentage(newHealth, maxHealth);
        healthBarText.text = "HP:    " + newHealth + " / " + maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
