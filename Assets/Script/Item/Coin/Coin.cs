using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    [SerializeField]
    private int points;
    private Coin()
    {
        itemName = "Coin";
    }

    public override void Start()
    {
        base.Start();
        
        InitializeFakeFigure();
    }
    public override void PickUp(Player _player)
    {
        _player.score += points;
        Destroy(this.gameObject);
    }
}
