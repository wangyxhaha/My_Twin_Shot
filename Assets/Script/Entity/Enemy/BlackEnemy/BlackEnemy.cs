using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.U2D.IK;

public class BlackEnemySlime : Enemy
{

    [SerializeField]
    private Transform cliffDetector;
    [SerializeField]
    private Transform wallDetector;
    [SerializeField]
    private Transform groundDetector;
    [SerializeField]
    private Transform destinationDetector;
    [SerializeField]
    private BlackEnemyData blackEnemyData;

    private enum STATE
    {
        move,
        jump
    }
    private STATE state;
    private STATE RandomDecision()
    {
        return UnityEngine.Random.Range(0.0f, 1.0f) > 0.01f ? STATE.move : STATE.jump;
    }
    private bool isJumping;
    private int jumpingState;
    private float jumpStartTime;
    private Vector2Int testDestination;
    private Vector2 testPosition;
    private Vector2 testV0;

    public void Start()
    {
        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();

        animator.SetBool("move", true);

        FacingDirection = 1;

        arrowFilter.SetLayerMask(blackEnemyData.whatIsArrow);
        arrowFilter.useLayerMask = true;

        state = STATE.move;

        isJumping = false;

        InitializeFakeFigure();
    }

    public void FixedUpdate()
    {
        if (!isDead)
        {
            if (isJumping)
            {
                switch (jumpingState)
                {
                    case 1:
                        if (!CheckGround() || Time.time - jumpStartTime > 0.3f) jumpingState = 2;
                        break;
                    case 2:
                        if (CheckGround())
                        {
                            jumpingState = 0;
                            isJumping = false;
                            state = STATE.move;
                        }
                        break;
                }
            }
            else if (state == STATE.move)
            {
                if (CheckGround())
                {
                    if (CheckCliff() && !CheckWall())
                    {
                        SetVelocityX(blackEnemyData.movementVelocity * FacingDirection);

                        state = RandomDecision();
                    }
                    else
                    {
                        if (CheckWall() || UnityEngine.Random.Range(0, 2) == 0)
                        {
                            Flip();

                            state = RandomDecision();
                        }
                        else
                        {
                            state = STATE.move;
                        }
                    }
                }
            }
            else if (state == STATE.jump)
            {
                Vector2? destination = FindJumpDestination();
                if (destination == null)
                {
                    if (!CheckCliff()) Flip();
                    Debug.Log("no destination");
                    state = STATE.move;
                }
                else if (!JumpTo((Vector2)destination))
                {
                    Debug.Log("can't jump to destination");
                    state = STATE.move;
                }
                else jumpStartTime = Time.time;
            }
        }
        FakeFigureUpdate();
    }

    private bool JumpTo(Vector2 _destination)
    {
        float deltaX = _destination.x - transform.position.x;
        float deltaXsquare = deltaX * deltaX;
        float deltaY = _destination.y - transform.position.y;
        float deltaYsquare = deltaY * deltaY;
        float gravity = -Physics2D.gravity.y * rb2D.gravityScale;
        // 固定初速度大小求角度
        // float V0square = blackEnemyData.jumpV0 * blackEnemyData.jumpV0;
        // float A = blackEnemyData.gravity * deltaXsquare / (2f * V0square);
        // float B = deltaX;
        // float C = blackEnemyData.gravity * deltaXsquare / (2f * V0square) + deltaY;
        // float D = B * B - 4f * A * C;
        // if (D < 0) return false;
        // isJumping = true;
        // jumpingState = 1;
        // float sqrtD = Mathf.Sqrt(D);
        // float[] tanTheta = new float[2] { (-B + sqrtD) / (2f * A), (-B - sqrtD) / (2f * A) };
        // // int choice = Mathf.Abs(D) < 0.01f ? 0 : Random.Range(0, 2);
        // int choice = 0;
        // float theta = Mathf.Atan(tanTheta[choice]);
        // Debug.Log(theta / Mathf.PI);
        // workspace.Set(blackEnemyData.jumpV0 * Mathf.Cos(theta) * FacingDirection, blackEnemyData.jumpV0 * Mathf.Sin(theta) * FacingDirection);
        // rb2D.velocity = workspace;
        // testV0 = workspace;
        // return true;

        // 角度只有一个解，即使用正好到达目标位置的最小速度
        if (deltaX == 0f) return false;
        float sqrtDeltaXSquareAddDeltaYSquare = Mathf.Sqrt(deltaXsquare + deltaYsquare);
        float V0 = Mathf.Sqrt(gravity * (sqrtDeltaXSquareAddDeltaYSquare + deltaY));
        float theta = Mathf.Atan((deltaY + sqrtDeltaXSquareAddDeltaYSquare) / deltaX);
        workspace.Set(V0 * Mathf.Cos(theta) * FacingDirection, V0 * Mathf.Sin(theta) * FacingDirection);
        rb2D.velocity = workspace;
        
        isJumping = true;
        jumpingState = 1;
        return true;
    }
    private bool CheckCliff()
    {
        return Physics2D.OverlapCircle(cliffDetector.position, blackEnemyData.CliffDetectorRadius, blackEnemyData.whatIsGround | blackEnemyData.whatIsPlatform);
    }
    private bool CheckWall()
    {
        return Physics2D.OverlapCircle(wallDetector.position, blackEnemyData.WallDetectorRadius, blackEnemyData.whatIsGround);
    }

