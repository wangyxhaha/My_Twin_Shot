using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GreenSlime : Enemy
{

    [SerializeField]
    private Transform cliffDetector;
    [SerializeField]
    private Transform wallDetector;
    [SerializeField]
    private GreenSlimeData greenSlimeData;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        animator.SetBool("move", true);

        FacingDirection = 1;

        arrowFilter.SetLayerMask(greenSlimeData.whatIsArrow);
        arrowFilter.useLayerMask=true;
    }

    void Update()
    {
        if (isDead) return;
        if (CheckCliff() && !CheckWall())
        {
            SetVelocityX(greenSlimeData.movementVelocity * FacingDirection);
        }
        else
        {
            Flip();
        }
    }


    private bool CheckCliff()
    {
        return Physics2D.OverlapCircle(cliffDetector.position, greenSlimeData.CliffDetectorRadius, greenSlimeData.whatIsGround | greenSlimeData.whatIsPlatform);
    }
    private bool CheckWall()
    {
        return Physics2D.OverlapCircle(wallDetector.position, greenSlimeData.WallDetectorRadius, greenSlimeData.whatIsGround);
    }

    public override void OnDead(int _direction)
    {
        isDead = true;
        animator.SetBool("move", false);
        animator.SetBool("dead", true);
        this.gameObject.layer = LayerMask.NameToLayer("Body");
        workspace.Set(greenSlimeData.deadStartVelocityX * _direction * -1, greenSlimeData.deadStartVelocityY);
        rb2D.velocity = workspace;
        Destroy(this.gameObject, 5f);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = CheckCliff() ? Color.green : Color.red;
        workspace.Set(greenSlimeData.CliffDetectorRadius, greenSlimeData.CliffDetectorRadius);
        Gizmos.DrawWireCube(cliffDetector.position, workspace);
        Gizmos.color = CheckWall() ? Color.green : Color.red;
        workspace.Set(greenSlimeData.WallDetectorRadius, greenSlimeData.WallDetectorRadius);
        Gizmos.DrawWireCube(wallDetector.position, workspace);
    }
}
