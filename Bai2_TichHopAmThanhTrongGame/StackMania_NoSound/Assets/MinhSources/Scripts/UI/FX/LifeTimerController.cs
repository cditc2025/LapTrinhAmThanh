using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeTimerController : MonoBehaviour
{
    public UIAnimationController controller;
    TMP_Text lifeTimeCountdown;

    private void Awake()
    {
        lifeTimeCountdown = GetComponent<TMP_Text>();
        if (controller)
        {
            controller.OnActivate += OnPopupEnable;
            controller.OnDeactivate += OnPopupDisable;
        }
    }

    private void OnDestroy()
    {
        PlayerResourceController.instance.OnLifeTimerChanged -= OnLifeTimerChanged;
    }

    public void OnLifeTimerChanged(double timeLeft)
    {
        lifeTimeCountdown.text = PlayerResourceController.ConvertTime(timeLeft);
    }

    private void OnPopupEnable()
    {
        if (PlayerResourceController.instance)
        {
            PlayerResourceController.instance.OnLifeTimerChanged += OnLifeTimerChanged;
        }
    }

    private void OnPopupDisable()
    {
        if (PlayerResourceController.instance)
        {
            PlayerResourceController.instance.OnLifeTimerChanged -= OnLifeTimerChanged;
        }
    }
    
}
