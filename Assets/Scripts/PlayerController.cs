using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5.0f;
    public float jumpImpulse = 10.0f;

    public GameObject clonePrefab; // assign in Inspector
    public Transform cloneSpawnPoint; // maybe spawn slightly behind or below player
    public int maxHealth = 100;
    public int currentHealth = 100;

    private TemporalCloneHandler cloneHandler;
    private bool touchingFrozenClone = false;

    public bool isControlled = true; // Player starts controlled




    [Header("Audio")]
    public AudioClip jumpSound;
    private AudioSource sfxSource;

    Vector2 moveInput;
    TouchingDirections touchingDirections;

    [SerializeField]
    private bool _isMoving = false;
    public bool IsMoving
    {
        get { return _isMoving; }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    public bool _isFacingRight = true;
    public bool IsFacingRight
    {
        get { return _isFacingRight; }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove
    {
        get { return animator.GetBool(AnimationStrings.canMove); }
    }

    public bool IsAlive
    {
        get { return animator.GetBool(AnimationStrings.isAlive); }
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (IsMoving && !touchingDirections.IsOnWall)
            {
                return walkSpeed;
            }
            else
            {
                return 0;
            }
        }
    }

   


    public bool LockVelocity
    {
        get { return animator.GetBool(AnimationStrings.lockVelocity); }
    }

    Rigidbody2D rb;
    Animator animator;
    TimeReversal timeReversal;  // Reference to our TimeReversal component

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        timeReversal = GetComponent<TimeReversal>(); // Ensure TimeReversal is attached
        cloneHandler = GetComponent<TemporalCloneHandler>();

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;
        sfxSource.volume = 0.1f;    
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool(AnimationStrings.isAlive, false);

            FindObjectOfType<DeathManager>().HandleDeath();
        }

        if (!isControlled) return; // Prevent input if not controlled

    }

    private void FixedUpdate()
    {
        if (timeReversal != null && timeReversal.IsRewinding)
        {
            return;
        }

        if (!LockVelocity)
        {
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        }

        // New: Update grounded status properly
        bool reallyGrounded = touchingDirections.IsGrounded || touchingFrozenClone;
        animator.SetBool(AnimationStrings.isGrounded, reallyGrounded);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }


    public void OnMove(InputAction.CallbackContext context)
    {
        // Disable movement input if rewinding is active
        if (timeReversal != null && timeReversal.IsRewinding)
            return;

        



        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            IsMoving = false;
        }

        cloneHandler?.PassMovement(moveInput);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerClone"))
        {
            var clone = collision.gameObject.GetComponent<CloneController>();
            if (clone != null && clone.IsFrozen())
            {
                touchingFrozenClone = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerClone"))
        {
            touchingFrozenClone = false;
        }
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        // Prevent jumping while rewinding
        cloneHandler?.PassJump();

        if (context.started && (touchingDirections.IsGrounded || touchingFrozenClone) && (timeReversal == null || !timeReversal.IsRewinding))
        {
            animator.SetTrigger(AnimationStrings.jump);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);

            if (jumpSound != null && sfxSource != null)
            {
                sfxSource.PlayOneShot(jumpSound);
            }
        }
    }

    public void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }

    public void onHit(int damage, Vector2 knockback)
    {
        if (!IsAlive) return;

        currentHealth -= damage;

        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        Debug.Log($"Player hit! Current HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            animator.SetBool(AnimationStrings.isAlive, false);
            rb.velocity = Vector2.zero;
            // Optional: add death animation / restart logic
            FindObjectOfType<DeathManager>().HandleDeath();

            Debug.Log("Player died.");
            // Optionally call ResetCurrentLevel() here
        }
    }


    public void ResetCurrentLevel()
    {
        Scene currentScence = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScence.name);
    }
}
