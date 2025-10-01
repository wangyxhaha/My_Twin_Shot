using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private bool isGrounded;
    private int inputX;
    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName) : base(player, stateMachine, playerData, animeBoolName)
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

        player.animator.SetFloat("velocityY",player.currentVelocity.y > 0 ? 1f : -1f);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        inputX = player.playerInputHandler.NormInputX;


        if (isGrounded && player.currentVelocity.y <= 0.01f)
        {
            playerStateMachine.ChangeState(player.playerIdleState);
        }
        else
        {
            player.CheckIfShouldFlip(inputX);

            player.animator.SetFloat("velocityY",player.currentVelocity.y > 0 ? 1f : -1f);

            if ((inputX > 0 && player.currentVelocity.x < playerData.maxInAirVelocity)
             || (inputX < 0 && player.currentVelocity.x > -playerData.maxInAirVelocity))
            {
                player.SetAccelerationX(playerData.inAirAcceleration * inputX);
            }
            else
            {
                player.SetAccelerationX(0f);
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        DoCheck();

        if (inputX == 0)
        {
            if (Mathf.Abs(player.currentVelocity.x) <= playerData.stoppingVelocityEpsilon)
            {
                player.SetAccelerationX(0f);
                player.SetVelocityX(0f);
            }
            else
            {
                player.SetAccelerationX(-(Vector2.right * player.currentVelocity.x).normalized.x * playerData.idleStoppingAcceleration);
            }
        }
    }
}
