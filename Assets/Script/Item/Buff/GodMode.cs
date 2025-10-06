using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodMode : Item
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
        _player.isGodBuff = true;
        Debug.Log("More Life!");
        _player.StartBuffTimer();
        
        Destroy(this.gameObject);
    }
}
