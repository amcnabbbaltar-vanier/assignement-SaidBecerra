using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public static event System.Action OnDoubleJump;

    private float walkSpeed = 5f;
    private float runMultiplier = 2f;

    private float minJumpForce = 5f;
    private float maxJumpForce = 15f;
    private float maxChargeTime = 3f;
    private float doubleJumpForce = 12f;

    private bool hasDoubleJumpPowerUp = false;

    private float speedBoostTimer = 0f;
    private float jumpBoostTimer = 0f;
    private bool speedBoosted = false;

    public GameObject speedBoostPrefab;
    public GameObject jumpBoostPrefab;
    private float speedBoostRespawnTimer = 0f;
    private float jumpBoostRespawnTimer = 0f;
    private Vector3 speedBoostSpawnPos;
    private Vector3 jumpBoostSpawnPos;

    private Rigidbody rb;
    public float moveInput;
    public bool isGrounded;
    private bool isRunning;
    private bool isHoldingJump;
    private bool hasDoubleJumped;
    private float jumpHoldTime;
    private bool jumpQueued;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveInput = Input.GetAxis("Horizontal");
        HandleRunToggle();
        HandleJumpInput();
        HandleFacingDirection();
        HandlePowerUpTimers();
        HandlePickupRespawn();
    }

    void FixedUpdate()
    {
        float speed = isRunning ? walkSpeed * runMultiplier : walkSpeed;
        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, moveInput * speed);
    }

    void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        hasDoubleJumped = false;
        StopCoroutine("UngroundDelay");

        if (collision.gameObject.CompareTag("SpeedBoost"))
        {
            ActivateSpeedBoost();
            speedBoostSpawnPos = collision.gameObject.transform.position;
            Destroy(collision.gameObject);
            speedBoostRespawnTimer = 30f;
        }
        else if (collision.gameObject.CompareTag("JumpBoost"))
        {
            ActivateJumpBoost();
            jumpBoostSpawnPos = collision.gameObject.transform.position;
            Destroy(collision.gameObject);
            jumpBoostRespawnTimer = 30f;
        }
        else if (collision.gameObject.CompareTag("Score"))
        {
            Destroy(collision.gameObject);
            GameManager.instance.AddScore(50);
        }
        else if (collision.gameObject.CompareTag("DeathPlane"))
        {
            GameManager.instance.TakeDamage();
        }
        else if (collision.gameObject.CompareTag("Trap"))
        {
            GameManager.instance.TakeTrapDamage();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        StartCoroutine("UngroundDelay");
    }

    System.Collections.IEnumerator UngroundDelay()
    {
        yield return new WaitForSeconds(0.1f);
        isGrounded = false;
    }

    void HandlePickupRespawn()
    {
        if (speedBoostRespawnTimer > 0f)
        {
            speedBoostRespawnTimer -= Time.deltaTime;
            if (speedBoostRespawnTimer <= 0f)
                Instantiate(speedBoostPrefab, speedBoostSpawnPos, Quaternion.identity);
        }

        if (jumpBoostRespawnTimer > 0f)
        {
            jumpBoostRespawnTimer -= Time.deltaTime;
            if (jumpBoostRespawnTimer <= 0f)
                Instantiate(jumpBoostPrefab, jumpBoostSpawnPos, Quaternion.identity);
        }
    }

    void HandleRunToggle()
    {
        isRunning = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    void HandleFacingDirection()
    {
        if (moveInput > 0)
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        else if (moveInput < 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
    }

    void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                isHoldingJump = true;
                jumpHoldTime = 0f;
                jumpQueued = false;
            }
            else if (!hasDoubleJumped && hasDoubleJumpPowerUp)
            {
                PerformDoubleJump();
            }
        }

        if (isHoldingJump && isGrounded)
        {
            jumpHoldTime += Time.deltaTime;

            if (jumpHoldTime >= maxChargeTime)
            {
                jumpHoldTime = maxChargeTime;
                isHoldingJump = false;
                jumpQueued = true;
            }
        }

        if (Input.GetButtonUp("Jump") && isHoldingJump)
        {
            isHoldingJump = false;
            jumpQueued = true;
        }

        if (jumpQueued && isGrounded)
        {
            jumpQueued = false;
            float t = jumpHoldTime / maxChargeTime;
            float force = Mathf.Lerp(minJumpForce, maxJumpForce, t);
            PerformJump(force);
        }
    }

    void PerformJump(float force)
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
    }

    void PerformDoubleJump()
    {
        hasDoubleJumped = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);
        OnDoubleJump?.Invoke();
    }

    void HandlePowerUpTimers()
    {
        if (speedBoostTimer > 0f)
        {
            speedBoostTimer -= Time.deltaTime;
            if (speedBoostTimer <= 0f)
            {
                speedBoostTimer = 0f;
                walkSpeed /= 2f;
                speedBoosted = false;
            }
        }

        if (jumpBoostTimer > 0f)
        {
            jumpBoostTimer -= Time.deltaTime;
            if (jumpBoostTimer <= 0f)
            {
                jumpBoostTimer = 0f;
                hasDoubleJumpPowerUp = false;
            }
        }
    }

    public void ActivateSpeedBoost()
    {
        if (!speedBoosted)
        {
            walkSpeed *= 2f;
            speedBoosted = true;
        }
        speedBoostTimer = 5f;
    }

    public void ActivateJumpBoost()
    {
        hasDoubleJumpPowerUp = true;
        jumpBoostTimer = 30f;
    }
}