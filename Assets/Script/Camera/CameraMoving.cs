using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public Camera Cam{ get; private set; }
    [SerializeField]
    private Transform target;
    private Vector3 workspace;
    private Vector2 currentPosition;
    private LevelInfor levelInfor;
    [SerializeField]
    private float maxSize = 10f;
    private float currentSize;
    void Start()
    {
        Cam = GetComponent<Camera>();
        levelInfor = GameObject.Find("LevelInfor").GetComponent<LevelInfor>();
        currentPosition = target.position;
        transform.position = target.position;
        currentSize = Mathf.Min((levelInfor.TileMapEdgeSign2.position.x - levelInfor.TileMapEdgeSign1.position.x) * 0.5f / Cam.aspect,
                                (levelInfor.TileMapEdgeSign2.position.y - levelInfor.TileMapEdgeSign1.position.y) * 0.5f,
                                maxSize);
        Debug.Log(currentSize);
    }
    void FixedUpdate()
    {
        Vector2 destination;
        destination.x = Mathf.Max(Mathf.Min(target.position.x, levelInfor.TileMapEdgeSign2.position.x - currentSize * Cam.aspect), levelInfor.TileMapEdgeSign1.position.x + currentSize * Cam.aspect);
        destination.y = Mathf.Max(Mathf.Min(target.position.y, levelInfor.TileMapEdgeSign2.position.y - currentSize), levelInfor.TileMapEdgeSign1.position.y + currentSize);

        workspace = currentPosition + ((Vector2)destination - currentPosition) * 0.1f;

        workspace.z = -10f;
        transform.position = workspace;
        currentPosition = workspace;
        Cam.orthographicSize = currentSize;
    }
}
