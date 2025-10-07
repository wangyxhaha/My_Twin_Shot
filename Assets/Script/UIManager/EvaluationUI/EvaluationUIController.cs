using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EvaluationUIController : UIController
{

    public override void Awake()
    {
        base.Awake();
    }

    public void OnCalled(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEventManager.Instance.CallNextLevelInvoke();
        }
    }
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
    
    void Update()
    {
        if (isActive)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                GameEventManager.Instance.CallNextLevelInvoke();
            }
        }
    }
}
