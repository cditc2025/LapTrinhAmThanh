using System.Collections;
using System.Collections.Generic;
using KienChi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadLevelButtonUIController : MonoBehaviour
{
    public TMP_Text levelText;
    Button loadLevelBtn;
    Animator animator;

    [Header("Difficulty Material")]
    public Renderer floor;
    public Material normalMaterial;
    public Material difficultMaterial;
    public Material brutalMaterial;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        loadLevelBtn = GetComponent<Button>();
        loadLevelBtn.onClick.AddListener(delegate
        {
            GameLevelManager.instance.StartGame();
        });

        //

        UpdateButtonStyle(GameLevelManager.instance.GetCurrentLevel());
    }

    public void UpdateButtonStyle(int level)
    {
        TextAsset[] allTextAsset = Resources.LoadAll<TextAsset>($"Data");
        // Debug.Log($"Data/{Consts.FILE_PREFIX}{_currentLevel}{Consts.FILE_SUFFIX}");
        // string encryptedText = textAsset.text;
        // Debug.Log(textAsset);

        string encryptedBase64 = allTextAsset[GameLevelManager.instance.CalculateLevelIndex(level, allTextAsset.Length)].text;
        string decrypted = AESCrypto.Decrypt(encryptedBase64);
        DataLevel dataLevel = JsonUtility.FromJson<DataLevel>(decrypted);
        //
        Difficulty difficulty = LevelManager.Instance.DifficultConvert(dataLevel.level.difficulty);
        //
        switch (difficulty)
        {
            case Difficulty.NORMAL:
                floor.material = normalMaterial;
                break;
            case Difficulty.DIFFICULT:
                floor.material = difficultMaterial;
                break;
            case Difficulty.BRUTAL:
                floor.material = brutalMaterial;
                break;
        }
        //
        animator.Play(difficulty.ToString());
        //
        levelText.text = $"Level {level}";
    }
}
