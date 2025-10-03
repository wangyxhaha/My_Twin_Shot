using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FlyEnemy : Enemy
{
    [SerializeField]
    private Transform wallDetector;
    [SerializeField]
    private FlyEnemyData flyEnemyData;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        animator.SetBool("move", true);

        FacingDirection = 1;

        arrowFilter.SetLayerMask(flyEnemyData.whatIsArrow);
        arrowFilter.useLayerMask=true;
    }

    void Update()
    {
        if (isDead) return;
        if (!CheckWall())
        {
            SetVelocityX(flyEnemyData.movementVelocity * FacingDirection);
        }
        else
        {
            Flip();
        }
        SetVelocityY(0f);
    }

    private bool CheckWall()
    {
        return Physics2D.OverlapCircle(wallDetector.position, flyEnemyData.WallDetectorRadius, flyEnemyData.whatIsGround | flyEnemyData.whatIsPlatform);
    }

    public override void OnDead(int _direction)
    {
        isDead = true;
        animator.SetBool("move", false);
        animator.SetBool("dead", true);
        this.gameObject.layer = LayerMask.NameToLayer("Body");
        workspace.Set(flyEnemyData.deadStartVelocityX * _direction * -1, flyEnemyData.deadStartVelocityY);
        rb2D.velocity = workspace;
        rb2D.gravityScale = 1f;
        Destroy(this.gameObject, 5f);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        workspace.Set(flyEnemyData.WallDetectorRadius, flyEnemyData.WallDetectorRadius);
        Gizmos.DrawWireCube(wallDetector.position, workspace);
    }
}
