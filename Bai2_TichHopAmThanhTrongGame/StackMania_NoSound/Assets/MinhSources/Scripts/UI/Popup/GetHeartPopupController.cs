using System.Collections;
using System.Collections.Generic;
using KienChi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GetHeartPopupController : MonoBehaviour
{
    UIAnimationController controller;
    public GameObject popupFullLife;

    [Header("Popup Refill Life Settings")]
    public GameObject popupRefillLife;

    [Header("Refill Button")]
    public Button freeRefillBtn;
    public Button goldRefillBtn;
    public Button adsRefillBtn;

    int goldPriceToRefill = 1750;

    void Awake()
    {
        controller = GetComponent<UIAnimationController>();
        GameLevelManager.instance.onLifeNotEnough += OpenPopup;
        //
        controller.OnActivate += () =>
        {
            Debug.Log("add");
            PlayerResourceController.instance.OnLifeChanged += OnLifeChangedInit;
        };

        controller.OnDeactivate += () =>
        {
            Debug.Log("remove");
            PlayerResourceController.instance.OnLifeChanged -= OnLifeChangedInit;
        };
    }

    void Start()
    {
        //
        
        goldRefillBtn.onClick.AddListener(delegate
        {
            if (PlayerResourceController.instance.UseGold(goldPriceToRefill))
            {
                PlayerResourceController.instance.RefillLife(PlayerResourceController.instance.maxLifeCount);
                ClosePopup();
            }
        });

        adsRefillBtn.onClick.AddListener(delegate
        {
            PlayerResourceController.instance.RefillLife(1);
            ClosePopup();
        });
    }   

    public void OnLifeChangedInit(int life)
    {
        if (life == PlayerResourceController.instance.maxLifeCount)
        {
            popupFullLife.SetActive(true);
            popupRefillLife.SetActive(false);
        }
        else
        {
            popupRefillLife.SetActive(true);
            HandleRefillBtnStatus();
            popupFullLife.SetActive(false);
        }
        controller.UpdateObjectChange();
    }

    public void HandleRefillBtnStatus()
    {
        freeRefillBtn.gameObject.SetActive(false);
        adsRefillBtn.gameObject.SetActive(false);
        goldRefillBtn.gameObject.SetActive(true);
        goldRefillBtn.interactable = (PlayerResourceController.instance.gold >= goldPriceToRefill);
    }

    public void OpenPopup()
    {
        //
        OnLifeChangedInit(PlayerResourceController.instance.life);
        controller.Activate();
    }

    public void ClosePopup()
    {
        controller.Deactivate();
    }

    private void OnDestroy()
    {
        PlayerResourceController.instance.OnLifeChanged -= OnLifeChangedInit;
        GameLevelManager.instance.onLifeNotEnough -= OpenPopup;
    }
}
