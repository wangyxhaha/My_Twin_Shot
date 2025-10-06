using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Player : Entity
{
    #region State Variables
    public PlayerStateMachine playerStateMachine { get; private set; }
    public PlayerIdleState playerIdleState { get; private set; }
    public PlayerMoveState playerMoveState { get; private set; }
    public PlayerJumpState playerJumpState { get; private set; }
    public PlayerInAirState playerInAirState { get; private set; }
    public PlayerLandState playerLandState { get; private set; }
    #endregion
    #region Components
    public Animator animator { get; private set; }
    public PlayerInputHandler playerInputHandler { get; private set; }
    public Rigidbody2D rb2D { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public CapsuleCollider2D capsuleCollider2D { get; private set; }
    #endregion
    #region Other Variables
    private int FacingDirection;

    // private Vector2 workspace;
    public Vector2 currentVelocity { get; private set; }
    public Vector2 currentAcceleration { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    private int shootLayerIndex;
    private int deadLayerIndex;
    private bool shootInput;
    [SerializeField]
    private GameObject arrowPrefab;
    private int lifeCount;
    private bool isDead;
    private bool isReincarnating;
    private float? reincarnationTimer;
    private Color colorWorkspace;
    public int score;
    private bool _isFasterBuff;
    public bool isFasterBuff
    {
        get => _isFasterBuff;
        set
        {
            if (value)
            {
                maxInAirVelocity = playerData.maxInAirVelocity * fasterVelocityScale;
                maxMovementVelocity = playerData.maxMovementVelocity * fasterVelocityScale;
            }
            else
            {
                maxInAirVelocity = playerData.maxInAirVelocity;
                maxMovementVelocity = playerData.maxMovementVelocity;
            }
            _isFasterBuff = value;
        }
    }
    public bool isFlyBuff;
    public bool isGodBuff;
    private float? buffTimer;
    [SerializeField]
    private float fasterVelocityScale = 1.5f;
    public float maxMovementVelocity;
    public float maxInAirVelocity;
    #endregion
    #region Check Transform
    [SerializeField]
    private Transform groundDetector;
    #endregion
    #region Unity Callback Functions
    private void Awake()
    {
        playerStateMachine = new PlayerStateMachine();
        playerIdleState = new PlayerIdleState(this, playerStateMachine, playerData, "idle");
        playerMoveState = new PlayerMoveState(this, playerStateMachine, playerData, "move");
        playerJumpState = new PlayerJumpState(this, playerStateMachine, playerData, "inAir");
        playerInAirState = new PlayerInAirState(this, playerStateMachine, playerData, "inAir");
        playerLandState = new PlayerLandState(this, playerStateMachine, playerData, "land");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        shootLayerIndex = animator.GetLayerIndex("Shoot Layer");
        deadLayerIndex = animator.GetLayerIndex("Dead Layer");

        FacingDirection = 1;
        playerStateMachine.Initialize(playerIdleState);

        lifeCount = playerData.maxLife;
        isReincarnating = false;
        isDead = false;
        reincarnationTimer = null;

        colorWorkspace = Color.white;

        score = 0;

        ClearBuff();

        InitializeFakeFigure();
    }

    public void Update()
    {
        currentVelocity = rb2D.velocity;
        playerStateMachine.currentState.LogicalUpdate();

        CheckDead();

        if (isReincarnating)
        {
            Reincarnate();
        }

        shootInput = playerInputHandler.ShootInput;

        if (shootInput)
        {
            playerInputHandler.UseShootInput();
            StartShoot();
        }

        CheckItem();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        RefreshBuff();

        currentVelocity = rb2D.velocity;

        // CheckIfStandOnArrow();

        playerStateMachine.currentState.PhysicsUpdate();
        if (isFasterBuff) FasterBuff();
        UseAcceleration();
    }
    #endregion
    #region Set Functions
    public void SetVelocityX(float _velocity)
    {
        workspace.Set(_velocity, currentVelocity.y);
        rb2D.velocity = workspace;
        currentVelocity = workspace;
    }
    public void SetVelocityY(float _velocity)
    {
        workspace.Set(currentVelocity.x, _velocity);
        rb2D.velocity = workspace;
        currentVelocity = workspace;
    }
    public void SetAccelerationX(float _acceleration)
    {
        // Debug.Log("SetAccelerationX");
        // Debug.Log(_acceleration);
        workspace.Set(_acceleration, currentAcceleration.y);
        currentAcceleration = workspace;
    }
    public void SetAccelerationY(float _acceleration)
    {
        workspace.Set(currentAcceleration.x, _acceleration);
        currentAcceleration = workspace;
    }
    #endregion
    #region Check Functions
    public void CheckIfShouldFlip(int _inputX)
    {
        if (_inputX != 0 && _inputX != FacingDirection)
        {
            Flip();
        }
    }

    public bool CheckIfGrounded()
    {
        workspace.Set(playerData.groundDetectorWidth, playerData.groundDetectorHeight);
        // return Physics2D.OverlapBox(groundDetector.position, workspace, 0f, playerData.whatIsGround) || CheckIfStandOnArrow();
        return Physics2D.OverlapBox(groundDetector.position, workspace, 0f, playerData.whatIsGround | playerData.whatIsPlatform | playerData.whatIsTrap);
    }

    public void CheckDead()
    {
        if (isDead || isGodBuff) return;
        workspace.Set(playerData.enmeyDetectorWidth, playerData.enmeyDetectorHeight);
        if (Physics2D.OverlapBox(transform.position, workspace, 0f, playerData.whatIsEnemy) || CheckTrap())
        {
            isDead = true;
            StartDead();
        }
    }

    private void CheckItem()
    {
        if (isDead) return;
        workspace.Set(playerData.itemDetectorWidth, playerData.itemDetectorHeight);
        Collider2D item = Physics2D.OverlapBox(transform.position, workspace, 0f, playerData.whatIsItem);
        if (item)
        {
            item.gameObject.GetComponent<Item>().PickUp(this);
        }
    }

    private bool CheckTrap()
    {
        workspace.Set(playerData.groundDetectorWidth, playerData.groundDetectorHeight);
        // return Physics2D.OverlapBox(groundDetector.position, workspace, 0f, playerData.whatIsGround) || CheckIfStandOnArrow();
        return Physics2D.OverlapBox(groundDetector.position, workspace, 0f, playerData.whatIsTrap);
    }
    #endregion
    #region Shoot Functions
    public void StartShoot()
    {
        if (isDead) return;
        Debug.Log("shoot");
        animator.SetBool("shoot", true);
        animator.SetLayerWeight(shootLayerIndex, 1f);
    }
    public void EndShoot()
    {
        animator.SetBool("shoot", false);
        animator.SetLayerWeight(shootLayerIndex, 0f);
    }
    public void ShootArrow()
    {
        Debug.Log(rb2D.position);
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        if (FacingDirection == -1)
        {
            arrow.GetComponent<Transform>().Rotate(0f, 180f, 0f);
            arrow.GetComponent<Arrow>().SetFacingDirection(-1);
        }
        else
        {
            arrow.GetComponent<Arrow>().SetFacingDirection(1);
        }
        // arrowScript.SetPosition(transform.position.x, transform.position.y);
        Debug.Log("shoot arrow");
    }
    #endregion
    #region Dead Functions
    private void StartDead()
    {
        EndShoot();
        Debug.Log("dead");
        isDead = true;
        animator.SetBool("dead", true);
        animator.SetLayerWeight(deadLayerIndex, 2f);
        lifeCount--;
        TryToReincarnate();
        workspace.Set(playerData.deadVelocityX * FacingDirection, playerData.deadVelocityY);
        rb2D.velocity = workspace;
    }
    private void EndDead()
    {
        isDead = false;
        animator.SetBool("dead", false);
        animator.SetLayerWeight(deadLayerIndex, 0f);
    }
    private void TryToReincarnate()
    {
        if (lifeCount > 0)
        {
            isReincarnating = true;
        }
        else
        {
            RealDead();
        }
    }
    private void Reincarnate()
    {
        if (reincarnationTimer == null)
        {
            reincarnationTimer = Time.time;
        }
        colorWorkspace.a = (int)((Time.time - reincarnationTimer) / 0.15f) & 1;
        spriteRenderer.color = colorWorkspace;
        if (currentVelocity.y < 0 && CheckIfGrounded())
        {
            spriteRenderer.color = Color.white;
            isReincarnating = false;
            reincarnationTimer = null;
            EndDead();
        }
    }
    private void RealDead()
    {
        EnableInfinitySpace = false;
        this.gameObject.layer = LayerMask.NameToLayer("Body");
        playerInputHandler.SetUseInput(false);
    }
    #endregion
    #region Buff Functions

    public void ClearBuff()
    {
        isFasterBuff = false;
        isFlyBuff = false;
        isGodBuff = false;
        buffTimer = null;
    }
    private void FasterBuff()
    {
        currentAcceleration *= fasterVelocityScale;
    }
    public void LifePlusPlus() => lifeCount = Math.Min(3, lifeCount + 1);

    public void StartBuffTimer()
    {
        buffTimer = Time.time;
    }

    private void RefreshBuff()
    {
        if (buffTimer == null) return;
        if (Time.time - buffTimer > playerData.buffDuration)
        {
            ClearBuff();
            buffTimer = null;
        }
    }

    #endregion
    #region Gizmos Functions
    public void OnDrawGizmos()
    {
        Gizmos.color = CheckIfGrounded() ? Color.green : Color.red;
        workspace.Set(playerData.groundDetectorWidth, playerData.groundDetectorHeight);
        Gizmos.DrawWireCube(groundDetector.position, workspace);
        Gizmos.color = Color.red;
        workspace.Set(playerData.enmeyDetectorWidth, playerData.enmeyDetectorHeight);
        Gizmos.DrawWireCube(transform.position, workspace);
    }
    #endregion
    #region Other Functions
    private void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    private void UseAcceleration()
    {
        workspace = currentVelocity + Time.fixedDeltaTime * currentAcceleration;
        rb2D.velocity = workspace;
        currentVelocity = workspace;
        workspace.Set(0f, 0f);
        currentAcceleration = workspace;
    }
    public void AnimationFinishTrigger() => playerStateMachine.currentState.AnimationFinishTrigger();
    public int GetLife() => lifeCount;

    #endregion
}
