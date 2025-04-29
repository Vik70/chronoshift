using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("Movement")]
    public Vector2 direction = Vector2.right;   // Direction to move (e.g., right or up)
    public float speed = 2f;                   // How fast it moves
    public float lifetime = 5f;

    private Vector3 startPos;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * speed;

        Destroy(gameObject, lifetime);
    }





    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Sign"))
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.onHit(999, Vector2.zero);
            }
        }
        else if (collision.CompareTag("PlayerClone"))
        {
            var clone = collision.GetComponent<CloneController>();
            if (clone != null && clone.handler != null)
            {
                clone.handler.KillClone();
            }
        }
        else if (collision.CompareTag("DestructibleDoor"))
        {
            var door = collision.GetComponent<DestructibleDoor>();
            if (door != null)
            {
                door.SendMessage("TakeDamage", 1);
            }
        }

        Destroy(gameObject);
    }
}

