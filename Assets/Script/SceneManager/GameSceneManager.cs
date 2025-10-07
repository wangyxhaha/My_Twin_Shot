using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

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

    public int? CurrentLevel { get; private set; }
    private Scene menuScene;
    private Scene currentScene;

    void Start()
    {

        SubscribeEvent();

        LoadBasicScene();

        CurrentLevel = null;

        menuScene = SceneManager.GetSceneByName("UI");
        currentScene = menuScene;

        GameEventManager.Instance.CallMainMenuInvoke();
    }

    private void LoadBasicScene()
    {
        SceneManager.LoadScene("Background", LoadSceneMode.Additive);
    }

    private void SubscribeEvent()
    {
        GameEventManager.Instance.CallLevel += CallLevel;
        GameEventManager.Instance.CallMainMenu += CallMainMenu;
        GameEventManager.Instance.CallNextLevel += CallNextLevel;
    }

    private void CallNextLevel()
    {
        Debug.Log("next level");
        GameEventManager.Instance.CallLevelInvoke((int)CurrentLevel + 1);
    }

    private void CallLevel(int _level)
    {
        if (CurrentLevel != null)
        {
            SceneManager.UnloadSceneAsync($"Level{_level - 1}");
        }
        SceneManager.LoadScene($"Level{_level}", LoadSceneMode.Additive);
        Debug.Log("scene loaded");
        SceneManager.sceneLoaded += (Scene scene, LoadSceneMode mode) =>
        {
            currentScene = scene;
            SetActiveSceneToCurrentScene();
        };
        CurrentLevel = _level;
    }

    private void CallMainMenu()
    {
        if (CurrentLevel != null)
        {
            Debug.Log("unload");
            SceneManager.UnloadSceneAsync($"Level{CurrentLevel}");
            CurrentLevel = null;
        }
        currentScene = menuScene;
        SetActiveSceneToCurrentScene();
    }

    private void SetActiveSceneToCurrentScene() => SceneManager.SetActiveScene(currentScene);
}
