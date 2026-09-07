using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : BaseObject
{
    [SerializeField] private TextMeshProUGUI levelName;
    [SerializeField] private Image progress;
    public static UIManager INSTANCE;
    // Start is called before the first frame update
    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void onLevelChange(int index)
    {
        levelName.text = "Level " + (index + 1);
    }
    public void onLevelProgress(int current, int total)
    {
        progress.fillAmount = (float) current / (float) total;
    }
}
