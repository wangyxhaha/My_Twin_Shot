using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 单例模式
    public static CameraController Instance { get; private set; }
    public Camera Cam { get; private set; }
    // [SerializeField]
    private Transform playerTarget;
    private Vector3 workspace;
    private Vector2 currentPosition;
    private LevelInfor levelInfor;
    [SerializeField]
    private float maxSize = 10f;
    private float currentSize;
    private bool _followPlayerMode;
    private bool _fixedMode;
    public bool followPlayerMode
    {
        get => _followPlayerMode;
        set
        {
            if (value)
            {
                _fixedMode = false;
                Initialize();
                playerTarget = GameObject.Find("Player").transform;
            }
            if (playerTarget != null)
            {
                _followPlayerMode = value;
            }
        }
    }
    public bool fixedMode
    {
        get => _fixedMode;
        set
        {
            if (value)
            {
                _followPlayerMode = false;
                playerTarget = null;
            }
            _fixedMode = value;
        }
    }

    public void Initialize()
    {
        levelInfor = GameObject.Find("LevelInfor").GetComponent<LevelInfor>();

        currentPosition = Vector2.zero;
        transform.position = Vector2.zero;

        fixedMode = true;

        currentSize = Mathf.Min((levelInfor.TileMapEdgeSign2.position.x - levelInfor.TileMapEdgeSign1.position.x) * 0.5f / Cam.aspect,
                                (levelInfor.TileMapEdgeSign2.position.y - levelInfor.TileMapEdgeSign1.position.y) * 0.5f,
                                maxSize);
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        Cam = GetComponent<Camera>();

        fixedMode = true;

        SubscribeEvent();

        // FollowModeInitialize();
        // Debug.Log(currentSize);

        //test
        // followPlayerMode = true;
    }
    void FixedUpdate()
    {
        // Debug.Log($"_followPlayerMode:{_followPlayerMode}");
        if (_followPlayerMode)
        {
            FollowPlayer();
        }
    }

    private void FollowPlayer()
    {
        Vector2 destination;
        destination.x = Mathf.Max(Mathf.Min(playerTarget.position.x, levelInfor.TileMapEdgeSign2.position.x - currentSize * Cam.aspect), levelInfor.TileMapEdgeSign1.position.x + currentSize * Cam.aspect);
        destination.y = Mathf.Max(Mathf.Min(playerTarget.position.y, levelInfor.TileMapEdgeSign2.position.y - currentSize), levelInfor.TileMapEdgeSign1.position.y + currentSize);

        workspace = currentPosition + ((Vector2)destination - currentPosition) * 0.1f;

        workspace.z = -10f;

        transform.position = workspace;
        currentPosition = workspace;
        Cam.orthographicSize = currentSize;
    }

    public void SetPosition(Vector2 _pos)
    {
        transform.position = _pos;
    }

    public void SetSize(float _size)
    {
        currentSize = _size;
    }

    public void SubscribeEvent()
    {
        GameEventManager.Instance.CallLevelOnLoad += () =>
        {
            followPlayerMode = true;
            Debug.Log("Level on load");
        };
        GameEventManager.Instance.CallLevelOnDestroy += () =>
        {
            fixedMode = true;
            Debug.Log("Level destroy");
        };
    }
}
