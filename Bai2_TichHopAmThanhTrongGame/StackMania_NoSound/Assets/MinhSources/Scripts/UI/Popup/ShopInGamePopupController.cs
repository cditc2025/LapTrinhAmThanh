using KienChi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopInGamePopupController : MonoBehaviour
{
    UIAnimationController uIAnimationController;
    public Scrollbar scrollbar;
    private WinPopupController winPopupController;

    private void Awake()
    {
        winPopupController = FindObjectOfType<WinPopupController>();
        uIAnimationController = GetComponent<UIAnimationController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NavigateToCoinShop()
    {
        scrollbar.value = 1;
        OpenPopup();
    }    

    public void OpenPopup()
    {
        uIAnimationController.Activate();
    }

    public void ClosePopup()
    {
        uIAnimationController.Deactivate();
        if (GameManager.Instance.isWinLevel)
        {
            winPopupController.ShowPopup();
        }
    }
}
