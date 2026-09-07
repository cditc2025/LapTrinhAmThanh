using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnAnimationFormular : MonoBehaviour
{
    public const int EASE_NONE = 0;
    public const int EASE_OUTELASTIC = 1;
    public const int EASE_OUTBACK = 2;
    public const int EASE_OUTBOUNCE = 3;
    public const int EASE_INOUTBACK = 4;
    public const int EASE_OUTCUBIC = 5;
    public const int EASE_INCUBIC = 6;
    public const int EASE_INQUINT = 7;
    public const int EASE_INOUTSINE = 8;
    public const int EASE_OUTSINE = 9;
    public const int EASE_INOUTCUBIC = 10;
    public const int EASE_INBACK = 11;
    public const int EASE_OUTQUINT = 12;
    public const int EASE_INOUTQUINT = 13;
    public const int EASE_INSINE = 14;
    public const int EASE_INOUTQUART = 15;
    public const int EASE_INOUTQUAD = 16;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public static float ft(float t, int easeType)
    {
        if (easeType == EASE_NONE)
            return t;
        else if (easeType == EASE_OUTELASTIC)
        {
            float c4 = (2 * Mathf.PI) / 3;

            return t == 0
              ? 0
              : t == 1
              ? 1
              : Mathf.Pow(2, -10 * t) * Mathf.Sin((t * 10 - 0.75f) * c4) + 1;
        }
        else if (easeType == EASE_OUTBACK)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return 1 + c3 * Mathf.Pow(t - 1, 3) + c1 * Mathf.Pow(t - 1, 2);
        }
        else if (easeType == EASE_OUTBOUNCE)
        {
            float n1 = 7.5625f;
            float d1 = 2.75f;

            if (t < 1 / d1)
            {
                return n1 * t * t;
            }
            else if (t < 2 / d1)
            {
                return n1 * (t -= 1.5f / d1) * t + 0.75f;
            }
            else if (t < 2.5 / d1)
            {
                return n1 * (t -= 2.25f / d1) * t + 0.9375f;
            }
            else
            {
                return n1 * (t -= 2.625f / d1) * t + 0.984375f;
            }
        }
        else if (easeType == EASE_INOUTBACK)
        {
            float c1 = 1.70158f;
            float c2 = c1 * 1.525f;

            return t < 0.5
              ? (Mathf.Pow(2 * t, 2) * ((c2 + 1) * 2 * t - c2)) / 2
              : (Mathf.Pow(2 * t - 2, 2) * ((c2 + 1) * (t * 2 - 2) + c2) + 2) / 2;
        }
        else if (easeType == EASE_OUTCUBIC)
        {
            return 1 - Mathf.Pow(1 - t, 3);
        }
        else if (easeType == EASE_INCUBIC)
        {
            return t * t * t;
        }
        else if (easeType == EASE_INQUINT)
        {
            return t * t * t * t * t;
        }
        else if (easeType == EASE_INOUTSINE)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }
        else if (easeType == EASE_OUTSINE)
        {
            return Mathf.Sin((t * Mathf.PI) / 2);
        }
        else if (easeType == EASE_INOUTCUBIC)
        {
            return t < 0.5 ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;
        }
        else if (easeType == EASE_INBACK)
        {
            float c1 = 1.70158f;
            float c3 = c1 + 1;

            return c3 * t * t * t - c1 * t * t;
        }
        else if (easeType == EASE_OUTQUINT)
        {
            return 1 - Mathf.Pow(1 - t, 5);
        }
        else if (easeType == EASE_INOUTQUINT)
        {
            return t < 0.5f ? 16 * t * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 5) / 2;
        }
        else if (easeType == EASE_INSINE)
        {
            return 1 - Mathf.Cos((t * Mathf.PI) / 2);
        }
        else if (easeType == EASE_INOUTQUART)
        {
            return t < 0.5 ? 8 * t * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 4) / 2;
        }
        else if (easeType == EASE_INOUTQUAD)
        {
            return t < 0.5 ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;
        }

        return t;
    }
}
