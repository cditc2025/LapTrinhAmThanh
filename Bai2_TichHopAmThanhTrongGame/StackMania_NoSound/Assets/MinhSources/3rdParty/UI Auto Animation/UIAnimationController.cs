using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(UIAutoAnimation), typeof(CanvasGroup))]
public class UIAnimationController : MonoBehaviour
{
    public bool isActive = false;
    CanvasGroup canvasGroup;
    UIAutoAnimation uIAutoAnimation;
    public UnityAction OnActivate;
    public UnityAction OnDeactivate;

    //compatible with scrollrect
    List<Behaviour> components;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        uIAutoAnimation = GetComponent<UIAutoAnimation>();
        UpdateCanvasGroup();
        //
        FindIncompatibleComponent();
    }

    private void FindIncompatibleComponent()
    {
        components = new List<Behaviour>();
        components.AddRange(GetComponentsInChildren<ScrollRect>());
        //components.AddRange(GetComponentsInChildren<Animator>());
    }

    private void DisableIncompatibleComponent()
    {
        for(int i = 0; i < components.Count; i ++)
        {
            components[i].enabled = false;
        }
    }

    private void EnableIncompatibleComponent()
    {
        for (int i = 0; i < components.Count; i++)
        {
            components[i].enabled = true;
        }
    }

    public void UpdateObjectChange()
    {
        uIAutoAnimation.SetAllEntranceState();
        uIAutoAnimation.GetComponentsList();
        if(!isActive)
        {
            uIAutoAnimation.SetAllExitState();
        } 
    }

    public void Activate()
    {
        if (!isActive)
        {
            CancelInvoke();
            
            isActive = true;
            UpdateCanvasAlpha();

            //run animation
            DisableIncompatibleComponent();
            uIAutoAnimation.EntranceAnimation();
            Invoke(nameof(UpdateCanvasInteractable), uIAutoAnimation.GetMaxEntranceDuration());
            Invoke(nameof(EnableIncompatibleComponent), uIAutoAnimation.GetMaxEntranceDuration());
            //
            OnActivate?.Invoke();
        }
    }

    public void Deactivate()
    {
        if (isActive)
        {
            CancelInvoke();
            isActive = false;

            //run animation
            DisableIncompatibleComponent();
            uIAutoAnimation.ExitAnimation();
            Invoke(nameof(UpdateCanvasGroup), uIAutoAnimation.GetMaxExitDuration());
            Invoke(nameof(EnableIncompatibleComponent), uIAutoAnimation.GetMaxExitDuration());

            //
            OnDeactivate?.Invoke();
        }
    }

    public void OnValidate()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        uIAutoAnimation = GetComponent<UIAutoAnimation>();
    }

    public void UpdateCanvasGroup()
    {
        UpdateCanvasAlpha();
        UpdateCanvasInteractable();
    }

    public void UpdateCanvasAlpha()
    {
        canvasGroup.alpha = isActive ? 1 : 0;
    }

    public void UpdateCanvasInteractable()
    {
        canvasGroup.interactable = isActive;
        canvasGroup.blocksRaycasts = isActive;
    }
}
