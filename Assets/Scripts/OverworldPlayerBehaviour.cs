using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OverworldPlayerBehaviour : MonoBehaviour
{
    private enum AxisLock { None, Horizontal, Vertical }

    public static OverworldPlayerBehaviour Instance { get; private set; }

    [Header("Movement Settings")]
    [SerializeField] private float normalSpeed = 3f;
    [SerializeField] private float runningSpeed = 6f;
    [SerializeField] private float sprintSpeed = 9f;

    [Header("Animation Settings")]
    [SerializeField] private AxisLock axisLock = AxisLock.None;
    [SerializeField] private Animator anim;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 lastMoveDir = Vector2.down;
    private bool isMoving;
    private float currentSpeed;
    private float runTime;
    private float runResetTimer;
    private const float runResetDelay = 0.1f;

    // Public Getters
    public bool IsMoving => isMoving;
    public Vector2 InputDirection => input;
    public float CurrentSpeed => currentSpeed;

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        if (anim == null)
            anim = GetComponent<Animator>();

        Time.fixedDeltaTime = 1f / 30f; // consistent Deltarune feel
    }

    private void Update()
    {
        ReadInput();
        HandleAxisLock();
        HandleAnimations();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void ReadInput()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = input.sqrMagnitude > 0f;

        if (isMoving)
            lastMoveDir = input;
    }

    private void HandleAxisLock()
    {
        bool hasX = Mathf.Abs(input.x) > 0f;
        bool hasY = Mathf.Abs(input.y) > 0f;

        if (!hasX && !hasY)
        {
            axisLock = AxisLock.None;
            return;
        }

        if (axisLock == AxisLock.None)
            axisLock = hasX ? AxisLock.Horizontal : AxisLock.Vertical;

        if (axisLock == AxisLock.Horizontal && !hasX)
            axisLock = AxisLock.None;

        if (axisLock == AxisLock.Vertical && !hasY)
            axisLock = AxisLock.None;
    }

    private void HandleMovement()
    {
        float speed = 0f;
        bool sprintHeld = Input.GetKey(KeyCode.LeftShift);

        if (isMoving)
        {
            runResetTimer = runResetDelay;

            if (sprintHeld)
            {
                runTime += Time.fixedDeltaTime * 55f;
                runTime = Mathf.Clamp(runTime, 0f, 30f);
                speed = runTime >= 30f ? sprintSpeed : runningSpeed;
            }
            else
            {
                speed = normalSpeed;
                runTime = 0f;
            }
        }
        else
        {
            if (runResetTimer > 0)
                runResetTimer -= Time.fixedDeltaTime;
            else
                runTime = 0f;

            speed = 0f;
        }

        currentSpeed = speed;

        Vector2 moveDelta = input * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveDelta);
    }

    private void HandleAnimations()
    {
        anim.SetBool("isMoving", isMoving);

        if (isMoving)
        {
            switch (axisLock)
            {
                case AxisLock.Horizontal:
                    anim.SetFloat("moveX", input.x);
                    anim.SetFloat("moveY", 0f);
                    break;

                case AxisLock.Vertical:
                    anim.SetFloat("moveX", 0f);
                    anim.SetFloat("moveY", input.y);
                    break;

                default:
                    anim.SetFloat("moveX", input.x);
                    anim.SetFloat("moveY", input.y);
                    break;
            }
        }
        else
        {
            anim.SetFloat("moveX", lastMoveDir.x);
            anim.SetFloat("moveY", lastMoveDir.y);
        }

        if (currentSpeed <= normalSpeed)
            anim.speed = 1f;
        else if (currentSpeed <= runningSpeed)
            anim.speed = 1.3f;
        else
            anim.speed = 1.7f;
    }

    // Save / Load
    public void Save(ref PlayerSaveData data) => data.Position = rb.position;
    public void Load(PlayerSaveData data)
    {
        rb.position = data.Position;
        transform.position = data.Position;
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    public Vector3 Position;
}
