using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimInfo
{
    public static int TYPE_MOVE = 1;
    public static int TYPE_ROTATE = 2;
    public static int TYPE_SCALE = 3;
    public int type;

    public AnimInfo(int type)
    {
        this.type = type;
    }

    public static AnimInfo moveInfo()
    {
        return new AnimInfo(TYPE_MOVE);
    }
    public static AnimInfo rotateInfo()
    {
        return new AnimInfo(TYPE_ROTATE);
    }
    public static AnimInfo scaleInfo()
    {
        return new AnimInfo(TYPE_SCALE);
    }

    public bool isBy = false;
    public Vector3 startValue;
    public Vector3 endValue;
    public Vector3 currentValue;
    public Vector3 byValue;
    public Quaternion qstartValue;
    public Quaternion qendValue;
    public Quaternion qcurrentValue;
    public Quaternion qbyValue;
    public float totalTime = 0;
    public float currentTime = 0;
    public int easeType = 0;

    public AnimationCurve curve = null;

    public float ft(float t, int easeType)
    {
        return OwnAnimationFormular.ft(t, easeType);
    }
}
