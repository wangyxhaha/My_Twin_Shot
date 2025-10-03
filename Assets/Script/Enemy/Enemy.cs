using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Animator animator { get; protected set; }
    public Rigidbody2D rb2D { get; protected set; }
    public CapsuleCollider2D capsuleCollider2D { get; protected set; }
    protected int FacingDirection;
    protected ContactFilter2D arrowFilter;
    protected Vector2 workspace;
    protected bool isDead = false;
    protected void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    public abstract void OnDead(int _direction);
    protected void SetVelocityX(float _velocity)
    {
        workspace.Set(_velocity, rb2D.velocity.y);
        rb2D.velocity = workspace;
    }
    protected void SetVelocityY(float _velocity)
    {
        workspace.Set(rb2D.velocity.x, _velocity);
        rb2D.velocity = workspace;
    }
}
