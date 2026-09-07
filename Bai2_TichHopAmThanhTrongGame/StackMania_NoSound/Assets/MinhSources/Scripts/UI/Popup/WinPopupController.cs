using System;
using System.Collections;
using System.Collections.Generic;
using KienChi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinPopupController : MonoBehaviour
{
    UIAnimationController controller;
    public Button homeBtn;
    public Button goldAdsBtn;
    public Button continueBtn;
    Animator animator;
    int goldWin = 40;

    [Header("Win Popup Elements")]
    public GameObject featureName;
    public Canvas featureUnlockCanvas;
    public LayoutGroup popupLayoutGroup;
   

    public GainGoldFX gainGoldVfx;
    public ParticleSystem victoryVfx;
    public ParticleSystem unlockVFX;
    public Image backgroundProgress;
    public Image progress;
    public TMP_Text percentageText;
    public TMP_Text featureNameText;
    public TMP_Text currentLevelText;
    public TMP_Text goldRewardText;


    private void Awake()
    {
        controller = GetComponent<UIAnimationController>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        GameLevelManager.instance.onGameLevelSuccess += OpenWinPopup;
        //
        homeBtn.onClick.AddListener(delegate {
            GameLevelManager.instance.ReturnHomeMenu();
        });

        goldAdsBtn.onClick.AddListener(delegate
        {
            Invoke(nameof(GainAdsGold), 0.2f);
            goldAdsBtn.gameObject.SetActive(false);
        });

        continueBtn.onClick.AddListener(delegate {
            GameLevelManager.instance.StartGame();
        });
    }

    public void GainAdsGold()
    {
        gainGoldVfx.GainGold(goldWin);
        goldRewardText.text = $"+{(goldWin * 2)}";
    }    

    public void OpenWinPopup(LevelResult result)
    {
        victoryVfx.startDelay = 1f;
        victoryVfx.Play();

        goldRewardText.text = $"+{goldWin}";

        //update win UI
        UnlockFeatureLevelMetadata unlockFeatureLevelMetadata = GameLevelManager.instance.GetNearestUnlockFeatureLevel();
        Debug.Log(unlockFeatureLevelMetadata);

        float targetPercentage = 0;
        float currentProgress = GetCurrentPercentage();
        currentLevelText.text = $"Level {GameLevelManager.instance.currentLevel}";

        if (unlockFeatureLevelMetadata != null)
        {
            targetPercentage = GameLevelManager.instance.GetCurrentPercentage();
            if (currentProgress > targetPercentage) {
                currentProgress = 0;
            }
            UpdateCurrentPercentage(targetPercentage);

            progress.sprite = unlockFeatureLevelMetadata.featureIcon;
            backgroundProgress.sprite = unlockFeatureLevelMetadata.featureIcon;
            featureNameText.text = unlockFeatureLevelMetadata.featureName;
            
            
            progress.fillAmount = currentProgress;
            percentageText.text = $"{(int)(currentProgress * 100)}%";

        }
        else
        {
            featureUnlockCanvas.sortingOrder = -1;
        }

        popupLayoutGroup.SetLayoutVertical();

        goldAdsBtn.gameObject.SetActive(true);

        controller.OnActivate += () =>
        {
            unlockVFX.Play();
            StartCoroutine(ProgressAnimation(currentProgress, targetPercentage));
        };

        //Invoke(nameof(ActivatePopup), 1.8f);
        Invoke(nameof(ActivatePopup), 0.8f);
    }

    public float GetCurrentPercentage()
    {
        if(PlayerPrefs.HasKey("feature_progress"))
        {
            float value = PlayerPrefs.GetFloat("feature_progress");
            return (value >= 1) ? 0 : value;

        } else
        {
            UpdateCurrentPercentage(0);
            return 0;
        }
    }

    public void UpdateCurrentPercentage(float f)
    {
        PlayerPrefs.SetFloat("feature_progress", f);
        PlayerPrefs.Save();
    }

    public void ActivatePopup()
    {
        //open popup
        controller.Activate();
    }

    public void ShowPopup()
    {
        controller.isActive = true;
        controller.UpdateCanvasGroup();
    }

    public void HidePopup()
    {
        controller.isActive = false;
        controller.UpdateCanvasGroup();
    }

    private void OnDestroy()
    {
        GameLevelManager.instance.onGameLevelSuccess -= OpenWinPopup;
    }

    IEnumerator ProgressAnimation(float currentProgress, float targetPercentage)
    {
        yield return new WaitForSeconds(0.5f);
        gainGoldVfx.GainGold(goldWin);
        yield return new WaitForSeconds(0.5f);
        
        //percentage fx
        float currentPercentage = currentProgress;
        float targetLerpTime = 1;
        float lerpTime = targetLerpTime;
        while(currentPercentage < targetPercentage)
        {
            lerpTime -= Time.deltaTime;
            currentPercentage = Mathf.Lerp(currentProgress, targetPercentage, 1 - (lerpTime / targetLerpTime));
            progress.fillAmount = currentPercentage;
            percentageText.text = $"{(int)(currentPercentage * 100)}%";
            yield return new WaitForEndOfFrame();
        }
        //
        progress.fillAmount = targetPercentage;
        percentageText.text = $"{(int)(targetPercentage * 100)}%";

        //unlock fx
        if (targetPercentage < 1) yield break;
        animator.Play("featureUnlockAnimation");
        
        //play sound
    }
}
