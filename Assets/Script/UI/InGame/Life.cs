using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Life : MonoBehaviour
{
    private GameObject player;
    [SerializeField]
    private Sprite[] sprites;
    public Image image{ get; private set; }
    void Start()
    {
        image = GetComponent<Image>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        image.sprite = sprites[player.GetComponent<Player>().GetLife()];
    }
}
