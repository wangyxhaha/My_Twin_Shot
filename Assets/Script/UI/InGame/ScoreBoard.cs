using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    private GameObject player;
    public Text text{ get; private set; }
    void Start()
    {
        player = GameObject.Find("Player");
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = player.GetComponent<Player>().score.ToString();
    }
}
