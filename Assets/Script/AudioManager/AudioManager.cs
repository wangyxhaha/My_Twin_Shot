using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip MenuBGM;
    [SerializeField]
    private AudioClip InGameBGM;

    private int? preLevel;

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

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        preLevel = GameSceneManager.Instance.CurrentLevel;
    }

    void Update()
    {
        if (preLevel != GameSceneManager.Instance.CurrentLevel)
        {
            audioSource.Stop();
            if (GameSceneManager.Instance.CurrentLevel == null)
            {
                audioSource.clip = MenuBGM;
            }
            else
            {
                audioSource.clip = InGameBGM;
            }
            audioSource.Play();
        }
        preLevel = GameSceneManager.Instance.CurrentLevel;
    }
}
