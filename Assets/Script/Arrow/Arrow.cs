using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Arrow : Entity
{
    [SerializeField]
    private Transform detector;
    [SerializeField]
    private GameObject arrowPlatformPrefab;
    private int FacingDirection = 1;
    public Rigidbody2D rb2D { get; private set; }
    public BoxCollider2D boxCollider2D{ get; private set; }
    public SpriteRenderer spriteRenderer { get; private set; }

    [SerializeField]
    private ArrowData arrowData;
    private float startTime;

    void Awake()
    {
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        workspace.Set(arrowData.startVelocity * FacingDirection, 0f);
        rb2D.velocity = workspace;

        startTime = Time.time;

        InitializeFakeFigure();
    }

    public void Update()
    {
        // base.Update();
        Rotate();
        CheckExistTime();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (DetectorCheck())
        {
            GameObject newArrowPlatform = Instantiate(arrowPlatformPrefab, detector.position, Quaternion.identity);
            if (rb2D.velocity.x < 0)
            {
                newArrowPlatform.GetComponent<ArrowPlatform>().Flip();
            }
            Destroy(this.gameObject);
        }
        CheckIfShouldFlip();
        CheckShotEnemy();
    }

    private void Rotate()
    {
        transform.rotation = Quaternion.Euler(0f, FacingDirection == 1 ? 0f : 180f,rb2D.velocity.y);
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
    public void CheckIfShouldFlip()
    {
        if (rb2D.velocity.x * FacingDirection < 0)
        {
            Flip();
            Debug.Log("arrow flip");
        }
    }

    public void CheckExistTime()
    {
        if (Time.time - startTime > arrowData.maxExistTime)
        {
            Destroy(this.gameObject);
            Debug.Log("destroy arrow");
        }
    }

    private void CheckShotEnemy()
    {
        workspace.Set(arrowData.detectorHalfWidth, arrowData.detectorHalfHeight);
        Collider2D result = Physics2D.OverlapBox(detector.position, workspace, 0f, arrowData.whatIsEnemy);
        // Debug.Log(result);
        if (result != null)
        {
            result.gameObject.GetComponent<Enemy>().OnDead(FacingDirection);
            Destroy(this.gameObject);
        }
    }

    public bool DetectorCheck()
    {
        workspace.Set(arrowData.detectorHalfWidth, arrowData.detectorHalfHeight);
        return Physics2D.OverlapBox(detector.position, workspace, 0f, arrowData.whatIsGround);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        workspace.Set(arrowData.detectorHalfWidth, arrowData.detectorHalfHeight);
        Gizmos.DrawWireCube(detector.position, workspace);
    }
}
