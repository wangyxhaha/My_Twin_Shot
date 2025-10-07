using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public event Action CallMainMenu;
    public void CallMainMenuInvoke() => CallMainMenu?.Invoke();

    public event Action CallLevelSeletingMenu;
    public void CallLevelSeletingMenuInvoke() => CallLevelSeletingMenu?.Invoke();

    public event Action<int> CallLevel;
    public void CallLevelInvoke(int _l) => CallLevel?.Invoke(_l);

    public event Action CallLevelOnLoad;
    public void CallLevelOnLoadInvoke() => CallLevelOnLoad?.Invoke();

    public event Action CallLevelOnDestroy;
    public void CallLevelOnDestroyInvoke() => CallLevelOnDestroy?.Invoke();

    public event Action CallEvaluationUI;
    public void CallEvaluationUIInvoke() => CallEvaluationUI?.Invoke();

    public event Action KillAllEnemy;
    public void KillAllEnemyInvoke() => KillAllEnemy?.Invoke();

    public event Action CallPauseLevel;
    public void CallPauseLevelInvoke() => CallPauseLevel?.Invoke();

    public event Action CallResumeLevel;
    public void CallResumeLevelInvoke() => CallResumeLevel?.Invoke();

    public event Action EnablePlayerInput;
    public void EnablePlayerInputInvoke() => EnablePlayerInput?.Invoke();

    public event Action DisablePlayerInput;
    public void DisablePlayerInputInvoke() => DisablePlayerInput?.Invoke();

    public event Action EnableUIInput;
    public void EnableUIInputInvoke() => EnableUIInput?.Invoke();

    public event Action DisableUIInput;
    public void DisableUIInputInvoke() => DisableUIInput?.Invoke();

    public event Action PlayCoinAudio;
    public void PlayCoinAudioInvoke() => PlayCoinAudio?.Invoke();

    public event Action PlayBuffAudio;
    public void PlayBuffAudioInvoke() => PlayBuffAudio?.Invoke();

    public event Action CallNextLevel;
    public void CallNextLevelInvoke() => CallNextLevel?.Invoke();
}
