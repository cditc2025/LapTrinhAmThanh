using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentLevelText : MonoBehaviour
{
    TMP_Text levelText;
    // Start is called before the first frame update
    void Start()
    {
        levelText = GetComponent<TMP_Text>();
        levelText.text = $"Level {GameLevelManager.instance.currentLevel}";
    }

}
