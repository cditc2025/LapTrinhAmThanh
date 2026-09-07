using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoosterTutoriorPopupController : MonoBehaviour
{
    public UIAnimationController controller;
    public TutoriorController tutoriorController;
    UnlockBoosterLevelMetadata unlockMetadata;
    [Header("Popup settings")]
    public TMP_Text boosterName;
    public Image boosterIcon;
    public Image boosterTutorior;
    public TMP_Text boosterDescription;
    public Button claimBtn;

    

    void OnValidate()
    {
        controller = GetComponent<UIAnimationController>();
        tutoriorController = FindObjectOfType<TutoriorController>();
    }

    public void SetupAndOpenPopup(UnlockBoosterLevelMetadata metadata, UnityAction unlockBooster)
    {

        if (tutoriorController.OnPlayBoosterTutorior != null) return;

        if(PlayerPrefs.HasKey(metadata.tutorior_id))
        {
            if (PlayerPrefs.GetInt(metadata.tutorior_id) == 1)
            {
                unlockBooster?.Invoke();
                return;
            }
        } 

        unlockMetadata = metadata;
        //setup booster info
        boosterName.text = unlockMetadata.boosterName;
        boosterIcon.sprite = unlockMetadata.boosterIcon;
        boosterTutorior.sprite = unlockMetadata.boosterTutoriorImage;
        boosterDescription.text = unlockMetadata.boosterDescription;
        //setup claim btn event
        claimBtn.onClick.AddListener(delegate
        {
            ClaimBooster();
            unlockBooster?.Invoke();
        });

        //setup listener
        tutoriorController.OnPlayBoosterTutorior += OpenBoosterTutorior;
        controller.OnDeactivate += () => {
            tutoriorController.OnBoosterTutoriorEnd?.Invoke();
        };
    }

    public void OpenBoosterTutorior()
    {
        //open popup
        controller.Activate();
    }

    public void ClaimBooster()
    {
        PlayerResourceController.instance.GainBooster(unlockMetadata.givedBoosterAmount, unlockMetadata.newBooster);
        PlayerPrefs.SetInt(unlockMetadata.tutorior_id, 1);
        PlayerPrefs.Save();
        controller.Deactivate();
    }
}
