using KienChi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePanelWithHold : MonoBehaviour
{
    public RectTransform hole;
    public RectTransform panelTop;
    public RectTransform panelBottom;
    public RectTransform panelLeft;
    public RectTransform panelRight;
    public RectTransform mainPanel;

    // Update is called once per frame
    void Update()
    {
        UpdatePanel();
    }

    private void OnDrawGizmos()
    {
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        float topY = (mainPanel.rect.height + hole.rect.height) / 2;
        float bottomY = mainPanel.rect.height - topY;

        float leftY = (mainPanel.rect.width + hole.rect.width) / 2;


        panelTop.offsetMin = new Vector2(0, topY + hole.localPosition.y);

        panelBottom.offsetMax = new Vector2(0, -topY + hole.localPosition.y);

        panelLeft.offsetMin = new Vector2(0, bottomY + hole.localPosition.y);
        panelLeft.offsetMax = new Vector2(-leftY + hole.localPosition.x, -bottomY + hole.localPosition.y);

        panelRight.offsetMin = new Vector2(leftY + hole.localPosition.x, bottomY + hole.localPosition.y);
        panelRight.offsetMax = new Vector2(0, -bottomY + hole.localPosition.y);
    }


}
