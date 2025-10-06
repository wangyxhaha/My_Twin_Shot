using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField]
    public GameObject FakeFigure;
    private LevelInfor levelInfor;
    private float sceneWidth;
    private float sceneHeight;
    private GameObject MiddleLayer;
    private GameObject FakeFigureTop;
    private GameObject FakeFigureBottom;
    private GameObject FakeFigureLeft;
    private GameObject FakeFigureRight;
    private SpriteRenderer FakeFigureTopSpriteRenderer;
    private SpriteRenderer FakeFigureBottomSpriteRenderer;
    private SpriteRenderer FakeFigureLeftSpriteRenderer;
    private SpriteRenderer FakeFigureRightSpriteRenderer;
    private SpriteRenderer mainSpriteRenderer;
    private Color transparent;
    protected Vector2 workspace;

    private bool _enableInfinitySpace;
    public bool EnableInfinitySpace
    {
        get => _enableInfinitySpace;
        set
        {
            if (value)
            {
                FakeFigureTopSpriteRenderer.color = Color.white;
                FakeFigureBottomSpriteRenderer.color = Color.white;
                FakeFigureLeftSpriteRenderer.color = Color.white;
                FakeFigureRightSpriteRenderer.color = Color.white;
            }
            else
            {
                FakeFigureTopSpriteRenderer.color = transparent;
                FakeFigureBottomSpriteRenderer.color = transparent;
                FakeFigureLeftSpriteRenderer.color = transparent;
                FakeFigureRightSpriteRenderer.color = transparent;
            }
            _enableInfinitySpace = value;
        }
    }
    protected void InitializeFakeFigure()
    {
        levelInfor = GameObject.Find("LevelInfor").GetComponent<LevelInfor>();
        sceneWidth = levelInfor.TileMapEdgeSign2.position.x - levelInfor.TileMapEdgeSign1.position.x;
        sceneHeight = levelInfor.TileMapEdgeSign2.position.y - levelInfor.TileMapEdgeSign1.position.y;

        MiddleLayer = Instantiate(FakeFigure, transform, false);

        FakeFigureTop = Instantiate(FakeFigure, MiddleLayer.transform, false);
        FakeFigureBottom = Instantiate(FakeFigure, MiddleLayer.transform, false);
        FakeFigureLeft = Instantiate(FakeFigure, MiddleLayer.transform, false);
        FakeFigureRight = Instantiate(FakeFigure, MiddleLayer.transform, false);

        FakeFigureTop.transform.localPosition = new Vector2(0f, sceneHeight);
        FakeFigureBottom.transform.localPosition = new Vector2(0f, -sceneHeight);
        FakeFigureLeft.transform.localPosition = new Vector2(-sceneWidth, 0f);
        FakeFigureRight.transform.localPosition = new Vector2(sceneWidth, 0f);

        FakeFigureTopSpriteRenderer = FakeFigureTop.GetComponent<SpriteRenderer>();
        FakeFigureBottomSpriteRenderer = FakeFigureBottom.GetComponent<SpriteRenderer>();
        FakeFigureLeftSpriteRenderer = FakeFigureLeft.GetComponent<SpriteRenderer>();
        FakeFigureRightSpriteRenderer = FakeFigureRight.GetComponent<SpriteRenderer>();

        mainSpriteRenderer = GetComponent<SpriteRenderer>();

        FakeFigureTopSpriteRenderer.sortingLayerName = mainSpriteRenderer.sortingLayerName;
        FakeFigureBottomSpriteRenderer.sortingLayerName = mainSpriteRenderer.sortingLayerName;
        FakeFigureLeftSpriteRenderer.sortingLayerName = mainSpriteRenderer.sortingLayerName;
        FakeFigureRightSpriteRenderer.sortingLayerName = mainSpriteRenderer.sortingLayerName;

        // Debug.Log(mainSpriteRenderer);

        EnableInfinitySpace = true;

        transparent = Color.white;
        transparent.a = 0f;
    }

    public virtual void FixedUpdate()
    {
        if (!_enableInfinitySpace) return;

        MiddleLayer.transform.rotation = Quaternion.identity;

        FakeFigureTopSpriteRenderer.sprite = mainSpriteRenderer.sprite;
        FakeFigureBottomSpriteRenderer.sprite = mainSpriteRenderer.sprite;
        FakeFigureLeftSpriteRenderer.sprite = mainSpriteRenderer.sprite;
        FakeFigureRightSpriteRenderer.sprite = mainSpriteRenderer.sprite;

        FakeFigureTopSpriteRenderer.flipX = mainSpriteRenderer.flipX;
        FakeFigureBottomSpriteRenderer.flipX = mainSpriteRenderer.flipX;
        FakeFigureLeftSpriteRenderer.flipX = mainSpriteRenderer.flipX;
        FakeFigureRightSpriteRenderer.flipX = mainSpriteRenderer.flipX;

        FakeFigureTopSpriteRenderer.color = mainSpriteRenderer.color;
        FakeFigureBottomSpriteRenderer.color = mainSpriteRenderer.color;
        FakeFigureLeftSpriteRenderer.color = mainSpriteRenderer.color;
        FakeFigureRightSpriteRenderer.color = mainSpriteRenderer.color;

        FakeFigureTopSpriteRenderer.transform.rotation = mainSpriteRenderer.transform.rotation;
        FakeFigureBottomSpriteRenderer.transform.rotation = mainSpriteRenderer.transform.rotation;
        FakeFigureLeftSpriteRenderer.transform.rotation = mainSpriteRenderer.transform.rotation;
        FakeFigureRightSpriteRenderer.transform.rotation = mainSpriteRenderer.transform.rotation;

        workspace = transform.position;

        if (transform.position.x > levelInfor.TileMapEdgeSign2.position.x)
        {
            workspace.x = levelInfor.TileMapEdgeSign1.position.x + 0.01f;
        }
        else if (transform.position.x < levelInfor.TileMapEdgeSign1.position.x)
        {
            workspace.x = levelInfor.TileMapEdgeSign2.position.x - 0.01f;
        }
        if (transform.position.y > levelInfor.TileMapEdgeSign2.position.y)
        {
            workspace.y = levelInfor.TileMapEdgeSign1.position.y + 0.01f;
        }
        else if (transform.position.y < levelInfor.TileMapEdgeSign1.position.y)
        {
            workspace.y = levelInfor.TileMapEdgeSign2.position.y - 0.01f;
        }

        transform.position = workspace;
    }
}
