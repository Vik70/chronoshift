using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEyeDamage : MonoBehaviour
{
    public int damageAmount = 50;
    public Vector2 knockbackForce = new Vector2(5, 3);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerController player = GetComponent<PlayerController>();

            if (player != null)
            {
                player.onHit(damageAmount, knockbackForce);
            }
        }
    }
}
