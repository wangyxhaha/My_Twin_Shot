using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField]
    private float velocity = -1f;

    [SerializeField]
    private float XLimit = -22.9167f;

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(velocity, 0f);
    }

    void Update()
    {
        if (transform.localPosition.x <= XLimit)
        {
            transform.localPosition = Vector2.zero;
        }
    }
}
