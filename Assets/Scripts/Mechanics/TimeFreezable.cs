using UnityEngine;

public class TimeFreezable : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isFrozen = false;
    private Vector2 storedVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // This method provides visual feedback that the object is marked for freezing.
    public void CastFreezeSpell()
    {
        // Change the object's color to cyan as feedback
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.color = Color.cyan;
        }
    }

    public void Freeze()
    {
        if (rb != null && !isFrozen)
        {
            storedVelocity = rb.velocity;
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            isFrozen = true;

            if (TryGetComponent<CloneController>(out var clone))
            {
                clone.Freeze(); // stop movement input
            }

            LaserTurret turret = GetComponent<LaserTurret>();
            if (turret != null)
            {
                turret.SetFrozen(true);
            }

            // Attempt to disable or flag the wizard.
            Wizard wizard = GetComponentInChildren<Wizard>(); // or GetComponent<Wizard>() if on same object
            if (wizard != null)
            {
                wizard.isFrozen = true;
                wizard.enabled = false;  // Or you might simply rely on isFrozen
            }

            // Disable the MovingPlatform component if present.
            MovingPlatform platform = GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platform.enabled = false;
            }

            VerticalMovingPlatform vPlatform = GetComponent<VerticalMovingPlatform>();
            if (vPlatform != null)
            {
                vPlatform.enabled = false;
            }

            Debug.Log($"{gameObject.name} is now frozen.");
        }
    }

    public void Unfreeze()
    {
        if (rb != null && isFrozen)
        {
            rb.isKinematic = false;
            rb.velocity = storedVelocity;
            isFrozen = false;

            if (TryGetComponent<CloneController>(out var clone))
            {
                clone.UnfreezeAndDie(); // Die if unfreezing a clone
            }

            LaserTurret turret = GetComponent<LaserTurret>();
            if (turret != null)
            {
                turret.SetFrozen(false);
            }

            // Re-enable the wizard if present.
            Wizard wizard = GetComponentInChildren<Wizard>(); // Use GetComponentInChildren if needed
            if (wizard != null)
            {
                wizard.isFrozen = false;
                wizard.enabled = true;
            }

            // Re-enable the MovingPlatform component if present.
            MovingPlatform platform = GetComponent<MovingPlatform>();
            if (platform != null)
            {
                platform.enabled = true;
            }

            VerticalMovingPlatform vPlatform = GetComponent<VerticalMovingPlatform>();
            if (vPlatform != null)
            {
                vPlatform.enabled = true;
            }

            // Optionally, restore original color.
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = Color.white;
            }

            Debug.Log($"{gameObject.name} is now unfrozen.");
        }
    }


}
