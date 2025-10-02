using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Tilemaps;
using UnityEngine;

public class ArrowPlatform : MonoBehaviour
{
    private int FacingDirection = 1;
    public BoxCollider2D boxCollider2D{ get; private set; }
    public Rigidbody2D rb2D { get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }
    public PlatformEffector2D platformEffector2D{ get; private set; }

    [SerializeField]
    private ArrowData arrowData;

    private float onWallStartTime;
    private bool haveCollider;
    private Vector2 workspace;
    private Color colorWorkspace = Color.white;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        platformEffector2D = GetComponent<PlatformEffector2D>();

        workspace.Set(arrowData.startVelocity * FacingDirection, 0f);

        onWallStartTime = Time.time;

        haveCollider = false;
    }

    void Update()
    {
        if (!haveCollider) CheckNoColliderTime();
        CheckIfShouldShine();
        CheckIfShouldDestroy();
    }

    public void SetPosition(float _x, float _y)
    {
        workspace.Set(_x, _y);
        transform.position = workspace;
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

    private void CheckNoColliderTime()
    {
        if (Time.time - onWallStartTime > arrowData.noColliderTime)
        {
            platformEffector2D.colliderMask = arrowData.whatIsPlayer | arrowData.whatIsEnemy;
            // platformEffector2D.surfaceArc = 60f;
            haveCollider = true;
        }
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
