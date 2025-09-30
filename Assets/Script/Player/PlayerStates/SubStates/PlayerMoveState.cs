using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName) : base(player, stateMachine, playerData, animeBoolName)
    {
        ;
    }
    public override void DoCheck()
    {
        base.DoCheck();
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

        player.CheckIfShouldFlip(inputX);

        if (player.currentVelocity.x < playerData.maxMovementVelocity)
        {
            player.SetAccelerationX(playerData.movementAcceleration * inputX);
        }
        else
        {
            player.SetAccelerationX(0f);
        }

        if (inputX == 0f)
        {
            playerStateMachine.ChangeState(player.playerIdleState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
