using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Date/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Idle State")]
    public float idleStoppingAcceleration = 30f;
    public float stoppingVelocityEpsilon = 0.5f;
    [Header("Move State")]
    public float maxMovementVelocity = 10f;
    public float movementAcceleration = 30f;

    [Header("In Air State")]
    public float maxInAirVelocity = 10f;
    public float inAirAcceleration = 30f;

    [Header("Jump State")]
    public float jumpVelocity = 7f;

    [Header("Check Variables")]
    public float groundDetectorWidth = 0.9f;
    public float groundDetectorHeight = 0.2f;
    public LayerMask whatIsGround = 1 << 7;
    public LayerMask whatIsPlatform = 1 << 9;
    public LayerMask whatIsEnemy = 1 << 10;
    public LayerMask whatIsBody = 1 << 11;
    public LayerMask whatIsItem = 1 << 12;
    public LayerMask whatIsTrap = 1 << 14;

    [Header("Dead State")]
    public float deadVelocityX = -4f;
    public float deadVelocityY = 8f;

    [Header("Other Variables")]
    public int maxLife = 3;
    public float enmeyDetectorWidth = 1f;
    public float enmeyDetectorHeight = 1.6f;
    public float itemDetectorWidth = 1f;
    public float itemDetectorHeight = 1.6f;
    public float buffDuration = 10f;
}
