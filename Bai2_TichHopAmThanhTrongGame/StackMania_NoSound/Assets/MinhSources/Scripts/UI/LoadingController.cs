using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingController : MonoBehaviour
{
    public static LoadingController instance;
    public Animator loadingAnimator;
    public UnityAction onFinishLoading;
    public UnityAction onLoadingOpened;
    public Transform progressBarCenter;
    public LayoutGroup group;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
       
    }

    private void Start()
    {
        OpenLoadingPanel(null);
        //CloseLoadingPanel();
    }

    public void OpenLoadingPanel(UnityAction action)
    {
        progressBarCenter.localScale = new Vector3(0, 1);
        group.transform.localScale = new Vector3(0, 1);
        //loadingAnimator.gameObject.SetActive(true);
        loadingAnimator.SetBool("isOn", true);
        onLoadingOpened += action;
    }

    public void OnLoadingOpened()
    {
        onLoadingOpened?.Invoke();
        onLoadingOpened = null;
        
    }

    public void CloseLoadingPanel()
    {
        StartCoroutine(CloseLoadingAfterDelay(0.75f));
        
    }

    IEnumerator CloseLoadingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(0.2f);
        float timer = 0;
        while (timer < delay)
        {
            timer += Time.deltaTime;
            float scaleVal = Mathf.Lerp(0, 1, timer / delay);
            progressBarCenter.localScale = new Vector3(scaleVal, 1, 1);
            group.transform.localScale = new Vector3(Mathf.Clamp01(scaleVal * 100f), 1, 1);
            group.SetLayoutHorizontal();
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(0.2f);
        loadingAnimator.SetBool("isOn", false);

        yield return new WaitForSeconds(0.5f);
        progressBarCenter.localScale = new Vector3(0, 1);
        group.SetLayoutHorizontal();
        //loadingAnimator.gameObject.SetActive(false);
        onFinishLoading?.Invoke();
        onFinishLoading = null;
    }
}
