using System;
using UnityEngine;

public class TemporalCloneHandler : MonoBehaviour
{
    public GameObject clonePrefab;
    public Transform spawnPoint;
    public AudioClip spawnSound;
    public AudioClip deathSound;
    public AudioClip errorSound;

    private GameObject activeClone;
    private AudioSource audioSource;
    private PlayerController playerController;

    private bool waitingToSpawn = true;
    private int currentHealth => playerController.currentHealth;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E was pressed");

            if (activeClone != null)
            {
                if (audioSource && deathSound) audioSource.PlayOneShot(deathSound);

                KillClone();
                waitingToSpawn = true;
            }
            else if (waitingToSpawn)
            {
                TrySpawnClone();
                Debug.Log("Trying bro");
            }
        }
    }



    void SpawnClone(Vector2 spawnPosition)
    {
        activeClone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);

        activeClone.GetComponent<CloneController>().handler = this;

        CloneController cloneCtrl = activeClone.GetComponent<CloneController>();
        if (cloneCtrl != null)
        {
            cloneCtrl.SetInput(Vector2.zero);
        }

        if (audioSource && spawnSound)
            audioSource.PlayOneShot(spawnSound);

        Debug.Log("Clone Spawned");
    }


    public void KillClone()
    {
        if(activeClone != null)
        {
            var inventory = activeClone.GetComponent<CloneTemporaryInventory>();
            if (inventory != null)
            {
                inventory.TransferToPlayer();

            }

            Destroy(activeClone);
            activeClone = null;
            waitingToSpawn = true;
            Debug.Log("Clone Killed");
        }
    }

    Vector2 CalculateSpawnPosition()
    {
        Vector2 direction = playerController.IsFacingRight ? Vector2.right : Vector2.left;

        Vector2 spawnPos = (Vector2)transform.position + direction;

        Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.2f, LayerMask.GetMask("Ground"));
        if (hit != null)
        {
            Debug.Log("Blocked! Can't spawn clone.");
            return transform.position; // fallback
        }

        return spawnPos;
    }


    void TrySpawnClone()
    {
        Vector2 spawnPos = CalculateSpawnPosition();

        Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.2f, LayerMask.GetMask("Ground"));
        if (hit != null)
        {
            Debug.Log("Spawn blocked!");
            if (audioSource && errorSound) audioSource.PlayOneShot(errorSound);
            return;
        }
        
        Damagable damagable = playerController.GetComponent<Damagable>();
        if (damagable != null)
        {
            if(damagable.Health <= 15)
            {
                Debug.Log("Not Enough Health to spawn clone");
                //Play sound
                return;
            }

            damagable.Hit(15, Vector2.zero);
        }
        else
        {
            Debug.LogWarning("No Damagable component on player");
            return;
        }


        //Sound effects
        SpawnClone(spawnPos);
        waitingToSpawn = false;
    }



    //void TrySpawnOrReplaceClone()
    //{
    //    if (activeClone != null )
    //    {
    //        KillClone();
    //    }

    //    Vector2 direction = playerController.IsFacingRight ? Vector2.right : Vector2.left;
    //    Vector2 spawnPos = (Vector2)transform.position + direction;

    //    // Check for wall at spawn location
    //    Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.2f, LayerMask.GetMask("Ground"));
    //    if (hit != null)
    //    {
    //        if (audioSource && errorSound) audioSource.PlayOneShot(errorSound);
    //        return;
    //    }

    //    // Split health
    //    playerController.currentHealth /= 2;

    //    GameObject clone = Instantiate(clonePrefab, spawnPos, Quaternion.identity);
    //    activeClone = clone;

    //    if (audioSource && spawnSound) audioSource.PlayOneShot(spawnSound);
    //}


    public void PassMovement(Vector2 input)
    {
        if (activeClone == null) return;

        var cloneController = activeClone.GetComponent<CloneController>();
        if (cloneController != null)
        {
            cloneController.SetInput(input);
        }
    }

    public void PassJump()
    {
        if (activeClone == null) return;

        var cloneController = activeClone.GetComponent<CloneController>();
        if (cloneController != null)
        {
            cloneController.RequestJump();
        }
    }
}
