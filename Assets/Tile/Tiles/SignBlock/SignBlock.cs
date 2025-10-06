using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBlock : MonoBehaviour
{
    private Vector2 size = new Vector2(1f, 1f);
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position, size);
    }
}
