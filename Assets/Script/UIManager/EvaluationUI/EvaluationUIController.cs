using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EvaluationUIController : UIController
{
    private PlayerInput UIInput;
    
    public override void Awake()
    {
        base.Awake();
        UIInput = GetComponent<PlayerInput>();
    }
    public void Start()
    {
        GameEventManager.Instance.EnableUIInput += EnableUIInput;
        GameEventManager.Instance.DisableUIInput += DisableUIInput;
        // GameEventManager.Instance.CallMainMenu += Resume;
    }
    private void EnableUIInput() => UIInput.enabled = true;
    private void DisableUIInput() => UIInput.enabled = false;

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
            GameEventManager.Instance.DisablePlayerInputInvoke();
            GameEventManager.Instance.EnableUIInputInvoke();
            for (int i = 0; i < childrenUI.Length; i++)
            {
                if (childrenUI[i].name == "LevelScore")
                {
                    childrenUI[i].GetComponent<LevelScore>().FindPlayerAndRefreshScore();
                    break;
                }
            }
        }
        else
        {
            GameEventManager.Instance.DisableUIInputInvoke();
            GameEventManager.Instance.EnablePlayerInputInvoke();
        }
    }
}
