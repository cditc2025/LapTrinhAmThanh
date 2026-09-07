using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutoriorController : MonoBehaviour
{
    public UnityAction OnPlayLevelIntro;
    public UnityAction OnLevelIntroEnd;

    public UnityAction OnPlayBoosterTutorior;
    public UnityAction OnBoosterTutoriorEnd;

    public UnityAction OnPlayNewFeatureTutorior;
    public UnityAction OnNewFeatureTutoriorEnd;

    // Start is called before the first frame update
    void Start()
    {
        LoadingController.instance.onFinishLoading += PlayTutorior;
    }

    private void OnDestroy()
    {
        LoadingController.instance.onFinishLoading -= PlayTutorior;
    }

    public void PlayTutorior()
    {
        if(OnPlayBoosterTutorior != null)
        {
            OnLevelIntroEnd += OnPlayBoosterTutorior;

            if(OnPlayNewFeatureTutorior != null)
            {
                OnBoosterTutoriorEnd += OnPlayNewFeatureTutorior;
            }
        } else
        {
            if(OnPlayNewFeatureTutorior != null)
            {
                OnLevelIntroEnd += OnPlayNewFeatureTutorior;
            }
        }
        //
        OnPlayLevelIntro?.Invoke();
    }
}
