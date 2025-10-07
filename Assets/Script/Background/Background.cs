using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Transform mainCam;
    private Vector3 workspace;

    void Start()
    {
        mainCam = GameObject.Find("Main Camera").transform;
    }

    void Update()
    {
        workspace = mainCam.position;
        workspace.z = 0f;
        transform.position = workspace;
        float tmp = mainCam.gameObject.GetComponent<Camera>().orthographicSize * 2f / GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        workspace.Set(tmp, tmp, tmp);
        transform.localScale = workspace;
    }
}
