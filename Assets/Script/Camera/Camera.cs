using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private Transform cameraTransform;
    private Vector3 workspace;
    void Start()
    {
        cameraTransform = GetComponent<Transform>();
    }
    void Update()
    {
        workspace.Set(target.position.x, target.position.y, -10);
        cameraTransform.position = workspace;
    }
}
