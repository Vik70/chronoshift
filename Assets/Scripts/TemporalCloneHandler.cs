using System.Collections;
using UnityEngine;
using Cinemachine;

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
    private CharacterSwitchManager switchManager;

    private bool waitingToSpawn = true;
    private int currentHealth => playerController.currentHealth;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<PlayerController>();
        switchManager = FindObjectOfType<CharacterSwitchManager>();
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
            }
        }
    }

    void SpawnClone(Vector2 spawnPosition)
    {
        activeClone = Instantiate(clonePrefab, spawnPosition, Quaternion.identity);

        CloneController cloneCtrl = activeClone.GetComponent<CloneController>();
        if (cloneCtrl != null)
        {
            cloneCtrl.handler = this;
            cloneCtrl.SetInput(Vector2.zero);
        }

        if (audioSource && spawnSound)
            audioSource.PlayOneShot(spawnSound);

        // Assign clone and its camera to the switch manager
        CinemachineVirtualCamera cloneCam = activeClone.GetComponentInChildren<CinemachineVirtualCamera>();
        StartCoroutine(AssignCloneToSwitch(cloneCam));

        Debug.Log("Clone Spawned");
    }

    private IEnumerator AssignCloneToSwitch(Cinemachine.CinemachineVirtualCamera cloneCam)
    {
        yield return null; // Wait one frame to ensure the clone is fully active

        if (activeClone != null && activeClone.activeInHierarchy)
        {
            if (switchManager != null && cloneCam != null)
            {
                switchManager.SetClone(activeClone, cloneCam);
                Debug.Log("Clone and camera assigned to switch manager.");
            }
            else
            {
                Debug.LogWarning("SwitchManager or clone camera is missing.");
            }
        }
        else
        {
            Debug.LogWarning("Clone is not active. Skipping camera assignment.");
        }
    }


    public void KillClone()
    {
        if (activeClone != null)
        {
            var inventory = activeClone.GetComponent<CloneTemporaryInventory>();
            if (inventory != null)
            {
                inventory.TransferToPlayer();
            }

            Destroy(activeClone);
            activeClone = null;
            waitingToSpawn = true;

            if (switchManager != null)
            {
                switchManager.ClearClone(); // Prevent switching to destroyed clone
            }

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
            return transform.position;
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
            if (damagable.Health <= 15)
            {
                Debug.Log("Not Enough Health to spawn clone");
                return;
            }

            damagable.Hit(15, Vector2.zero);
        }
        else
        {
            Debug.LogWarning("No Damagable component on player");
            return;
        }

        SpawnClone(spawnPos);
        waitingToSpawn = false;
    }

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
