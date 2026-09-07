using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    public static PopupController instance;
    public UIAnimationController pausePopup;
    
    public void OpenPausePopup()
    {
        pausePopup.Activate();
    }

    public void ClosePausePopup()
    {
        pausePopup.Deactivate();
    }
}
