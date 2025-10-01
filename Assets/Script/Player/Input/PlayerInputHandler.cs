using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 RawMovementInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    public bool JumpInput { get; private set; }
    public bool ShootInput { get; private set; }
    [SerializeField]
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float shootInputStartTime;

    public void Update()
    {
        CheckJumpHoldTime();
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
    }
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            jumpInputStartTime = Time.time;
        }
    }
    public void OnShootInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShootInput = true;
            shootInputStartTime = Time.time;
        }
    }
    public void UseJumpInput() => JumpInput = false;
    public void UseShootInput() => ShootInput = false;
    private void CheckJumpHoldTime()
    {
        if (Time.time - jumpInputStartTime > inputHoldTime)
        {
            JumpInput = false;
        }
    }
    private void CheckShootHoldTime()
    {
        if (Time.time - shootInputStartTime > inputHoldTime)
        {
            ShootInput = false;
        }
    }
}
