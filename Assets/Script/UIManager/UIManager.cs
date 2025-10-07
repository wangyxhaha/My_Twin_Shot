using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private InGameUIController InGameUI;
    [SerializeField]
    private UIController MainMenuUI;
    [SerializeField]
    private EvaluationUIController EvaluationUI;
    [SerializeField]
    private PauseUIController PauseUI;

    public void Start()
    {
        SubscribeEvent();
    }

    private void DeactivateAllUI()
    {
        InGameUI.SetActive(false);
        MainMenuUI.SetActive(false);
        EvaluationUI.SetActive(false);
        PauseUI.SetActive(false);
    }

    private void SubscribeEvent()
    {
        GameEventManager.Instance.CallMainMenu += () =>
        {
            Debug.Log("call main menu");
            DeactivateAllUI();
            MainMenuUI.SetActive(true);
        };
        GameEventManager.Instance.CallLevelSeletingMenu += () =>
        {
            DeactivateAllUI();

            //test
            GameEventManager.Instance.CallLevelInvoke(1);
        };
        GameEventManager.Instance.CallEvaluationUI += () =>
        {
            DeactivateAllUI();
            EvaluationUI.SetActive(true);
        };
        GameEventManager.Instance.CallPauseLevel += () =>
        {
            DeactivateAllUI();
            PauseUI.SetActive(true);
        };
        GameEventManager.Instance.CallResumeLevel += () =>
        {
            DeactivateAllUI();
            InGameUI.SetActive(true);
        };
    }
}
