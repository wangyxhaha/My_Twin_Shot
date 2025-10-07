using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUIController : UIController
{

    public override void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        GameEventManager.Instance.CallResumeLevel += Resume;
    }

    private void Resume() => Time.timeScale = 1f;

    public override void SetActive(bool _b)
    {
        base.SetActive(_b);


        if (_b)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Resume();
        }
    }

    void Update()
    {
        if (isActive)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                GameEventManager.Instance.CallResumeLevelInvoke();
            }
        }
    }
}
