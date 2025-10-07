using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelScore : UI
{
    private Player player;
    private Text text;
    private int finalLevelScore;
    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = $"Level score:   {finalLevelScore}";
    }

    public void FindPlayerAndRefreshScore()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        finalLevelScore = player.score;
    }
}
