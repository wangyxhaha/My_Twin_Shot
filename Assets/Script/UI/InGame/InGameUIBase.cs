using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIBase : UI
{
    protected Player player;

    public virtual void FindPlayer()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
}
