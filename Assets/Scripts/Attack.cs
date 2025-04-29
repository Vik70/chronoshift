using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{

    //Collider2D attackCollider;

    public int attackDamage = 10;
    public Vector2 knockback = Vector2.zero;
    public float knockbackX = 4f;
    public float knockbackY = 7f;
    public bool isFacingRight = true; // set this in code based on your attacker facing direction



    //private void Awake()
    //{
    //    attackCollider = GetComponent<Collider2D>();
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damagable damagable = collision.GetComponent<Damagable>();

        if (damagable != null)
        {
            Vector2 adjustedKnockback = knockback;

            // Use the transform scale to determine direction
            float direction = transform.lossyScale.x > 0 ? 1f : -1f;
            adjustedKnockback.x *= direction;

            bool gotHit = damagable.Hit(attackDamage, adjustedKnockback);
            if (gotHit)
            {
                PlayerController player = collision.GetComponent<PlayerController>();
                Vector2 knockbackDir = (player.transform.position.x < transform.position.x)
                    ? new Vector2(-knockbackX, knockbackY)
                    : new Vector2(knockbackX, knockbackY);

                Debug.Log(collision.name + " hit for " + attackDamage + " with knockback " + adjustedKnockback);
            }
        }
    }


}