    private bool CheckGround()
    {
        workspace.Set(blackEnemyData.GroundDetectorHalfWidth, blackEnemyData.GroundDetectorHalfHeight);
        return Physics2D.OverlapBox(groundDetector.position, workspace, 0f, blackEnemyData.whatIsGround | blackEnemyData.whatIsPlatform);
    }

    private bool CheckBlock(Vector2 _position)
    {
        return Physics2D.OverlapCircle(_position, blackEnemyData.DestinationDetectorRadius, blackEnemyData.whatIsGround);
    }
    
    private static int[] dx = new int[4] { -1, 0, 1, 0 };
    private static int[] dy = new int[4] { 0, 1, 0, -1 };
    private Vector2? FindJumpDestination()
    {
        int d = blackEnemyData.DestinationRadius * 2 - 1;
        int r = blackEnemyData.DestinationRadius;
        bool[,] map = new bool[d, d];
        bool[,] flag = new bool[d, d];
        // 遍历获取周围tile的情况
        for (int i = 0; i < d; i++)
        {
            for (int j = 0; j < d; j++)
            {
                workspace.Set(transform.position.x - r + i, transform.position.y - r + j);
                map[i, j] = CheckBlock(workspace);
            }
        }

        // 使用bfs获取有可能到达的位置
        Queue<(int, int)> q = new Queue<(int, int)>();
        q.Enqueue((r, r));
        flag[r, r] = true;
        while (q.Count() > 0)
        {
            (int, int) front = q.Dequeue();
            for (int i = 0; i < 4; i++)
            {
                int tx = front.Item1 + dx[i];
                int ty = front.Item2 + dy[i];
                if (tx > 0 && tx < d && ty > 0 && ty < d && !map[tx, ty] && !flag[tx, ty])
                {
                    flag[tx, ty] = true;
                    q.Enqueue((tx, ty));
                }
            }
        }

        // 以越高越好，越远越好，只挑正面的原则选择一个位置
        flag[r, r] = false;
        if (FacingDirection == 1)
        {
            for (int i = r + 1; i < d; i++)
            {
                for (int j = d - 1; j > r; j--)
                {
                    if (Math.Abs(j - r) + Math.Abs(i - r) <= blackEnemyData.minDestinationRadius) continue;
                    if (flag[j, i] && map[j, i - 1])
                    {
                        workspace.Set(transform.position.x - r + j, transform.position.y - r + i);
                        testDestination = new Vector2Int(j, i);
                        testPosition = transform.position;
                        return workspace;
                    }
                }
            }
            for (int i = r; i > 0; i--)
            {
                for (int j = d - 1; j > r; j--)
                {
                    if (Math.Abs(j - r) + Math.Abs(i - r) <= blackEnemyData.minDestinationRadius) continue;
                    if (flag[j, i] && map[j, i-1])
                    {
                        workspace.Set(transform.position.x - r + j, transform.position.y - r + i);
                        testDestination = new Vector2Int(j,i);
                        testPosition = transform.position;
                        return workspace;
                    }
                }
            }
        }
        else
        {
            for (int i = r + 1; i < d; i++)
            {
                for (int j = 0; j < r; j++)
                {
                    if (Math.Abs(j - r) + Math.Abs(i - r) <= blackEnemyData.minDestinationRadius) continue;
                    if (flag[j, i] && map[j, i-1])
                    {
                        workspace.Set(transform.position.x - r + j, transform.position.y - r + i);
                        testDestination = new Vector2Int(j,i);
                        testPosition = transform.position;
                        return workspace;
                    }
                }
            }
            for (int i = r; i > 0; i--)
            {
                for (int j = 0; j < r; j++)
                {
                    if (Math.Abs(j - r) + Math.Abs(i - r) <= blackEnemyData.minDestinationRadius) continue;
                    if (flag[j, i] && map[j, i-1])
                    {
                        workspace.Set(transform.position.x - r + j, transform.position.y - r + i);
                        testDestination = new Vector2Int(j,i);
                        testPosition = transform.position;
                        return workspace;
                    }
                }
            }
        }
        return null;
    }

