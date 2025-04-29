using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var cp = collision.GetComponent<CheckpointManager>();
            if (cp != null)
            {
                // Mark this position as the new checkpoint
                cp.SetCheckpoint(transform.position);
                // Optional: visually disable or animate the checkpoint object
                 GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
    }
}
