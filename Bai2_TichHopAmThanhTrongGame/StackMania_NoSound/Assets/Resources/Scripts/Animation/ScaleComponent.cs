using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScaleComponent : MonoBehaviour
{
    private List<AnimInfo> listInfo = new List<AnimInfo>();
    private UnityAction endAction;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (listInfo.Count > 0)
        {
            AnimInfo info = listInfo[0];

            if (info.totalTime > 0)
            {
                info.currentTime += Time.deltaTime;
                if (info.currentTime > info.totalTime)
                    info.currentTime = info.totalTime;
                float t = info.ft(info.currentTime / info.totalTime, info.easeType);
                transform.localScale = info.startValue + (info.endValue - info.startValue) * t;
                if (info.currentTime == info.totalTime)
                {
                    // finish
                    info.totalTime = 0;
                    listInfo.Remove(info);
                    updatePos2Current();
                    if (listInfo.Count == 0)
                    {
                        if (endAction != null)
                            endAction();
                    }
                }
            }
            else
            {
                listInfo.Remove(info);
                updatePos2Current();
                if (listInfo.Count == 0)
                {
                    if (endAction != null)
                        endAction();
                }
            }
        }

    }
    private void updatePos2Current()
    {
        if (listInfo.Count > 0)
        {
            AnimInfo info = listInfo[0];
            info.startValue = transform.localScale;
            if (info.isBy)
                info.endValue = info.startValue + info.byValue;

        }
    }
    public ScaleComponent scaleTo(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.startValue = transform.localScale;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listInfo.Clear();
        listInfo.Add(info);
        return this;
    }
    public ScaleComponent scaleTo(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.startValue = transform.localScale;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listInfo.Clear();
        listInfo.Add(info);
        return this;
    }

    public ScaleComponent scaleBy(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.isBy = true;
        info.startValue = transform.localScale;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listInfo.Clear();
        listInfo.Add(info);
        return this;
    }
    public ScaleComponent scaleBy(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.isBy = true;
        info.startValue = transform.localScale;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listInfo.Clear();
        listInfo.Add(info);
        return this;
    }


    public ScaleComponent thenScaleTo(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.startValue = transform.localScale;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listInfo.Add(info);
        return this;
    }

    public ScaleComponent thenScaleTo(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.startValue = transform.localScale;
        info.endValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listInfo.Add(info);
        return this;
    }

    public ScaleComponent thenScaleBy(Vector3 value, float duration)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.isBy = true;
        info.startValue = transform.localScale;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        listInfo.Add(info);
        return this;
    }
    public ScaleComponent thenScaleBy(Vector3 value, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.scaleInfo();
        info.isBy = true;
        info.startValue = transform.localScale;
        info.endValue = info.startValue + value;
        info.byValue = value;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listInfo.Add(info);
        return this;
    }

    public void stopAll()
    {
        listInfo.Clear();
        endAction = null;
    }

    public void setEndAction(UnityAction eA)
    {
        endAction = eA;
    }
}
