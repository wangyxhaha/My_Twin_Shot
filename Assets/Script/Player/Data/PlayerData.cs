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
    public float movementVelocity = 5f;
    public float maxMovementVelocity = 10f;
    public float movementAcceleration = 30f;

    [Header("Jump State")]
    public float jumpVelocity = 15f;

    [Header("Check Variables")]
    public float groundCheckRadius = 0.3f;
    public LayerMask whatIsGround;
}
