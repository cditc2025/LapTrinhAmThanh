using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LocalMoveComponent : MonoBehaviour
{
    private List<AnimInfo> listMoveInfo = new List<AnimInfo>();
    private UnityAction endAction;
    private UnityAction onChangeAction;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (listMoveInfo.Count > 0)
        {
            AnimInfo info = listMoveInfo[0];

            if (info.totalTime > 0)
            {
                info.currentTime += Time.deltaTime;
                if (info.currentTime > info.totalTime)
                    info.currentTime = info.totalTime;
                float value = info.currentTime / info.totalTime;
                float t = info.ft(value, info.easeType);
                transform.localPosition = info.startValue + (info.endValue - info.startValue) * t;

                if (info.curve != null)
                {
                    float t1 = info.curve.Evaluate(t);
                    Debug.Log("curve " + t + " " + t1);
                    transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y * t1, transform.localPosition.z);
                }
                if (info.currentTime == info.totalTime)
                {
                    // finish
                    info.totalTime = 0;
                    listMoveInfo.Remove(info);
                    updatePos2Current();
                    if (listMoveInfo.Count == 0)
                    {
                        if (endAction != null)
                            endAction();
                    }
                }
                if (onChangeAction != null)
                    onChangeAction();
            }
            else
            {
                listMoveInfo.Remove(info);
                updatePos2Current();
                if (listMoveInfo.Count == 0)
                {
                    if (endAction != null)
                        endAction();
                }
            }
        }

    }
    private void updatePos2Current()
    {
        if (listMoveInfo.Count > 0)
        {
            AnimInfo info = listMoveInfo[0];
            info.startValue = transform.localPosition;
        }
    }
    public LocalMoveComponent moveTo(Vector3 pos, float duration)
    {
        AnimInfo info = AnimInfo.moveInfo();
        info.startValue = transform.localPosition;
        info.endValue = pos;
        info.totalTime = duration;
        info.currentTime = 0;
        listMoveInfo.Clear();
        listMoveInfo.Add(info);
        return this;
    }
    public LocalMoveComponent moveTo(Vector3 pos, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.moveInfo();
        info.startValue = transform.localPosition;
        info.endValue = pos;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listMoveInfo.Clear();
        listMoveInfo.Add(info);
        return this;
    }
    public LocalMoveComponent moveTo(Vector3 pos, float duration, AnimationCurve curve)
    {
        AnimInfo info = AnimInfo.moveInfo();
        info.startValue = transform.localPosition;
        info.endValue = pos;
        info.totalTime = duration;
        info.currentTime = 0;
        info.curve = curve;
        listMoveInfo.Clear();
        listMoveInfo.Add(info);
        return this;
    }

    public LocalMoveComponent thenMoveTo(Vector3 pos, float duration)
    {
        AnimInfo info = AnimInfo.moveInfo();
        info.startValue = transform.localPosition;
        info.endValue = pos;
        info.totalTime = duration;
        info.currentTime = 0;
        listMoveInfo.Add(info);
        return this;
    }

    public LocalMoveComponent thenMoveTo(Vector3 pos, float duration, int easeType)
    {
        AnimInfo info = AnimInfo.moveInfo();
        info.startValue = transform.localPosition;
        info.endValue = pos;
        info.totalTime = duration;
        info.currentTime = 0;
        info.easeType = easeType;
        listMoveInfo.Add(info);
        return this;
    }

    public LocalMoveComponent stopAll()
    {
        listMoveInfo.Clear();
        endAction = null;
        return this;
    }

    public LocalMoveComponent setEndAction(UnityAction eA)
    {
        endAction = eA;
        return this;
    }
    public LocalMoveComponent setChangeAction(UnityAction cA)
    {
        onChangeAction = cA;
        return this;
    }
}
