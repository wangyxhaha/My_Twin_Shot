using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newArrowData", menuName = "Data/Arrow Date/Base Data")]
public class ArrowData : ScriptableObject
{
    [Header("Velocity")]
    public float startVelocity = 15f;

    [Header("Detector")]
    public float detectorHalfWidth = 0.2f;
    public float detectorHalfHeight = 0.04f;
    public LayerMask whatIsGround = 1 << 7;

    [Header("Time Setting")]
    public float noGravityTime = 1.5f;
    public float shiningStartTime = 3f;
    public float shiningK = 70;
    public float lifeTime = 8f;
    public float maxExistTime = 30f;
}
