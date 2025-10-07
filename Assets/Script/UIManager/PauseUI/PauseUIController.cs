using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseUIController : UIController
{
    private PlayerInput UIInput;

    public override void Awake()
    {
        base.Awake();
        UIInput = GetComponent<PlayerInput>();
    }
    public void Start()
    {
        GameEventManager.Instance.CallResumeLevel += Resume;
        GameEventManager.Instance.EnableUIInput += EnableUIInput;
        GameEventManager.Instance.DisableUIInput += DisableUIInput;
        // GameEventManager.Instance.CallMainMenu += Resume;
    }

    private void Resume() => Time.timeScale = 1f;
    private void EnableUIInput() => UIInput.enabled = true;
    private void DisableUIInput() => UIInput.enabled = false;

    public override void SetActive(bool _b)
    {
        base.SetActive(_b);


        if (_b)
        {
            GameEventManager.Instance.DisablePlayerInputInvoke();
            GameEventManager.Instance.EnableUIInputInvoke();
            Time.timeScale = 0f;
        }
        else
        {
            // Debug.Log(GameEventManager.Instance);
            GameEventManager.Instance.DisableUIInputInvoke();
            GameEventManager.Instance.EnablePlayerInputInvoke();
            Resume();
        }
    }
}
