using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FreezeChargeBar : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider freezeChargeSlider;      // The Slider representing freeze charges
    public TMP_Text freezeChargeText;      // Text to show current/maximum charges

    private TimeFreezeSpell2 freezeSpell;

    void Awake()
    {
        // Find the player by tag and get the TimeFreezeSpell2 component.
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure your player is tagged 'Player'.");
        }
        else
        {
            freezeSpell = player.GetComponent<TimeFreezeSpell2>();
            if (freezeSpell == null)
            {
                Debug.LogError("TimeFreezeSpell2 component not found on player.");
            }
        }
    }

    void Start()
    {
        if (freezeSpell != null && freezeChargeSlider != null)
        {
            freezeChargeSlider.maxValue = freezeSpell.MaxCharges;
            freezeChargeSlider.value = freezeSpell.CurrentCharges;
        }

        if (freezeSpell != null && freezeChargeText != null)
        {
            freezeChargeText.text = $"Freeze Charges: {freezeSpell.CurrentCharges}/{freezeSpell.MaxCharges}";
        }
    }

    void Update()
    {
        if (freezeSpell != null)
        {
            if (freezeChargeSlider != null)
            {
                freezeChargeSlider.value = freezeSpell.CurrentCharges;
            }

            if (freezeChargeText != null)
            {
                freezeChargeText.text = $"Freeze Charges: {freezeSpell.CurrentCharges}/{freezeSpell.MaxCharges}";
            }
        }
    }
}
