using System;
using System.Collections;
using System.Collections.Generic;
using KienChi;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RevivePopupController : MonoBehaviour
{
    UIAnimationController controller;
    public TMP_Text levelText;

    [Header("Life Settings")]
    public GameObject lifeContainer;
    public GameObject lifeInfinite;
    [Header("Slot")]
    public List<GameObject> slotGO;
    public TMP_Text addNumSlot;
    public TMP_Text coin;

    [Header("Button")]
    public Button retryGoldBtn;
    public Button retryBtn;
    public Button leaveBtn;

    LosePopupController losePopupController;

    private void Awake()
    {
        controller = GetComponent<UIAnimationController>();
        controller.OnActivate += () =>
        {
            Debug.Log("add");
            PlayerResourceController.instance.OnLifeInfiniteStateChanged += OnLifeInifiniteChanged;
        };

        controller.OnDeactivate += () =>
        {
            Debug.Log("remove");
            PlayerResourceController.instance.OnLifeInfiniteStateChanged -= OnLifeInifiniteChanged;
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        losePopupController = FindObjectOfType<LosePopupController>();
        GameLevelManager.instance.onGameLevelFail += OpenPopup;
        //
        leaveBtn.onClick.AddListener(delegate
        {
            ClosePopup();
            losePopupController.OpenPopup();
        });

        retryBtn.onClick.AddListener(delegate
        {
            GameLevelManager.instance.RetryGame();
        });
        retryGoldBtn.onClick.AddListener(delegate
        {
            RetryGold();
        });
    }

    public void OnLifeInifiniteChanged(bool isInfinite)
    {
        if (isInfinite)
        {
            lifeInfinite.SetActive(true);
            lifeContainer.SetActive(false);
            //
            retryBtn.gameObject.SetActive(true);
            leaveBtn.gameObject.SetActive(false);
        }
        else
        {
            lifeInfinite.SetActive(false);
            lifeContainer.SetActive(true);
            //
            retryBtn.gameObject.SetActive(false);
            leaveBtn.gameObject.SetActive(true);
        }
    }
    private int gold;
    private int maxAddSlot;
    public void AddSlot()
    {
        foreach (GameObject slot in slotGO)
        {
            slot.SetActive(false);
        }
        maxAddSlot = Consts.MAX_SLOTS_PER_ROW - SlotManager.Instance.Slots.Count;
        if (maxAddSlot > 2) maxAddSlot = 2;
        gold = 900;
        for (int i = 0; i < maxAddSlot; i++)
        {
            slotGO[i].SetActive(true);
            //gold += i * 25;
        }
        addNumSlot.text = $"Revive with extra <color=#42B118>{maxAddSlot}</color> slots?";
        coin.text = gold.ToString();
    }
    public void RetryGold()
    {
        // PlayerResourceController.instance.GainGold(10000);
        Debug.Log(gold);
        if (!PlayerResourceController.instance.UseGold(gold)) return;

        GameLevelManager.instance.UpdateReviveCount();

        for (int i = 0; i < maxAddSlot; i++)
            SlotManager.Instance.SlotAds.AddSlot(false);

        Revive();
    }

    public void Revive()
    {
        GameManager.Instance.isFinishLevel = false;
        controller.Deactivate();
        //
        GameManager.Instance.CheckLoseGame();
    }    

    public void OpenPopup(LevelResult result)
    {
        //
        AddSlot();

        bool isInfinite = PlayerResourceController.instance.isInfiniteLife;
        OnLifeInifiniteChanged(isInfinite);
        //
        levelText.text = $"Level {GameLevelManager.instance.currentLevel}";

        retryGoldBtn.interactable = PlayerResourceController.instance.gold >= gold;

        if (Consts.MAX_SLOTS_PER_ROW - SlotManager.Instance.Slots.Count > 0)
            StartCoroutine(OpenPopupAfterDelay(0.5f));
        else
            losePopupController.OpenPopup();
    }

    IEnumerator OpenPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        controller.Activate();
        //
    }

    public void ClosePopup()
    {
        //
        controller.Deactivate();
    }

    private void OnDestroy()
    {
        PlayerResourceController.instance.OnLifeInfiniteStateChanged -= OnLifeInifiniteChanged;
        GameLevelManager.instance.onGameLevelFail -= OpenPopup;
    }
}
