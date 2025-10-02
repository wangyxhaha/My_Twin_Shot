using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName) : base(player, stateMachine, playerData, animeBoolName)
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
        // player.SetVelocityX(0f);
        // Physics.IgnoreLayerCollision(6, 8, false);
    }
    public override void Exit()
    {
        base.Exit();
        Debug.Log("idle exit");
        // Physics.IgnoreLayerCollision(6, 8, true);
    }
    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (inputX != 0f)
        {
            playerStateMachine.ChangeState(player.playerMoveState);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

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