    public override void OnDead(int _direction)
    {
        base.OnDead(_direction);
        isDead = true;
        animator.SetBool("move", false);
        animator.SetBool("dead", true);
        this.gameObject.layer = LayerMask.NameToLayer("Body");
        workspace.Set(blackEnemyData.deadStartVelocityX * _direction * -1, blackEnemyData.deadStartVelocityY);
        rb2D.velocity = workspace;
        rb2D.constraints = RigidbodyConstraints2D.None;
        rb2D.angularVelocity = 90f * _direction * -1;
        EnableInfinitySpace = false;
        DropGoods();
        Destroy(this.gameObject, 5f);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = CheckCliff() ? Color.green : Color.red;
        workspace.Set(blackEnemyData.CliffDetectorRadius, blackEnemyData.CliffDetectorRadius);
        Gizmos.DrawWireCube(cliffDetector.position, workspace);
        Gizmos.color = CheckWall() ? Color.green : Color.red;
        workspace.Set(blackEnemyData.WallDetectorRadius, blackEnemyData.WallDetectorRadius);
        Gizmos.DrawWireCube(wallDetector.position, workspace);
        Gizmos.color = CheckGround() ? Color.green : Color.red;
        workspace.Set(blackEnemyData.GroundDetectorHalfWidth, blackEnemyData.GroundDetectorHalfHeight);
        Gizmos.DrawWireCube(groundDetector.position, workspace);
        if (isJumping)
        {
            int d = blackEnemyData.DestinationRadius * 2 - 1;
            int r = blackEnemyData.DestinationRadius;
            bool[,] map = new bool[d, d];
            bool[,] flag = new bool[d, d];
            // 遍历获取周围tile的情况
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    workspace.Set(testPosition.x - r + i, testPosition.y - r + j);
                    map[i, j] = CheckBlock(workspace);
                }
            }

            // 使用bfs获取有可能到达的位置
            Queue<(int, int)> q = new Queue<(int, int)>();
            q.Enqueue((r, r));
            flag[r, r] = true;
            while (q.Count() > 0)
            {
                (int, int) front = q.Dequeue();
                for (int i = 0; i < 4; i++)
                {
                    int tx = front.Item1 + dx[i];
                    int ty = front.Item2 + dy[i];
                    if (tx > 0 && tx < d && ty > 0 && ty < d && !map[tx, ty] && !flag[tx, ty])
                    {
                        flag[tx, ty] = true;
                        q.Enqueue((tx, ty));
                    }
                }
            }
            workspace.Set(0.1f, 0.1f);
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < d; j++)
                {
                    if (i == testDestination.x && j == testDestination.y) Gizmos.color = Color.blue;
                    else Gizmos.color = flag[i,j] ? Color.green : Color.red;
                    Gizmos.DrawWireCube(new Vector3(testPosition.x - r + i, testPosition.y - r + j), workspace);
                }
            }
        }
    }
}
