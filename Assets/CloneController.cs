using UnityEngine;

public class CloneController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 10f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private bool isFrozen = false;
    private bool jumpRequested = false;
    private bool isFacingRight = true;

    private TouchingDirections touchingDirections;

    public bool isControlled = false;


    public TemporalCloneHandler handler;

    Animator animator;


    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();


    }

    public void SetInput(Vector2 input)
    {
        if (isFrozen) return;
        moveInput = input;
    }

    public void RequestJump()
    {
        if (isFrozen) return;
        jumpRequested = true;
    }

    private void Flip(float moveX)
    {
        if (moveX > 0 && !isFacingRight)
        {
            isFacingRight = true;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (moveX < 0 && isFacingRight)
        {
            isFacingRight = false;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

   



    private void UpdateAnimator(float moveX, bool isGrounded, float yVelocity)
    {
        if (animator == null) return;

        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isMoving", Mathf.Abs(moveX) > 0.1f);
        animator.SetFloat("yVelocity", yVelocity);
        animator.SetBool("isAlive", true);
        animator.SetBool("canMove", true);
    }





    void FixedUpdate()
    {
        if (isFrozen)
        {
            rb.velocity = Vector2.zero;
            UpdateAnimator(0, true, 0);
            return;
        }

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        float moveSpeed = 0f;
        if (Mathf.Abs(moveInput.x) > 0.1f && (touchingDirections == null || !touchingDirections.IsOnWall))
        {
            moveSpeed = speed;
        }

        rb.velocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);

        Flip(moveInput.x);

        if (jumpRequested && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        UpdateAnimator(moveInput.x, isGrounded, rb.velocity.y);

        jumpRequested = false;
    }


    public bool IsFrozen()
    {
        return isFrozen;
    }





    public void Freeze()
    {
        isFrozen = true;
        rb.velocity = Vector2.zero;
    }

    public void UnfreezeAndDie()
    {
        isFrozen = false;
        // Or play death animation
    }
}
