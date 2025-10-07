using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : Item
{
    [SerializeField]
    public override void Start()
    {
        base.Start();

        InitializeFakeFigure();
    }
    public override void PickUp(Player _player)
    {
        _player.ClearBuff();
        _player.isFlyBuff = true;

        // _player.StartBuffTimer();
        
        Destroy(this.gameObject);
    }
}
