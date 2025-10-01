using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    protected int inputX;
    private bool JumpInput;
    private bool isGrounded;
    public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName) : base(player, stateMachine, playerData, animeBoolName)
    {
        ;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        isGrounded = player.CheckIfGrounded();
    }
    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        inputX = player.playerInputHandler.NormInputX;
        JumpInput = player.playerInputHandler.JumpInput;

        if (JumpInput)
        {
            player.playerInputHandler.UseJumpInput();
            playerStateMachine.ChangeState(player.playerJumpState);
        }
        else if (!isGrounded)
        {
            playerStateMachine.ChangeState(player.playerInAirState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        DoCheck();
    }
}
