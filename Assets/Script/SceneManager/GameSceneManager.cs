using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public int? CurrentLevel { get; private set; }
    private Scene menuScene;
    private Scene currentScene;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

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
