using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileTexture : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    [SerializeField]
    private Sprite[] sprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeToRandomTexture();
    }

    private void ChangeToRandomTexture()
    {
        System.Random random = new System.Random((int)(transform.position.x * transform.position.y * 100f));
        spriteRenderer.sprite = sprites[random.Next(sprites.Length)];
        // transform.Rotate(0f, 180f * random.Next(2), 0f);
    }
}
