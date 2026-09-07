using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KienChi;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameLevelManager : MonoBehaviour
{
    public static GameLevelManager instance;
    [Header("Level Settings")]
    public int currentLevel = 1;
    public UnityAction<int> onCurrentLevelChanged;

    [Header("Game State Settings")]
    public bool isPlaying;
    public UnityAction<LevelResult> onGameLevelFail;
    public UnityAction<LevelResult> onGameLevelSuccess;

    public UnityAction onLifeNotEnough;
    public SortedDictionary<int, UnlockFeatureLevelMetadata> unlockFeatureLevels;
    public SortedDictionary<int, UnlockBoosterLevelMetadata> unlockBoosterLevels;

    public LevelResult result;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            Init();
        } else
        {
            Destroy(this);
        }
    }

    public int CalculateLevelIndex(int level, int totalLevel)
    {
        int startLoopLevel = (level > totalLevel) ? 20 : 0;
        int levelIndex = level - 1;
        levelIndex = startLoopLevel + (levelIndex - startLoopLevel) % (totalLevel - startLoopLevel);

        return levelIndex;
    }

    void Init()
    {
        //set target framerate
        Application.targetFrameRate = 60;
        //PlayerPrefs.DeleteAll();
        //PlayerPrefs.SetInt("level", 80);
        //PlayerPrefs.Save();
#if UNITY_EDITOR
        Debug.unityLogger.logEnabled = true;
#else
        Debug.unityLogger.logEnabled = true;
#endif
        //get current level
        if (!PlayerPrefs.HasKey("level"))
        {
            PlayerPrefs.SetInt("level", 1);
            PlayerPrefs.Save();
            currentLevel = 1;
        }
        else
        {
            currentLevel = PlayerPrefs.GetInt("level");  
        }

        //create unlock dictionary
        unlockFeatureLevels = CreateUnlockFeatureMap();
        unlockBoosterLevels = CreateUnlockBoosterMap();
        //ResetTutorior();
        //init event
        SceneManager.sceneLoaded += OnSceneLoad;
        onGameLevelSuccess += OnGameLevelSuccess;
        onGameLevelFail += OnGameLevelFail;
    }

    public void ResetTutorior()
    {
        PlayerPrefs.SetInt("first_time_play", 0);
        foreach(UnlockBoosterLevelMetadata metadata in unlockBoosterLevels.Values)
        {
            PlayerPrefs.SetInt(metadata.tutorior_id, 0);
        }
        foreach (UnlockFeatureLevelMetadata metadata in unlockFeatureLevels.Values)
        {
            PlayerPrefs.SetInt(metadata.tutorior_id, 0);
        }
        PlayerPrefs.Save();
    }

    public void OnGameLevelSuccess(LevelResult levelResult)
    {
        //StartCoroutine(ShowAdsOnEndSession(1.5f));
    }

    public IEnumerator ShowAdsOnEndSession(float delay)
    {
        yield return new WaitForSeconds(delay);

    }

    public void OnGameLevelFail(LevelResult levelResult)
    {
        float levelProgress = GetLevelProgress();

        //
        UpdateResultPref();
        StartCoroutine(ShowAdsOnEndSession(1.5f));
    }    

    public int GetCurrentLevel()
    {
        currentLevel = PlayerPrefs.GetInt("level");
        return currentLevel;
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        if(scene.buildIndex == 0)
        {
            if((PlayerPrefs.HasKey("first_time_play") && PlayerPrefs.GetInt("first_time_play") == 1) || currentLevel != 1)
            {
                SceneManager.LoadScene("HomeMenu");
            } else
            {
                isPlaying = true;
                result = GetLevelResult();
                SceneManager.LoadScene("GameScene");
            }
            
        } else
        {
            LoadingController.instance.CloseLoadingPanel();
           
        }
    }

