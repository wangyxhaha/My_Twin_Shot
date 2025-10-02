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
    public float groundDetectorHalfWidth = 0.9f;
    public float groundDetectorHalfHeight = 0.2f;
    public LayerMask whatIsGround = 1 << 7;
    public LayerMask whatIsPlatform = 1 << 9;
}
