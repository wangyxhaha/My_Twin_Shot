using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalScore : UI
{
    private Recorder recorder;
    private Text text;
    void Start()
    {
        recorder = GameObject.Find("Recorder").GetComponent<Recorder>();
        text = GetComponent<Text>();
    }

    void Update()
    {
        text.text = $"Total score:   {recorder.TotalScore}";
    }
}
