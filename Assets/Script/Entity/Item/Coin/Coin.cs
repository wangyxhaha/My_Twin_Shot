using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    [SerializeField]
    private int points;
    private Recorder recorder;
    private Coin()
    {
        itemName = "Coin";
    }

    public override void Start()
    {
        base.Start();

        InitializeFakeFigure();

        recorder = GameObject.Find("Recorder").GetComponent<Recorder>();
    }
    public override void PickUp(Player _player)
    {
        GameEventManager.Instance.PlayCoinAudioInvoke();
        _player.score += points;
        recorder.TotalScore += points;
        Destroy(this.gameObject);
    }
}
