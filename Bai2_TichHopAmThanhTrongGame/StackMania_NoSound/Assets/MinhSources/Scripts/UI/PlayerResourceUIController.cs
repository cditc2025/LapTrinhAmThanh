using KienChi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourceUIController : MonoBehaviour
{
    public Transform canvas;
    [Header("Life Settings")]
    public Button addLifeBtn;
    public TMP_Text lifeCount;
    public GameObject lifeInfinite;
    public TMP_Text lifeTimer;
    GetHeartPopupController getHeartPopupController;

    [Header("Gold Settings")]
    public TMP_Text goldCount;
    public Button addGoldBtn;
    NavigationPageController pageController;
    ShopInGamePopupController shopInGamePopupController;
    WinPopupController winPopupController;
    private void Awake()
    {
        getHeartPopupController = FindObjectOfType<GetHeartPopupController>();
        pageController = FindAnyObjectByType<NavigationPageController>();
        shopInGamePopupController = FindAnyObjectByType<ShopInGamePopupController>();
        winPopupController = FindObjectOfType<WinPopupController>();
        //bind button event
        addLifeBtn.onClick.AddListener(delegate
        {
            getHeartPopupController.OpenPopup();
        });

        addGoldBtn.onClick.AddListener(delegate {
            if(!GameLevelManager.instance.isPlaying)
            {
                pageController.NavigateToGoldShop();
            } else
            {
                if (GameManager.Instance.isWinLevel)
                {
                    winPopupController.HidePopup();
                }
                shopInGamePopupController.NavigateToCoinShop();
            }
            
        });
        //
    }

    private void Start()
    {
        UpdateResourceOnStart();
        //bind event listener
        PlayerResourceController.instance.OnGoldChanged += OnGoldChanged;
        PlayerResourceController.instance.OnLifeTimerChanged += OnLifeTimerChanged;
        PlayerResourceController.instance.OnLifeChanged += OnLifeChanged;
        PlayerResourceController.instance.OnLifeInfiniteStateChanged += OnLifeInifiniteChanged;
        //
        if (GameLevelManager.instance.isPlaying)
        {
            GameLevelManager.instance.onGameLevelSuccess += OnGameLevelEnd;
            GameLevelManager.instance.onGameLevelFail += OnGameLevelEnd;
        }
    }
    public void OnGameLevelEnd(LevelResult result)
    {
        transform.SetParent(canvas);
        transform.SetAsLastSibling();
    }

    private void OnDestroy()
    {
        PlayerResourceController.instance.OnGoldChanged -= OnGoldChanged;
        PlayerResourceController.instance.OnLifeTimerChanged -= OnLifeTimerChanged;
        PlayerResourceController.instance.OnLifeChanged -= OnLifeChanged;
        PlayerResourceController.instance.OnLifeInfiniteStateChanged -= OnLifeInifiniteChanged;
        //
        GameLevelManager.instance.onGameLevelSuccess -= OnGameLevelEnd;
        GameLevelManager.instance.onGameLevelFail -= OnGameLevelEnd;
    }

    public void UpdateResourceOnStart()
    {
        OnGoldChanged(PlayerResourceController.instance.gold);
        OnLifeChanged(PlayerResourceController.instance.life);
        OnLifeInifiniteChanged(PlayerResourceController.instance.isInfiniteLife);
    }

    public void OnGoldChanged(int gold)
    {
        goldCount.text = gold.ToString();
    }

    public void OnLifeChanged(int life)
    {
        lifeCount.text = life.ToString();
        if(PlayerResourceController.instance.isFullLife)
        {
            lifeTimer.text = "Full";
        }
    }

    public void OnLifeTimerChanged(double timeLeft)
    {
        lifeTimer.text = PlayerResourceController.ConvertTime(timeLeft);
    }

    public void OnLifeInifiniteChanged(bool isInfinite)
    {
        if (isInfinite)
        {
            lifeInfinite.SetActive(true);
            lifeCount.gameObject.SetActive(false);
        }
        else
        {
            lifeInfinite.SetActive(false);
            lifeCount.gameObject.SetActive(true);
            if (PlayerResourceController.instance.isFullLife)
            {
                lifeTimer.text = "Full";
            }
        }
    }


}
