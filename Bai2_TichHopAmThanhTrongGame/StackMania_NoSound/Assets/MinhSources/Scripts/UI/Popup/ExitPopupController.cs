using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitPopupController : MonoBehaviour
{
    UIAnimationController controller;

    [Header("Button")]
    public Button confirmBtn;
    public Button closeBtn;

    private void Start()
    {
        controller = GetComponent<UIAnimationController>();
        //
        confirmBtn.onClick.AddListener(delegate
        {
            controller.Deactivate();
            GameLevelManager.instance.QuitGameLevel();
        });

        closeBtn.onClick.AddListener(delegate
        {
            controller.Deactivate();
        });
    }

    public void OpenPopup()
    {
        if(PlayerResourceController.instance.isInfiniteLife)
        {
            GameLevelManager.instance.ReturnHomeMenu();
            return;
        }

        StartCoroutine(OpenPopupAfterDelay(0f));
    }

    IEnumerator OpenPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        controller.Activate();
    }
}
