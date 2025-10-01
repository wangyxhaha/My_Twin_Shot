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
    public PlayerMoveShootState playerMoveShootState { get; private set; }
    #endregion
    #region Components
    public Animator animator { get; private set; }
    public PlayerInputHandler playerInputHandler { get; private set; }
    public Rigidbody2D rb2D { get; private set; }
    #endregion
    #region Other Variables
    private int FacingDirection;

    private Vector2 velocityWorkspace;
    private Vector2 accelerationWorkspace;
    public Vector2 currentVelocity { get; private set; }
    public Vector2 currentAcceleration { get; private set; }

    [SerializeField]
    private PlayerData playerData;
    #endregion
    #region Check Transform
    [SerializeField]
    private Transform groundCheck;
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
        playerMoveShootState = new PlayerMoveShootState(this, playerStateMachine, playerData, "shoot");
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerInputHandler = GetComponent<PlayerInputHandler>();
        rb2D = GetComponent<Rigidbody2D>();
        playerStateMachine.Initialize(playerIdleState);
        FacingDirection = 1;
    }

    private void Update()
    {
        currentVelocity = rb2D.velocity;
        playerStateMachine.currentState.LogicalUpdate();
    }

    private void FixedUpdate()
    {
        currentVelocity = rb2D.velocity;
        playerStateMachine.currentState.PhysicsUpdate();
        UseAcceleration();
        // Debug.Log(currentAcceleration);
    }
    #endregion
    #region Set Functions
    public void SetVelocityX(float _velocity)
    {
        velocityWorkspace.Set(_velocity, currentVelocity.y);
        rb2D.velocity = velocityWorkspace;
        currentVelocity = velocityWorkspace;
    }
    public void SetVelocityY(float _velocity)
    {
        velocityWorkspace.Set(currentVelocity.x, _velocity);
        rb2D.velocity = velocityWorkspace;
        currentVelocity = velocityWorkspace;
    }
    public void SetAccelerationX(float _acceleration)
    {
        // Debug.Log("SetAccelerationX");
        // Debug.Log(_acceleration);
        accelerationWorkspace.Set(_acceleration, currentAcceleration.y);
        currentAcceleration = accelerationWorkspace;
    }
    public void SetAccelerationY(float _acceleration)
    {
        accelerationWorkspace.Set(currentAcceleration.x, _acceleration);
        currentAcceleration = accelerationWorkspace;
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
        return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
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
        velocityWorkspace = currentVelocity + Time.fixedDeltaTime * currentAcceleration;
        rb2D.velocity = velocityWorkspace;
        currentVelocity = velocityWorkspace;
        accelerationWorkspace.Set(0f, 0f);
        currentAcceleration = accelerationWorkspace;
    }
    public void AnimationFinishTrigger() => playerStateMachine.currentState.AnimationFinishTrigger();

    #endregion
}
