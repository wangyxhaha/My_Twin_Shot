using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newFlyEnemyData", menuName = "Data/Fly Enemy Date/Base Data")]
public class  FlyEnemyData : ScriptableObject
{
    [Header("Detector")]
    public float WallDetectorRadius = 0.2f;
    public LayerMask whatIsGround = 1 << 7;
    public LayerMask whatIsPlatform = 1 << 9;
    public LayerMask whatIsArrow = 1 << 8;
    public LayerMask whatIsBody = 1 << 11;

    [Header("Movement Setting")]
    public float movementVelocity = 4f;

    [Header("Dead Setting")]
    public float deadStartVelocityX = -4f;
    public float deadStartVelocityY = 8f;
}
