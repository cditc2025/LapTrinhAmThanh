using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImages : BaseObject
{
    [SerializeField] private List<Image> listImage;
    [SerializeField] private List<float> listAlpha;
    [SerializeField] private float duration;
    private float t;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        if (listAlpha.Count == 0)
            for (int i = 0; i < listImage.Count; i++)
            {
                listAlpha.Add(listImage[i].color.a);
            }
        for (int i = 0; i < listImage.Count; i++)
        {
            listImage[i].color = new Color(listImage[i].color.r, listImage[i].color.g, listImage[i].color.b, 0);
        }
        t = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (t < duration)
        {
            t += Time.deltaTime;
            if (t > duration)
            {
                t = duration;
            }
            for (int i = 0; i < listImage.Count; i++)
            {
                listImage[i].color = new Color(listImage[i].color.r, listImage[i].color.g, listImage[i].color.b, t / duration * listAlpha[i]);
            }
        }
    }
}
