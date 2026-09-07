using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MButton : BaseObject, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool autoSound = true;
    [SerializeField] private string specialSound = "";
    [SerializeField] private bool negativeSound = false;
    [SerializeField] private bool scaleWhenTouch = false;

    private Vector3 currentScale = new Vector3(1, 1, 1);

    public void OnPointerDown(PointerEventData eventData)
    {
        if (scaleWhenTouch)
        {
            GetComponent<ScaleComponent>().scaleTo(currentScale * 1.15f, 0.15f, OwnAnimationFormular.EASE_OUTCUBIC);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (scaleWhenTouch)
        {
            GetComponent<ScaleComponent>().scaleTo(currentScale * 1.0f, 0.15f, OwnAnimationFormular.EASE_OUTCUBIC);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (scaleWhenTouch)
        {
            gameObject.AddComponent<ScaleComponent>();
        }
        currentScale = transform.localScale;
        GetComponent<Button>().onClick.AddListener(() => {
            if (autoSound)
            {
                Debug.Log("MButton sound");
                if (specialSound.Length > 0)
                    AudioManager.INSTANCE.playAudio(specialSound);
                else
                    AudioManager.INSTANCE.playAudio(negativeSound ? "click_normal" : "click_normal");
            }
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

}
