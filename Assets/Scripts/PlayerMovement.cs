using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // [SerializeField] KeyCode leftKey;
    // [SerializeField] KeyCode rightKey;
    //  [SerializeField] KeyCode jumpKey;


    BoxCollider2D mainCollider;
    Rigidbody2D rb;
    [HideInInspector]
    public Animator animator;

    bool isGrounded;
    public float horizontalDrag;
    public float jumpStrength = 8;
    public float moveSpeed;
    //private float jumpTimeCounter;
    private float startingSizeX;
    public float gravityScale = 5;
    public AudioClip jump;

    [SerializeField] float jumpCooldownTimeInSeconds = 0.1f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float ladderHorizontalSpeed = 1f;

    [Header("Movement assist settings")]
    [SerializeField] float CoyoteTimeInSeconds = 0.1f;
    [SerializeField] float JumpBufferInSeconds = 0.1f;

    [Header("Better jump")]
    [SerializeField] bool enableBetterJump = true;

    [SerializeField] float additionalLowJumpGravMultiplier = 0;
    [SerializeField] float additionalFallGravMultiplier = 0;
    [SerializeField] float maxVerticalSpeed;


    private float stunned = 0;
    
    [HideInInspector] public bool canMove = true;

    private float jumpBufferTime = 0;
    private float coyoteTime = 0;
    private float jumpCooldown = 0;

    private bool isJumping = false;

    [Header("Ice physics")]
    
    [SerializeField] float icePagreitis = 0.2f;
    [SerializeField] float iceDrag = 0.05f;

    public static GameObject playerObject;
    private void Awake()
    {
        playerObject = gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        startingSizeX = transform.localScale.x;
        rb.gravityScale = gravityScale;
    }


    bool touchingLadder = false;
    [HideInInspector]
    public bool onLadder = false;

    List<GameObject> touchedLadders = new List<GameObject>();
    float maxLadderHeight = 0;
    void RecalculateMaxLadderHeight()
    {
        maxLadderHeight = int.MinValue;
        foreach(GameObject obj in touchedLadders)
        {
            maxLadderHeight = Mathf.Max(maxLadderHeight, obj.transform.position.y+obj.transform.lossyScale.y/2);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            touchingLadder = true;
            touchedLadders.Add(collision.gameObject);
            RecalculateMaxLadderHeight();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ladder"))
        {
            touchedLadders.Remove(collision.gameObject);
            RecalculateMaxLadderHeight();

            if (touchedLadders.Count == 0)
            {
                touchingLadder = false;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        mainCollider = GetComponent<BoxCollider2D>();

        Bounds colliderBounds = mainCollider.bounds;
        Vector3 colliderSize = new Vector3(mainCollider.size.x * 0.9f * Mathf.Abs(transform.localScale.x), 0.1f, 1);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderSize.y * 0.45f, 0);

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos, colliderSize);

        Gizmos.color = Color.blue;
        float grav = Physics2D.gravity.y;

        float t = -jumpStrength / grav;
        float yPos = transform.position.y + jumpStrength * t + (grav * t * t) / 2;

        Gizmos.DrawRay(new Vector2(transform.position.x, yPos), Vector2.right);
    }

    [Header("Controls")]

    public bool flipped = false;
    public bool onIce = false;


    [HideInInspector]
    public bool grabbing = false;

    float lastVerticalVal = 0;
    // Update is called once per frame
    void Update()
    {
        
        // coyote time and jump buffer time update
        if (coyoteTime > 0) coyoteTime -= Time.deltaTime;
        if (jumpBufferTime > 0) jumpBufferTime -= Time.deltaTime;

        // jump cooldown time update 
        if (jumpCooldown > 0) jumpCooldown -= Time.deltaTime;

        //isGrounded patikrina
        Bounds colliderBounds = mainCollider.bounds;
        Vector3 colliderSize = new Vector3(mainCollider.size.x * 0.9f * Mathf.Abs(transform.localScale.x), 0.1f, 1);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderSize.y * 0.45f, 0);
        
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheckPos, colliderSize, 0, 64);
        
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0 && rb.velocity.y < 0.05f)
        {
            coyoteTime = CoyoteTimeInSeconds;
            isGrounded = true;


        }
        if (stunned > 0)
        {
            stunned = Mathf.Max(0, stunned - Time.deltaTime);
        }

        float verticalVal = Input.GetAxisRaw("Vertical");
        float horizontalVal = Input.GetAxisRaw("Horizontal");

        float jumpVal = Input.GetAxisRaw("Jump");

        if (onLadder && ((isGrounded && verticalVal < 0) || !touchingLadder) || playerObject.transform.position.y > maxLadderHeight + 0.2f)
        {
            onLadder = false;
        }
        if (verticalVal != 0 && touchingLadder && playerObject.transform.position.y <= maxLadderHeight)
        {
            onLadder = true;
            //Debug.Log("virs");
        }
        
        
        

        if (onLadder)
        {
            rb.gravityScale = 0;

            if (playerObject.transform.position.y >= maxLadderHeight && Input.GetAxis("Vertical") > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, Input.GetAxis("Vertical") * climbSpeed);
            }
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
        Collider2D[] interactColliders = Physics2D.OverlapBoxAll(gameObject.transform.position,GetComponent<BoxCollider2D>().size,0,128);
        
        if (interactColliders != null && interactColliders.Length > 0)
        {
            GameUI.Instance.ShowPlayerText("E to interact");
        }
        else
        {
            GameUI.Instance.HidePlayerText();
        }
        if (Input.GetKeyDown(KeyCode.E) && interactColliders != null && interactColliders.Length > 0 && !GameManager.Instance.playerDead && !GameUI.Instance.InDialog)
        {
            interactColliders[0].gameObject.GetComponent<Interactable>().Interact();
        }
        

        if (flipped)
        {
            horizontalVal *= -1;
        }

        //atsakingas uz vaiksciojima
        if (canMove && stunned == 0)
        {

            
            float playerVelocity = 0;

            if (onIce)
            {
                playerVelocity = rb.velocity.x;
            }

            if (horizontalVal < 0)
            {
                transform.localScale = new Vector3(-startingSizeX, transform.localScale.y, transform.localScale.z); // flip sprite to the left

                if (!onIce)
                {
                    playerVelocity -= moveSpeed;
                }
                else
                {
                    playerVelocity -= icePagreitis * Time.deltaTime;
                    playerVelocity = Mathf.Max(Mathf.Min(moveSpeed, playerVelocity), -moveSpeed);
                }

            }
            if (horizontalVal > 0)
            {
                transform.localScale = new Vector3(startingSizeX, transform.localScale.y, transform.localScale.z); // flip sprite to the right
                if (!onIce)
                {
                    playerVelocity += moveSpeed;
                }
                else
                {
                    playerVelocity += icePagreitis * Time.deltaTime;
                    playerVelocity = Mathf.Max(Mathf.Min(moveSpeed, playerVelocity), -moveSpeed);
                }
            }
            
            if (onIce)
            {
                playerVelocity -= iceDrag * Time.deltaTime * Mathf.Sign(playerVelocity);
            }

            if (onLadder)
            {
                playerVelocity = 0;
                if (horizontalVal < 0)
                {
                    playerVelocity -= ladderHorizontalSpeed;
                }
                else if (horizontalVal > 0)
                {
                    playerVelocity += ladderHorizontalSpeed;
                }
            }

            if (playerVelocity == 0)
            {
                if (rb.velocity.x > 0)
                {
                    rb.velocity = new Vector2(Mathf.Max(0, Mathf.Max(rb.velocity.x - horizontalDrag * Time.deltaTime,0)), rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(Mathf.Min(0, Mathf.Min(rb.velocity.x + horizontalDrag * Time.deltaTime, 0)), rb.velocity.y);
                }
            }
            else
            {
                rb.velocity = new Vector2(playerVelocity, rb.velocity.y);
            }

            if (jumpCooldown <= 0)
            {
                if (jumpVal > 0 && lastVerticalVal == 0)
                {
                    jumpBufferTime = JumpBufferInSeconds;
                }

                //atsakingas uz sokinejima
                if (((jumpVal > 0 && lastVerticalVal == 0) || jumpBufferTime > 0) && (isGrounded == true || coyoteTime > 0))
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                    //jumpTimeCounter = jumpTime;

                    //SoundManager.Instance.PlaySoundOneShot(jump);
                    isJumping = true;
                    SoundManager.Instance.Play("Jump");
                    jumpCooldown = jumpCooldownTimeInSeconds;
                    jumpBufferTime = 0;
                }
            }
            if (jumpVal <= 0)
            {
                //Debug.Log("pavyko");
                isJumping = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

        if (enableBetterJump && !onLadder)
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (additionalFallGravMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !isJumping)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (additionalLowJumpGravMultiplier - 1) * Time.deltaTime;
            }
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * Mathf.Min(Mathf.Abs(rb.velocity.y), maxVerticalSpeed));
        }
        lastVerticalVal = jumpVal;

        //atsakingas uz animacija
        if (animator != null)
        {
            animator.SetBool("Moving", Mathf.Abs(horizontalVal) > 0.1 || (Mathf.Abs(verticalVal) > 0.1 && onLadder));
            animator.SetBool("Grounded", isGrounded);
            animator.SetBool("On Ladders", onLadder);
        }

    }
}
