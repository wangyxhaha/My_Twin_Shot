using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIController : UIController
{
    public void FindPlayer()
    {
        for (int i = 0; i < childrenUI.Length; i++)
        {
            childrenUI[i].GetComponent<InGameUIBase>().FindPlayer();
        }
    }

    public override void SetActive(bool _b)
    {
        base.SetActive(_b);

        if (_b) FindPlayer();
    }

    public void Start()
    {
        GameEventManager.Instance.CallLevelOnLoad += () => SetActive(true);
    }
}
