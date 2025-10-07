using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buff : InGameUIBase
{
    [SerializeField]
    private Sprite[] sprites;
    public Image image{ get; private set; }

    private Color transparent;
    void Start()
    {
        image = GetComponent<Image>();

        transparent = Color.white;
        transparent.a = 0f;
    }

    void Update()
    {
        if (player.isFasterBuff)
        {
            image.sprite = sprites[0];
        }
        else if (player.isFlyBuff)
        {
            image.sprite = sprites[1];
        }
        else if (player.isGodBuff)
        {
            image.sprite = sprites[2];
        }
        else
        {
            image.color = transparent;
            return;
        }
        image.color = Color.white;
    }
}
