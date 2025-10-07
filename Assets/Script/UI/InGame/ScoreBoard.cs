using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : InGameUIBase
{
    public Text text{ get; private set; }
    void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        text.text = player.GetComponent<Player>().score.ToString();
    }
}
