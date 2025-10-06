using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private Transform mainCam;
    private Vector3 workspace;

    void Start()
    {
        ;
    }

    void Update()
    {
        transform.position = mainCam.position;
        float tmp = mainCam.gameObject.GetComponent<Camera>().orthographicSize / GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        workspace.Set(tmp, tmp, tmp);
        transform.localScale = workspace;
    }
}
