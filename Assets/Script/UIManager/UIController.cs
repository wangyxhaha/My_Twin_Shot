using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    protected GameObject[] childrenUI;

    public virtual void Awake()
    {
        childrenUI = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childrenUI[i] = transform.GetChild(i).gameObject;
            // Debug.Log($"start UI controller, UI:{childrenUI[i]}");
        }
    }

    public virtual void SetActive(bool _b)
    {
        // Debug.Log
        for (int i = 0; i < childrenUI.Length; i++)
        {
            // Debug.Log(childrenUI[i]);
            childrenUI[i].SetActive(_b);
        }
    }

}
