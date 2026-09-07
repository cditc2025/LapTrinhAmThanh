using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PausePopupController : MonoBehaviour
{
    public static PausePopupController instance;

    public UnityAction onGamePaused;
    public UnityAction onGameUnPaused;

    UIAnimationController controller;
    public Sprite onSprite;
    public Sprite offSprite;

    [Header("Sound Settings")]
    public Button toggleSoundBtn;
    Image toggleSoundBtnImage;
    public Button toggleMusicBtn;
    Image toggleMusicBtnImage;


    [Header("Haptic Settings")]
    public Button toggleHapticBtn;
    Image toggleHapticBtnImage;

    [Header("Others Buttons")]
    public Button homeBtn;
    public Button privacyBtn;

    // Start is called before the first frame update
    void Start()
    {
        //
        controller = GetComponent<UIAnimationController>();

        //get image
        toggleSoundBtnImage = toggleSoundBtn.GetComponent<Image>();
        toggleHapticBtnImage = toggleHapticBtn.GetComponent<Image>();
        toggleMusicBtnImage = toggleMusicBtn.GetComponent<Image>();

        //update sound and haptic settings
        InitSettings();
        InitButtonEventListener();

    }

    public void InitSettings()
    {
        if (!PlayerPrefs.HasKey("music_settings"))
        {
            PlayerPrefs.SetInt("music_settings", 1);
        }
        UpdateMusicSettings();

        if (!PlayerPrefs.HasKey("sound_settings"))
        {
            PlayerPrefs.SetInt("sound_settings", 1);
        }
        UpdateSoundSettings();

        if (!PlayerPrefs.HasKey("haptic_settings"))
        {
            PlayerPrefs.SetInt("haptic_settings", 1);
        }
        UpdateHapticSettings();
    }

    public void InitButtonEventListener()
    {
        //add button event listener
        toggleSoundBtn.onClick.AddListener(delegate { HandleToggleSound(); });

        toggleHapticBtn.onClick.AddListener(delegate { HandleToggleHaptic(); });

        toggleMusicBtn.onClick.AddListener(delegate { HandleToggleMusic(); });

        homeBtn.onClick.AddListener(delegate
        {
            ClosePopup();
        });

        if (privacyBtn)
        {
            privacyBtn.onClick.AddListener(delegate
            {
                //MaxSdk.ShowMediationDebugger();
            });
        }
    }

    public void OpenPopup()
    {
        bool isIngame = GameLevelManager.instance.isPlaying;

        homeBtn.gameObject.SetActive(isIngame && GameLevelManager.instance.currentLevel != 1);

        controller.Activate();
        onGamePaused?.Invoke();
    }

    public void ClosePopup()
    {
        controller.Deactivate();
        onGameUnPaused?.Invoke();
    }

    #region Handle Haptic Vibration

    public void HandleToggleHaptic()
    {
        int isHapticOn = PlayerPrefs.GetInt("haptic_settings");
        PlayerPrefs.SetInt("haptic_settings", (isHapticOn + 1) % 2);
        PlayerPrefs.Save();
        UpdateHapticSettings();
    }

    public void UpdateHapticSettings()
    {
        int isHapticOn = PlayerPrefs.GetInt("haptic_settings");
        if (isHapticOn == 1)
        {
            toggleHapticBtnImage.sprite = onSprite;
            //enable haptic
        }
        else
        {
            toggleHapticBtnImage.sprite = offSprite;
            //disable haptic
        }
    }

    #endregion

    #region Handle sound fx and music
    public void HandleToggleSound()
    {
        int isSoundOn = PlayerPrefs.GetInt("sound_settings");
        PlayerPrefs.SetInt("sound_settings", (isSoundOn + 1) % 2);
        PlayerPrefs.Save();
        UpdateSoundSettings();
    }

    public void UpdateSoundSettings()
    {
        int isSoundOn = PlayerPrefs.GetInt("sound_settings");
        if (isSoundOn == 1)
        {
            toggleSoundBtnImage.sprite = onSprite;
            //enable sound
     
        }
        else
        {
            toggleSoundBtnImage.sprite = offSprite;
            //disable sound

        }
    }

    public void HandleToggleMusic()
    {
        int isMusicOn = PlayerPrefs.GetInt("music_settings");
        PlayerPrefs.SetInt("music_settings", (isMusicOn + 1) % 2);
        PlayerPrefs.Save();
        UpdateMusicSettings();
    }

    public void UpdateMusicSettings()
    {
        int isMusicOn = PlayerPrefs.GetInt("music_settings");
        if (isMusicOn == 1)
        {
            toggleMusicBtnImage.sprite = onSprite;
            //enable sound
        }
        else
        {
            toggleMusicBtnImage.sprite = offSprite;
            //disable sound

        }
    }

    #endregion
}
