using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine playerStateMachine;
    protected PlayerData playerData;

    protected float startTime;

    private string animeBoolName;

    public PlayerState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animeBoolName)
    {
        this.player = player;
        this.playerStateMachine = stateMachine;
        this.playerData = playerData;
        this.animeBoolName = animeBoolName;
    }

    public virtual void Enter()
    {
        DoCheck();
        player.animator.SetBool(animeBoolName, true);
        startTime = Time.time;
        // Debug.Log(animeBoolName);
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animeBoolName, false);
    }

    public virtual void LogicalUpdate()
    {
        ;
    }

    public virtual void PhysicsUpdate()
    {
        DoCheck();
    }

    public virtual void DoCheck()
    {
        ;
    }

    public virtual void AnimationFinishTrigger()
    {
        ;
    }
}
