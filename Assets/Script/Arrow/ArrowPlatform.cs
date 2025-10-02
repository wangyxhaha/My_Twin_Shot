using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ArrowPlatform : MonoBehaviour
{
    private Transform selfTransform;
    private int FacingDirection = 1;
    public BoxCollider2D boxCollider2D{ get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    [SerializeField]
    private ArrowData arrowData;

    private float onWallStartTime;

    private Vector2 workspace;
    private Color colorWorkspace = Color.white;

    void Start()
    {
        selfTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        workspace.Set(arrowData.startVelocity * FacingDirection, 0f);

        onWallStartTime = Time.time;
    }

    void Update()
    {
        CheckIfShouldShine();
        CheckIfShouldDestroy();
    }

    public void SetPosition(float _x, float _y)
    {
        workspace.Set(_x, _y);
        selfTransform.position = workspace;
    }

    public void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    public void SetFacingDirection(int _d)
    {
        FacingDirection = _d;
    }

    public void CheckIfShouldDestroy()
    {
        if (Time.time - onWallStartTime > arrowData.lifeTime)
        {
            Destroy(this.gameObject);
            Debug.Log("destroy arrow");
        }
    }

    public void CheckIfShouldShine()
    {
        if (Time.time - onWallStartTime > arrowData.shiningStartTime)
        {
            float x = (Time.time - onWallStartTime - arrowData.shiningStartTime) / arrowData.lifeTime;
            colorWorkspace.a = 1 - Mathf.Pow(Mathf.Sin(arrowData.shiningK * x * x), 2);
            spriteRenderer.color = colorWorkspace;
        }
    }
}