#region Handle Game State
    public void StartGame()
    {
        if (PlayerResourceController.instance.life <= 0)
        {
            onLifeNotEnough?.Invoke();
            return;
        }
        isPlaying = true;
        LoadingController.instance.OpenLoadingPanel(() =>
        {
            result = GetLevelResult();
            if (result.level != currentLevel)
            {
                result = new LevelResult();
                result.level = currentLevel;

                UpdateResultPref();

            } else
            {
                UpdateRetryCount();
            }

            SceneManager.LoadScene("GameScene");
        });

    }

    public LevelResult GetLevelResult()
    {
        if(result == null)
        {
            if(PlayerPrefs.HasKey("current_level_result"))
            {
                string currentLevelResult = PlayerPrefs.GetString("current_level_result");
                result = LevelResult.ConvertToObject(currentLevelResult);
            } else
            {
                result = new LevelResult();
                result.level = currentLevel;
            }
        }
        return result;
    }

    public void RetryGame()
    {
        if(isPlaying)
        {
            //substract heart
            PlayerResourceController.instance.LostLife();
            //Restart game
            StartGame();
        }
    }

    public void UpdateRetryCount()
    {
        result.retry_count++;
        UpdateResultPref();
    }

    public void UpdateBoosterCount(Booster booster)
    {
        switch(booster)
        {
            case Booster.MIXUP:
                result.shuffle++;
                break;
            case Booster.PICKER:
                result.pointer++;
                break;
            case Booster.DYNAMITE:
                result.dynamite++;
                break;
        }
        result.booster_count++;
        UpdateResultPref();
    }

    public void UpdateReviveCount()
    {
        result.revive++;
        result.booster_count++;
        UpdateResultPref();
    }

    public void UpdateAddSlotCount()
    {
        result.slot_add++;
        result.booster_count++;
        UpdateResultPref();
    }

    public void UpdateResultPref()
    {
        PlayerPrefs.SetString("current_level_result", result.ToJSON());
        PlayerPrefs.Save();
    }

    public void QuitGameLevel()
    {
        float levelProgress = GetLevelProgress();

        //substract heart
        PlayerResourceController.instance.LostLife();
        //
        ReturnHomeMenu();
    }

    public void ReturnHomeMenu()
    {
        isPlaying = false;
        LoadingController.instance.OpenLoadingPanel(() =>
        {
            SceneManager.LoadScene("HomeMenu");
        });
    }

    public void IncreaseCurrentLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
        PlayerPrefs.Save();
        //
        onCurrentLevelChanged?.Invoke(currentLevel);
    }

    public float GetLevelProgress()
    {
        uint totalBlock = BlockManager.Instance.TotalBlock;
        uint currDetroyBlock = BlockManager.Instance.NumOfDestroyBlock;

        return (float)(currDetroyBlock * 100f / totalBlock);
    }    
#endregion

#region Handle Unlock Feature Level

    private SortedDictionary<int, UnlockFeatureLevelMetadata> CreateUnlockFeatureMap()
    {
        UnlockFeatureLevelMetadata[] unlockFeatures = Resources.LoadAll<UnlockFeatureLevelMetadata>("");

        // Create the quest map
        SortedDictionary<int, UnlockFeatureLevelMetadata> unlockFeaturesMap = new SortedDictionary<int, UnlockFeatureLevelMetadata>();
        foreach (UnlockFeatureLevelMetadata feature in unlockFeatures)
        {
            if (unlockFeaturesMap.ContainsKey(feature.level))
            {
                Debug.LogWarning("Duplicate ID found when creating Unlock feature map: " + feature.level);
            }
            unlockFeaturesMap.Add(feature.level, feature);
        }

        return unlockFeaturesMap;
    }

    public UnlockFeatureLevelMetadata GetUnlockFeatureOfCurrentLevel()
    {
        if(unlockFeatureLevels.ContainsKey(currentLevel))
        {
            return unlockFeatureLevels[currentLevel];
        } else
        {
            return null;
        }
    }    

    public UnlockFeatureLevelMetadata GetNearestUnlockFeatureLevel()
    {
        for(int i = 0; i < unlockFeatureLevels.Count; i ++)
        {
            UnlockFeatureLevelMetadata data = unlockFeatureLevels.ElementAt(i).Value;
            if(data.level > currentLevel)
            {
                return data;
            }
        }

        return null;
    }

    public float GetCurrentPercentage()
    {
        float previousLevel = 0;
        float targetLevel = 0;
        for (int i = 0; i < unlockFeatureLevels.Count; i++)
        {
            UnlockFeatureLevelMetadata data = unlockFeatureLevels.ElementAt(i).Value;
            if (data.level > currentLevel)
            {
                targetLevel = data.level - 1;
                break;
            } else
            {
                previousLevel = data.level - 1;
            }
        }

        return (currentLevel - previousLevel) / (targetLevel - previousLevel);
    }

#endregion

#region Handle Unlock Booster Level
    private SortedDictionary<int, UnlockBoosterLevelMetadata> CreateUnlockBoosterMap()
    {
        UnlockBoosterLevelMetadata[] unlockBoosters = Resources.LoadAll<UnlockBoosterLevelMetadata>("");

        // Create the quest map
        SortedDictionary<int, UnlockBoosterLevelMetadata> unlockBoostersMap = new SortedDictionary<int, UnlockBoosterLevelMetadata>();
        foreach (UnlockBoosterLevelMetadata booster in unlockBoosters)
        {
            if (unlockBoostersMap.ContainsKey(booster.level))
            {
                Debug.LogWarning("Duplicate ID found when creating Unlock booster map: " + booster.level);
            }
            unlockBoostersMap.Add(booster.level, booster);
        }

        return unlockBoostersMap;
    }

    public UnlockBoosterLevelMetadata CheckUnlockBoosterOfCurrentLevel()
    {
        if (unlockBoosterLevels.ContainsKey(currentLevel))
        {
            return unlockBoosterLevels[currentLevel];
        }
        else
        {
            return null;
        }
    }
#endregion
}
