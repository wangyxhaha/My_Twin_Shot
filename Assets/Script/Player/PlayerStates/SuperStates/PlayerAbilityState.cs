using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityState : PlayerState
{
    protected bool isAbilityDone;
    protected bool isGrounded;
    public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName) : base(player, stateMachine, playerData, animeBoolName)
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

        isAbilityDone = false;
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicalUpdate()
    {
        base.LogicalUpdate();

        if (isAbilityDone)
        {
            if (isGrounded && player.currentVelocity.y < 0.01f)
            {
                playerStateMachine.ChangeState(player.playerIdleState);
            }
            else
            {
                playerStateMachine.ChangeState(player.playerInAirState);
            }
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
