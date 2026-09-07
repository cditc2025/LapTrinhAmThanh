using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopupManager : BaseObject
{
    [SerializeField] private GameObject popupSetting;
    [SerializeField] private GameObject popupWin;
    [SerializeField] private GameObject popupLose;
    [SerializeField] private Image unlockProgress;
    [SerializeField] private TextMeshProUGUI unlockProgressText;

    public static PopupManager INSTANCE;
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void showSetting()
    {
        popupSetting.SetActive(true);
    }
    public void showWin(int unlockPercent)
    {
        unlockProgress.fillAmount = (float) unlockPercent / 100.0f;
        unlockProgressText.text = "" + unlockPercent + "%";
        popupWin.SetActive(true);
    }
    public void showLose(GameObject slot)
    {
        float y = Camera.main.WorldToScreenPoint(slot.transform.position).y * 1024.0f / Screen.width;
        RectTransform rectTransform = popupLose.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
        popupLose.SetActive(true);
    }
    public bool isShowingPopup()
    {
        return popupSetting.activeSelf || popupWin.activeSelf || popupLose.activeSelf;
    }
}
