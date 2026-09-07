using System.Collections;
using System.Collections.Generic;
using KienChi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelIntroController : MonoBehaviour
{
    Animator animator;
    public Material textNormalMaterial;
    public Material textDifficultMaterial;
    public Material textBrutalMaterial;
    
    [Header("Text Component")]
    public TMP_Text difficultText;
    public TMP_Text levelText;
    public TMP_Text chestCountText;

    [Header("Animation Settings")]
    public Transform iconSkull;
    public Transform mainCanvas;
    public Transform targetSkullPosition;
    public CanvasGroup introPopup;

    TutoriorController tutoriorController;
    Difficulty currentDifficulty = Difficulty.DIFFICULT;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        tutoriorController = FindObjectOfType<TutoriorController>();
        LevelManager.Instance.OnLoadLevelData += (data) => {
            UpdateUI(data);
            if(data == Difficulty.NORMAL)
            {

            } else
            {

            }
        };
    }

    private void Start()
    {
        tutoriorController.OnPlayLevelIntro += PlayLevelIntro;
        // UpdateUI(currentDifficulty);
    }

    private void OnDestroy()
    {
        tutoriorController.OnPlayLevelIntro -= PlayLevelIntro;
    }

    public void PlayLevelIntro()
    {
        StartCoroutine(PlayIntroAnimation());
    }    

    IEnumerator PlayIntroAnimation()
    {
        if(currentDifficulty == Difficulty.NORMAL)
        {
            tutoriorController.OnLevelIntroEnd?.Invoke();
            yield break;
        }


        iconSkull.SetParent(mainCanvas);
        iconSkull.SetAsLastSibling();
        yield return new WaitForSeconds(0.5f);

        float fadeTime = 0.5f;
        float time = fadeTime;
        while(time > 0)
        {
            time -= Time.deltaTime;
            introPopup.alpha = Mathf.Lerp(0, 1, time / fadeTime);
            yield return new WaitForEndOfFrame();
        }

        iconSkull.SetParent(targetSkullPosition);

        float flyTime = 0.5f;
        time = 0;
        Vector3 startPosition = iconSkull.localPosition;
        Vector3 startScale = iconSkull.localScale;
        while (time < flyTime)
        {
            time += Time.deltaTime;
            iconSkull.localPosition = Vector3.Lerp(startPosition, Vector3.zero, time / flyTime);
            iconSkull.localScale = Vector3.Lerp(startScale, startScale * 1.2f / 3, time / flyTime);
            yield return new WaitForEndOfFrame();
        }

        introPopup.blocksRaycasts = false;
        introPopup.interactable = false;
        tutoriorController.OnLevelIntroEnd?.Invoke();
    }

    public void UpdateUI(Difficulty level)
    {
        currentDifficulty = level;
        UpdateTextMaterial(level);
        animator.SetInteger("difficulty", (int)level);
    }

    public void UpdateTextMaterial(Difficulty level)
    {
        Material targetTextMaterial = textNormalMaterial;
        switch (level)
        {
            case Difficulty.NORMAL:
                targetTextMaterial = textNormalMaterial;
                break;

            case Difficulty.DIFFICULT:
                targetTextMaterial = textDifficultMaterial;
                difficultText.text = "Hard";
                break;

            case Difficulty.BRUTAL:
                targetTextMaterial = textBrutalMaterial;
                difficultText.text = "Brutal";
                break;
        }

        difficultText.fontSharedMaterial = targetTextMaterial;
        //levelText.fontSharedMaterial = targetTextMaterial;
        chestCountText.fontSharedMaterial = targetTextMaterial;
    }
}
