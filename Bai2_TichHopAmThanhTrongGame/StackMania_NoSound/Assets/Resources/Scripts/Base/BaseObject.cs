using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BaseObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void doSmthAfter(float time, UnityAction action)
    {
        StartCoroutine(ExecuteAfterTime(time, action));
    }

    IEnumerator ExecuteAfterTime(float time, UnityAction action)
    {
        yield return new WaitForSeconds(time);

        // Code to execute after the delay
        action();
    }
    protected void clearAllChildren(Transform t)
    {
        for (int i = t.childCount - 1; i >= 0; i--)
        {
            Destroy(t.GetChild(i).gameObject);
        }
    }
}
