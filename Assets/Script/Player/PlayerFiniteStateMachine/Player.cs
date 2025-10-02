using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
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
    #endregion
    #region Other Variables
    private int FacingDirection;

    private Vector2 workspace;
    public Vector2 currentVelocity { get; private set; }
    public Vector2 currentAcceleration { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    private int shootLayerIndex;
    private bool shootInput;
    [SerializeField]
    private GameObject arrowPrefab;
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

        shootLayerIndex = animator.GetLayerIndex("Shoot Layer");

        FacingDirection = 1;
        playerStateMachine.Initialize(playerIdleState);
    }

    private void Update()
    {
        currentVelocity = rb2D.velocity;
        playerStateMachine.currentState.LogicalUpdate();

        shootInput = playerInputHandler.ShootInput;

        if (shootInput)
        {
            playerInputHandler.UseShootInput();
            StartShoot();
        }
    }

    private void FixedUpdate()
    {
        currentVelocity = rb2D.velocity;

        // CheckIfStandOnArrow();

        playerStateMachine.currentState.PhysicsUpdate();
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
        workspace.Set(playerData.groundDetectorHalfWidth, playerData.groundDetectorHalfHeight);
        // return Physics2D.OverlapBox(groundDetector.position, workspace, 0f, playerData.whatIsGround) || CheckIfStandOnArrow();
        return Physics2D.OverlapBox(groundDetector.position, workspace, 0f, playerData.whatIsGround | playerData.whatIsArrowPlatform);
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

    #endregion
    #region Shoot Functions
    public void StartShoot()
    {
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
    void OnDrawGizmos()
    {
        Gizmos.color = CheckIfGrounded() ? Color.green : Color.red;
        workspace.Set(playerData.groundDetectorHalfWidth, playerData.groundDetectorHalfHeight);
        Gizmos.DrawWireCube(groundDetector.position, workspace);
    }
    #endregion
}
