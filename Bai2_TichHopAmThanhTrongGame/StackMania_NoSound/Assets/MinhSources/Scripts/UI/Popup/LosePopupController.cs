using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LosePopupController : MonoBehaviour
{
    UIAnimationController controller;

    public Button homeBtn;
    public Button retryBtn;

    private void Awake()
    {
        controller = GetComponent<UIAnimationController>();
    }

    private void Start()
    {
        //
        homeBtn.onClick.AddListener(delegate
        {
            GameLevelManager.instance.ReturnHomeMenu();
        });

        retryBtn.onClick.AddListener(delegate
        {
            GameLevelManager.instance.StartGame();
        });
    }

    public void OpenPopup()
    {
        PlayerResourceController.instance.LostLife();
        retryBtn.gameObject.SetActive(PlayerResourceController.instance.life > 0);
        StartCoroutine(OpenPopupAfterDelay(0.2f));
    }

    IEnumerator OpenPopupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        controller.Activate();
        //
    }

    public void ClosePopup()
    {
        controller.Deactivate();
    }
}
