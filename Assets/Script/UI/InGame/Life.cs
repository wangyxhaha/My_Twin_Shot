using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : InGameUIBase
{
    [SerializeField]
    private Sprite[] sprites;
    public Image image{ get; private set; }
    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        image.sprite = sprites[player.GetComponent<Player>().GetLife()];
    }
}
