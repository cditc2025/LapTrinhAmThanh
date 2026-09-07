using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RotateComponent : MonoBehaviour
{
    private List<AnimInfo> listRotateInfo = new List<AnimInfo>();
    private UnityAction endAction;
    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (listRotateInfo.Count > 0)
        {
            AnimInfo info = listRotateInfo[0];

            if (info.totalTime > 0)
            {
                info.currentTime += Time.deltaTime;
                if (info.currentTime > info.totalTime)
                    info.currentTime = info.totalTime;
                float t = info.ft(info.currentTime / info.totalTime, info.easeType);
                transform.eulerAngles = info.startValue + (info.endValue - info.startValue) * t;
                //Debug.Log("rotateComponent " + transform.eulerAngles);
                if (info.currentTime == info.totalTime)
                {
                    // finish
                    info.totalTime = 0;
                    listRotateInfo.Remove(info);
                    updatePos2Current();
                    if (listRotateInfo.Count == 0)
                    {
                        if (endAction != null)
                            endAction();
                        endAction = null;
                    }
                }
            }
            else
            {
                listRotateInfo.Remove(info);
                updatePos2Current();
                if (listRotateInfo.Count == 0)
                {
                    Debug.Log("prepare EndAction");
                    if (endAction != null)
                        endAction();
                    endAction = null;
                }
            }
        }

    }
    private void updatePos2Current()
    {
        if (listRotateInfo.Count > 0)
        {
            AnimInfo info = listRotateInfo[0];
            info.startValue = transform.eulerAngles;
            if (info.isBy)
                info.endValue = info.startValue + info.byValue;

        }
    }
    public RotateComponent rotateTo(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.startValue = transform.eulerAngles;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listRotateInfo.Clear();
        listRotateInfo.Add(info);
        return this;
    }
    public RotateComponent rotateTo(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.startValue = transform.eulerAngles;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listRotateInfo.Clear();
        listRotateInfo.Add(info);
        return this;
    }

    public RotateComponent rotateBy(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.isBy = true;
        info.startValue = transform.eulerAngles;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listRotateInfo.Clear();
        listRotateInfo.Add(info);
        return this;
    }
    public RotateComponent rotateBy(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.isBy = true;
        info.startValue = transform.eulerAngles;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listRotateInfo.Clear();
        listRotateInfo.Add(info);
        return this;
    }


    public RotateComponent thenRotateTo(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.startValue = transform.eulerAngles;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listRotateInfo.Add(info);
        return this;
    }

    public RotateComponent thenRotateTo(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.startValue = transform.eulerAngles;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listRotateInfo.Add(info);
        return this;
    }

    public RotateComponent thenRotateBy(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.isBy = true;
        info.startValue = transform.eulerAngles;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listRotateInfo.Add(info);
        return this;
    }
    public RotateComponent thenRotateBy(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.rotateInfo();
        info.isBy = true;
        info.startValue = transform.eulerAngles;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listRotateInfo.Add(info);
        return this;
    }

    public void stopAll()
    {
        listRotateInfo.Clear();
        endAction = null;
    }

    public void setEndAction(UnityAction eA)
    {
        endAction = eA;
    }

    private void testRotate()
    {
        rotateBy(new Vector3(0, 360, 0), 3).setEndAction(() => { testRotate(); });
    }
}
