using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LifeContainerController : MonoBehaviour
{
    public UIAnimationController controller;
    public bool hasMissingLifeState = true;
    void Awake()
    {
        if(controller)
        {
            controller.OnActivate += OnPopupEnable;
            controller.OnDeactivate += OnPopupDisable;
        }
    }

    private void OnPopupEnable()
    {
        
        if(PlayerResourceController.instance)
        {
            UpdateLifeUI(PlayerResourceController.instance.life);
            PlayerResourceController.instance.OnLifeChanged += UpdateLifeUI;
        }
    }

    private void OnPopupDisable()
    {
  
        if (PlayerResourceController.instance)
        {
            PlayerResourceController.instance.OnLifeChanged -= UpdateLifeUI;
        }
    }

    private void OnDestroy()
    {
        PlayerResourceController.instance.OnLifeChanged -= UpdateLifeUI;
    }

    private void OnEnable()
    {
        UpdateLifeUI(PlayerResourceController.instance.life);
    }

    public void UpdateLifeUI(int currentLife)
    {
        int maxLife = PlayerResourceController.instance.maxLifeCount;
        for (int i = 0; i < maxLife; i++)
        {
            Animator lifeIcon = transform.GetChild(i).GetComponent<Animator>();

            if (i < currentLife - 1)
            {
                lifeIcon.SetInteger("state", 0);
            }
            else if (i == currentLife - 1)
            {
                if(hasMissingLifeState)
                {
                    lifeIcon.SetInteger("state", 1);
                } else
                {
                    lifeIcon.SetInteger("state", 0);
                }
               
            }
            else
            {
                lifeIcon.SetInteger("state", 2);
            }
        }
    }
}
