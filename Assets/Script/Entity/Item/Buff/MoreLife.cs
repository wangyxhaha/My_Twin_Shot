using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreLife : Item
{
    [SerializeField]
    public override void Start()
    {
        base.Start();

        InitializeFakeFigure();
    }
    public override void PickUp(Player _player)
    {
        // _player.ClearBuff();
        GameEventManager.Instance.PlayBuffAudioInvoke();
        _player.LifePlusPlus();
        Debug.Log("More Life!");
        
        Destroy(this.gameObject);
    }
}
