using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaluationUIController : UIController
{
    public override void SetActive(bool _b)
    {
        base.SetActive(_b);

        if (_b)
        {
            for (int i = 0; i < childrenUI.Length; i++)
            {
                if (childrenUI[i].name == "LevelScore")
                {
                    childrenUI[i].GetComponent<LevelScore>().FindPlayerAndRefreshScore();
                    break;
                }
            }
        }
        
    }
}
