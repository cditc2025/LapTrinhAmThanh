using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationPageController : MonoBehaviour
{
    public GameObject navButtonContainer;
    public GameObject navPageContainer;
    public int selectedPage = 1;
    public float scrollSpeed = 5f;
    public Scrollbar shopScrollbar;
    bool isScrolling = false;

    // Start is called before the first frame update
    void Start()
    {
        if(navButtonContainer.transform.childCount != navPageContainer.transform.childCount)
        {
            Debug.LogError("Number of navigation button and navigation page is not equal!");
        } else
        {
            BindNavButtonEvent();
            HandleNavButtonAnimation(selectedPage);
            //Invoke(nameof(Init), 0.1f);
        }
    }

    public void Init()
    {
        HandleNavButtonAnimation(selectedPage);
        //HandleScrollPageAnimation(selectedPage);
    }

    public void BindNavButtonEvent()
    {
        for(int i = 0; i < navButtonContainer.transform.childCount; i ++)
        {
            Button btn = navButtonContainer.transform.GetChild(i).GetComponent<Button>();
            Animator btnAnimator = btn.GetComponent<Animator>();
            int index = i;
            btn.onClick.AddListener(delegate
            {
                ScrollToPage(index);
            });
        }
    }

    public void ScrollToPage(int index)
    {
        if (!isScrolling)
        {
            shopScrollbar.value = 1;
            //
            selectedPage = index;
            HandleNavButtonAnimation(selectedPage);
            HandleScrollPageAnimation(selectedPage);
        }
    }

    public void NavigateToGoldShop()
    {
        ScrollToPage(0);
        shopScrollbar.value = 0;
    }

    public void HandleScrollPageAnimation(int selectedIndex)
    {
        float totalWidth = navPageContainer.GetComponent<RectTransform>().sizeDelta.x;
        isScrolling = true;
        StartCoroutine(ScrollPage(- totalWidth * selectedIndex / navPageContainer.transform.childCount));
    }

    IEnumerator ScrollPage(float targetPosX)
    {
        RectTransform pageContainerTransform = navPageContainer.GetComponent<RectTransform>();
        Vector2 targetPos = new Vector2(targetPosX, 0);
        while ( Vector2.Distance( pageContainerTransform.anchoredPosition, targetPos) > 10f)
        {
            pageContainerTransform.anchoredPosition = Vector2.Lerp(pageContainerTransform.anchoredPosition, targetPos, scrollSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        pageContainerTransform.anchoredPosition = targetPos;
        isScrolling = false;
        yield return null;
    }

    public void HandleNavButtonAnimation(int selectedIndex)
    {
        for (int i = 0; i < navButtonContainer.transform.childCount; i++)
        {
            Animator btnAnimator = navButtonContainer.transform.GetChild(i).GetComponent<Animator>();
            if(i == selectedIndex)
            {
                btnAnimator.SetBool("isSelected", true);
            } else
            {
                btnAnimator.SetBool("isSelected", false);
            }
        }
    }
}
