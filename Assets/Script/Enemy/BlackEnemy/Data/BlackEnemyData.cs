using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newBlackEnemyData", menuName = "Data/Black Enemy Date/Base Data")]
public class BlackEnemyData : ScriptableObject
{
    [Header("Detector")]
    public float DestinationDetectorRadius = 0.2f;
    public int DestinationRadius = 7;
    public float CliffDetectorRadius = 0.5f;
    public float WallDetectorRadius = 0.3f;
    public float GroundDetectorHalfWidth = 0.5f;
    public float GroundDetectorHalfHeight = 0.2f;
    public LayerMask whatIsGround = 1 << 7;
    public LayerMask whatIsPlatform = 1 << 9;
    public LayerMask whatIsArrow = 1 << 8;
    public LayerMask whatIsBody = 1 << 11;

    [Header("Movement Setting")]
    public float movementVelocity = 4f;

    [Header("Dead Setting")]
    public float deadStartVelocityX = -4f;
    public float deadStartVelocityY = 8f;

    [Header("Jumping Setting")]
    public float gravity = -9.8f;
    public float jumpV0 = 10f;
}
