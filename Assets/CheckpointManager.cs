using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CheckpointManager : MonoBehaviour
{
    private Vector3 checkpointPosition;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Initialize the first checkpoint as the player’s start position
        checkpointPosition = transform.position;
    }

    /// <summary>
    /// Call this to update the player’s checkpoint.
    /// </summary>
    public void SetCheckpoint(Vector3 pos)
    {
        checkpointPosition = pos;
        Debug.Log($"Checkpoint set to {pos}");
    }

    /// <summary>
    /// Move the player back to the last checkpoint.
    /// </summary>
    public void Respawn()
    {
        transform.position = checkpointPosition;
        rb.velocity = Vector2.zero;
        Debug.Log("Player respawned at last checkpoint");
    }
}
