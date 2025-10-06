using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class FragileBlock : MonoBehaviour
{
    [SerializeField]
    private Transform playerDetector;
    [SerializeField]
    private Vector2 detectorSize;
    [SerializeField]
    private LayerMask whatIsEntity;
    public Animator animator { get; private set; }
    public SpriteRenderer spriteRenderer{ get; private set; }
    public BoxCollider2D boxCollider2D{ get; private set; }
    private Color transparentColor;
    public Rigidbody2D rb2D{ get; private set; }

    private int playerStandingOnCounter;
    private bool isStand;
    private bool isBroken;
    private float brokenStartTime;
    void Awake()
    {
        Physics2D.autoSyncTransforms = true;
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb2D = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        playerStandingOnCounter = 0;
        isBroken = false;
        isStand = false;

        transparentColor = Color.white;
        transparentColor.a = 0f;
    }

    void FixedUpdate()
    {
        if (isBroken)
        {
            if (Time.time - brokenStartTime >= 3f)
            {
                animator.SetBool("smash", false);
                isBroken = false;
                spriteRenderer.color = Color.white;
                boxCollider2D.usedByEffector = true;
                this.gameObject.layer = LayerMask.NameToLayer("Ground");
            }
            return;
        }
        if (isStand)
        {
            playerStandingOnCounter++;
            if (playerStandingOnCounter == 30)
            {
                isBroken = true;
                isStand = false;
                playerStandingOnCounter = 0;
                brokenStartTime = Time.time;
                this.gameObject.layer = LayerMask.NameToLayer("BrokenGround");
                boxCollider2D.usedByEffector = false;
                animator.SetBool("smash", true);
            }
        }
        if (CheckPlayer())
        {
            isStand = true;
        }
    }

    private bool CheckPlayer()
    {
        return Physics2D.OverlapBox(playerDetector.position, detectorSize, 0f, whatIsEntity);
    }

    public void OnBroken()
    {
        spriteRenderer.color = transparentColor;
        Physics2D.SyncTransforms();
    }
}
