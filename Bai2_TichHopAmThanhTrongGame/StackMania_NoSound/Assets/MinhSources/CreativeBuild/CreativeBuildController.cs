using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreativeBuildController : MonoBehaviour
{
    public GameObject visualizeTool;
    [Header("Cursor Size")]
    public Toggle toggleCursor;
    public Slider sliderCursor;
    public Transform cursorHolder;
    float cursorSize = 2;
    //
    [Header("Cursor Image")]
    public Sprite normalHand;
    public Sprite clickHand;
    public Image cursorImage;
    public Transform cursor;
    [Header("Ads")]
    public Toggle toggleAds;

    [Header("UI")]
    bool isToggleUI = false;
    public Toggle toggleUI;
    GameObject[] uis;

    [Header("Change Gold")]
    public TMP_InputField goldInput;
    public Button applyGoldButton;

    [Header("Change Level")]
    public TMP_InputField levelInput;
    public Button applyLevelButton;
    public Button nextLevelBtn;
    public Button prevLevelBtn;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        InitAdsToggle();
        InitUIToggle();
        InitToggleCursor();
        InitSliderCursor();
        InitGoldChange();
        InitLevelChange();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.P))
        {
            visualizeTool.SetActive(!visualizeTool.activeSelf);
        }

        if (!toggleCursor.isOn) return;
        cursor.position = Input.mousePosition;

        if(Input.GetMouseButtonDown(0))
        {
            ChangeCursor(clickHand);
        } else if(Input.GetMouseButtonUp(0))
        {
            ChangeCursor(normalHand);
        }
    }

    public void InitAdsToggle()
    {
        toggleAds.onValueChanged.AddListener((value) =>
        {

        });
        //
        toggleAds.isOn = false;
    }

    public void InitGoldChange()
    {
        applyGoldButton.onClick.AddListener(delegate
        {
            int gold = 0;
            bool parseSuccess = int.TryParse(goldInput.text, out gold);
            
            if(parseSuccess) PlayerResourceController.instance.GainGold(gold);
        });
    }

    public void InitLevelChange()
    {
        applyLevelButton.onClick.AddListener(delegate
        {
            int level = 0;
            bool parseSuccess = int.TryParse(levelInput.text, out level);

            if (parseSuccess && level > 0)
            {
                levelInput.text = "";
                PlayerPrefs.SetInt("level", level);
                PlayerPrefs.Save();
                GameLevelManager.instance.currentLevel = level;
                GameLevelManager.instance.StartGame();
            }
        });

        nextLevelBtn.onClick.AddListener(delegate
        {
            GameLevelManager.instance.currentLevel++;
            PlayerPrefs.SetInt("level", GameLevelManager.instance.currentLevel);
            PlayerPrefs.Save();
            GameLevelManager.instance.StartGame();
        });

        prevLevelBtn.onClick.AddListener(delegate
        {
            if (GameLevelManager.instance.currentLevel <= 1) return;
            GameLevelManager.instance.currentLevel--;
            PlayerPrefs.SetInt("level", GameLevelManager.instance.currentLevel);
            PlayerPrefs.Save();
            GameLevelManager.instance.StartGame();
        });
    }

    

    #region Handle Toggle UI
    public void InitUIToggle()
    {
        isToggleUI = toggleUI.isOn;
        toggleUI.onValueChanged.AddListener((value) =>
        {
            isToggleUI = value;
            ToggleUI(!value);
        });

        SceneManager.sceneLoaded += (scene, mode) =>
        {
            ToggleUI(!isToggleUI);
        };
    }

    public void ToggleUI(bool toggle)
    {
        if(!toggle)
        {
            uis = GameObject.FindGameObjectsWithTag("MainGameUI");
        }

        if (uis == null) return;

        foreach (GameObject go in uis)
        {
            if(go) go.SetActive(toggle);
        }
    }

    #endregion

    public void InitSliderCursor()
    {
        sliderCursor.onValueChanged.AddListener((value) =>
        {
            cursorHolder.localScale = (1 + value * cursorSize) * Vector3.one;
        });

        sliderCursor.value = 0;

    }

    public void InitToggleCursor()
    {
        ToggleCursor(toggleCursor.isOn);
        toggleCursor.onValueChanged.AddListener((value) =>
        {
            ToggleCursor(value);
        });

    }

    public void ToggleCursor(bool toggle)
    {
        Cursor.visible = !toggle;
        cursorImage.enabled = toggle;
        ChangeCursor(normalHand);
    }

    public void ChangeCursor(Sprite texture)
    {
        cursorImage.sprite = texture;
    }    
}
