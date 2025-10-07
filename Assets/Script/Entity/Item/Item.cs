using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Entity
{
    public string itemName { get; protected set; }
    [SerializeField]
    public float startingVelocity = 5f;

    public Rigidbody2D rb2D { get; private set; }

    public virtual void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        randomStartingVelocity();
    }
    public abstract void PickUp(Player p);
    protected void randomStartingVelocity()
    {
        float theta = Random.Range(Mathf.PI * 0.25f, Mathf.PI * 0.75f);
        Vector2 workspace = new Vector2(Mathf.Cos(theta) * startingVelocity, Mathf.Sin(theta) * startingVelocity);
        rb2D.velocity = workspace;
    }

    public virtual void FixedUpdate()
    {
        FakeFigureUpdate();
    }
}
