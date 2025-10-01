using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveShootState : PlayerMoveState
{
    public PlayerMoveShootState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName) : base(player, stateMachine, playerData, animeBoolName)
    {
        ;
    }
    public override void AnimationFinishTrigger()
    {
        playerStateMachine.ChangeState(player.playerMoveState);
    }
}
