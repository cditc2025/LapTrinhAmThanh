using System.Collections;
using System.Collections.Generic;
using KienChi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UseBoosterBtn : MonoBehaviour
{
    Button boosterBtn;
    public bool isUnlock = false;
    public UnlockBoosterLevelMetadata metadata;

    [Header("Lock State UI Component")]
    public GameObject lockStateContent;
    public TMP_Text levelRequire;

    [Header("Unlock State UI Component")]
    public GameObject unlockStateContent;
    public Image boosterIcon;
    public TMP_Text boosterCount;
    public GameObject addIcon;
    public ParticleSystem gainBoosterFX;
    public BoosterTutoriorPopupController tutoriorController;
    public BuyBoosterPopup buyBoosterPopup;

    private void Awake()
    {
        boosterBtn = GetComponent<Button>();
    }

    private void Start()
    {
        
        //
        InitButton();
        if (GameLevelManager.instance.currentLevel == metadata.level)
        {
            tutoriorController.SetupAndOpenPopup(metadata, delegate
            {
                isUnlock = true;
                UpdateBoosterState();
            });
        }

        if (GameLevelManager.instance.currentLevel > metadata.level)
        {
            isUnlock = true;
            UpdateBoosterState();
        }
    }

    private void InitButton()
    {
        boosterCount.text = PlayerResourceController.instance.GetBoosterCount(metadata.newBooster).ToString();
        //
        boosterBtn.onClick.AddListener(delegate
        {
            
            //open buy popup
            if (PlayerResourceController.instance.GetBoosterCount(metadata.newBooster) <= 0)
            {
                buyBoosterPopup.OpenPopup(metadata);
                return;
            }

            //using booster
            BoosterManager boosterManager = BoosterManager.Instance;
            switch (metadata.newBooster)
            {
                case Booster.MIXUP:
                    // PlayerResourceController.instance.UseBooster(Booster.MIXUP);
                    boosterManager.ShuffleShooterBooster(() =>
                    {
                        PlayerResourceController.instance.UseBooster(metadata.newBooster);
                    });
                    break;
                case Booster.PICKER:
                    boosterManager.PickShooterBooster(() =>
                    {
                        PlayerResourceController.instance.UseBooster(metadata.newBooster);
                    });
                    break;
                case Booster.DYNAMITE:
                    boosterManager.BreakBlockBooster(() =>
                    {
                        PlayerResourceController.instance.UseBooster(metadata.newBooster);
                    });
                    break;
            }
            //PlayerResourceController.instance.UseBooster(metadata.newBooster);
        });
        //
        AddBoosterCountChangeListener();
    }

    public void OnBoosterCountChanged(int count, bool isGained)
    {
        if (isGained)
        {
            int currentCount = int.Parse(boosterCount.text);
            var burst = gainBoosterFX.emission.GetBurst(0);

            burst.cycleCount = count - currentCount;
            gainBoosterFX.emission.SetBurst(0, burst);
            gainBoosterFX.Play();
        }
        addIcon.SetActive(count <= 0);
        boosterCount.text = count.ToString();
    }

    private void AddBoosterCountChangeListener()
    {
        switch (metadata.newBooster)
        {
            case Booster.MIXUP:
                PlayerResourceController.instance.OnMixUpBoosterCountChanged += OnBoosterCountChanged;
                break;
            case Booster.PICKER:
                PlayerResourceController.instance.OnPickerBoosterCountChanged += OnBoosterCountChanged;
                break;
            case Booster.DYNAMITE:
                PlayerResourceController.instance.OnDynamiteBoosterCountChanged += OnBoosterCountChanged;
                break;
        }
    }

    private void RemoveBoosterCountChangeListener()
    {
        switch (metadata.newBooster)
        {
            case Booster.MIXUP:
                PlayerResourceController.instance.OnMixUpBoosterCountChanged -= OnBoosterCountChanged;
                break;
            case Booster.PICKER:
                PlayerResourceController.instance.OnPickerBoosterCountChanged -= OnBoosterCountChanged;
                break;
            case Booster.DYNAMITE:
                PlayerResourceController.instance.OnDynamiteBoosterCountChanged -= OnBoosterCountChanged;
                break;
        }
    }

    private void UpdateBoosterState()
    {
        lockStateContent.SetActive(!isUnlock);
        unlockStateContent.SetActive(isUnlock);

        if(boosterBtn == null)
        {
            boosterBtn = GetComponent<Button>();
        }
        boosterBtn.interactable = isUnlock;
        //
        if(PlayerResourceController.instance && metadata)
        {
            OnBoosterCountChanged(PlayerResourceController.instance.GetBoosterCount(metadata.newBooster), false);
        }
        
    }

    private void OnValidate()
    {
        UpdateBoosterState();
        if (metadata)
        {
            levelRequire.text = metadata.level.ToString();
            boosterIcon.sprite = metadata.boosterIcon;
        }
        if (tutoriorController == null) tutoriorController = FindObjectOfType<BoosterTutoriorPopupController>();
        if (buyBoosterPopup == null) buyBoosterPopup = FindObjectOfType<BuyBoosterPopup>();
    }

    private void OnDestroy()
    {
        RemoveBoosterCountChangeListener();
    }
}
