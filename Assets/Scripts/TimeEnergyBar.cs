using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeRewindBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider timeRewindSlider;      // The UI Slider that displays the rewind energy.
    public TMP_Text timeRewindText;      // Optional: Text to show current/maximum energy.

    [Header("Rewind Energy Settings")]
    public float maxRewindEnergy = 5f;   // Maximum amount of energy/time available for rewinding.
    public float drainRate = 2f;         // Energy drain rate (units per second) while rewinding.
    public float rechargeRate = 0.5f;      // Energy recharge rate (units per second) when not rewinding.

    private float currentRewindEnergy;
    private TimeReversal timeReversal;   // Reference to your TimeReversal component.

    private void Awake()
    {
        // Find the player by tag.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure your player GameObject is tagged 'Player'.");
        }
        else
        {
            timeReversal = player.GetComponent<TimeReversal>();
            if (timeReversal == null)
            {
                Debug.LogError("TimeReversal component not found on player.");
            }
        }
    }

    void Start()
    {
        // Initialize energy to maximum.
        currentRewindEnergy = maxRewindEnergy;
        if (timeRewindSlider != null)
        {
            timeRewindSlider.maxValue = maxRewindEnergy;
            timeRewindSlider.value = currentRewindEnergy;
        }
        if (timeRewindText != null)
        {
            timeRewindText.text = $"{currentRewindEnergy:0.0} / {maxRewindEnergy:0.0}";
        }
    }

    void Update()
    {
        if (timeReversal != null && timeReversal.IsRewinding)
        {
            currentRewindEnergy -= drainRate * Time.deltaTime;
            Debug.Log("Draining Energy: " + currentRewindEnergy);
            if (currentRewindEnergy <= 0f)
            {
                currentRewindEnergy = 0f;
                timeReversal.StopRewind();
            }
        }
        else
        {
            currentRewindEnergy += rechargeRate * Time.deltaTime;
            if (currentRewindEnergy > maxRewindEnergy)
            {
                currentRewindEnergy = maxRewindEnergy;
            }
        }

        if (timeRewindSlider != null)
        {
            timeRewindSlider.value = currentRewindEnergy;
        }

        if (timeRewindText != null)
        {
            timeRewindText.text = $"Time Energy:   {currentRewindEnergy:0.0} / {maxRewindEnergy:0.0}";
        }
    }

}
