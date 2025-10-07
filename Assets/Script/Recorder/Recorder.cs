using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recorder : MonoBehaviour
{
    public static Recorder Instance{ get; private set; }

    void Awake()
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

    private void SubscribeEvent()
    {
        GameEventManager.Instance.CallLevel += (int _l) =>
        {
            currentLevel = _l;
        };
    }

    public int TotalScore;
    public int preTotalScore;
    public int CurrentEnemyCount;
    public int currentLevel;

    public void ResetScore()
    {
        TotalScore = 0;
    }

    public void CheckEnemyCount()
    {
        if (CurrentEnemyCount == 0)
        {
            GameEventManager.Instance.CallEvaluationUIInvoke();
        }
    }

    void Start()
    {
        ResetScore();

        CurrentEnemyCount = 0;

        SubscribeEvent();
    }
}
