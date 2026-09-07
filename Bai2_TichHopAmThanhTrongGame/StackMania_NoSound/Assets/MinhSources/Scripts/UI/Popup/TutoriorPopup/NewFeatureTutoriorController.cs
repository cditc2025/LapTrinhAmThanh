using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KienChi;
using UnityEngine;
using UnityEngine.EventSystems;

public class NewFeatureTutoriorController : MonoBehaviour
{
    public NewFeature newFeature;
    public bool needToTap = false;
    UIAnimationController controller;
    TutoriorController tutoriorController;
    bool isShowing = false;

    string tutorior_id = "";
    void Awake()
    {
        controller = GetComponent<UIAnimationController>();
        tutoriorController = FindObjectOfType<TutoriorController>();
        SetupNewFeatureTutorior();
    }

    // Update is called once per frame
    void Update()
    {
        //if(!UIEvent.instance.IsPointerOverUIObject())
        //{
        //    Debug.Log("can click");
        //}
        if (!isShowing || !needToTap) return;

        Vector3 worldPos = ShooterManager.Instance.GetTutoriorShooterPos();
        worldPos.y += 0.5f;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = screenPos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        // Debug.Log(results.Count);
        //else return false
        if (results.Count != 0)
        {
            GetComponentInChildren<CreatePanelWithHold>().hole.transform.position = results[0].worldPosition;
        }
    }

    public void ClosePopup()
    {
        controller.Deactivate();
        PlayerPrefs.SetInt(tutorior_id, 1);
        PlayerPrefs.Save();
        isShowing = false;
    }

    public void SetupNewFeatureTutorior()
    {
        if(GameLevelManager.instance.currentLevel == 1 && newFeature == NewFeature.FIRST_TIME)
        {
            tutorior_id = "first_time_play";
            //
            if (PlayerPrefs.HasKey(tutorior_id))
            {
                if (PlayerPrefs.GetInt(tutorior_id) == 1) return;
            }
            
            tutoriorController.OnPlayNewFeatureTutorior = null;
            tutoriorController.OnPlayNewFeatureTutorior += PlayNewFeatureTutorior;
            return;
        }

        UnlockFeatureLevelMetadata metadata = GameLevelManager.instance.GetUnlockFeatureOfCurrentLevel();
    
        if (tutoriorController.OnPlayNewFeatureTutorior != null || metadata == null || newFeature != metadata.newFeature) return;

        tutorior_id = metadata.tutorior_id;
        if (PlayerPrefs.HasKey(tutorior_id))
        {
            if (PlayerPrefs.GetInt(tutorior_id) == 1) return;
        } 

        if (newFeature == metadata.newFeature)
        {
            tutoriorController.OnPlayNewFeatureTutorior += PlayNewFeatureTutorior;
        }
    }

    public void PlayNewFeatureTutorior()
    {
        controller.Activate();
        isShowing = true;
        controller.OnDeactivate += tutoriorController.OnNewFeatureTutoriorEnd;
    }
}
