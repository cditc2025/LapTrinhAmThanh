using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerResourceController : MonoBehaviour
{
    public static PlayerResourceController instance;

    public UnityAction<int> OnGoldChanged;
    public UnityAction<int> OnLifeChanged;
    public UnityAction<bool> OnLifeInfiniteStateChanged;
    public UnityAction<double> OnLifeTimerChanged;
    //
    public UnityAction<int, bool> OnMixUpBoosterCountChanged;
    public UnityAction<int, bool> OnPickerBoosterCountChanged;
    public UnityAction<int, bool> OnDynamiteBoosterCountChanged;

    public int gold => PlayerPrefs.GetInt("gold_count");

    [Header("Life Settings")]
    double lifeTimer = 0f;
    public float regenLifeTime = 1800;
    public long regenLifeStart => long.Parse(PlayerPrefs.GetString("regen_life_start"));

    [HideInInspector] public bool isInfiniteLife = false;
    public int life => PlayerPrefs.GetInt("life_count");
    public int maxLifeCount = 5;
    public bool isFullLife => life == maxLifeCount;
    //in seconds
    public float lifeInfiniteDuration => PlayerPrefs.GetFloat("life_infinite_duration");
    public long lifeInfiniteStart => long.Parse(PlayerPrefs.GetString("life_infinite_start"));

    //Booster settings
    public int mixUpCount => PlayerPrefs.GetInt("mix_up_count");
    public int pickerCount => PlayerPrefs.GetInt("picker_count");
    public int dynamiteCount => PlayerPrefs.GetInt("dynamite_count");

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            InitPlayerPref();
            //SetLife(4);
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //Notify to all UI
        OnGoldChanged?.Invoke(gold);
        OnLifeChanged?.Invoke(life);
        OnLifeInfiniteStateChanged?.Invoke(isInfiniteLife);
    }

    public void InitPlayerPref()
    {
        if (!PlayerPrefs.HasKey("gold_count"))
        {
            SetGold(150);
        }
        else
        {
            OnGoldChanged?.Invoke(gold);
        }

        if (!PlayerPrefs.HasKey("life_count"))
        {
            SetLife(maxLifeCount);
        }
        else
        {
            if (PlayerPrefs.HasKey("regen_life_start"))
            {
                double passedRegenTime = new TimeSpan(DateTime.Now.Ticks - regenLifeStart).TotalSeconds;
                int gainLife = (int)(passedRegenTime / regenLifeTime);
                if (gainLife > 0)
                {
                    SetLife(Math.Clamp(life + gainLife, 0, maxLifeCount));
                }

                if (!isFullLife)
                {
                    lifeTimer = regenLifeTime - (passedRegenTime % regenLifeTime);
                }

            }
            else
            {
                SetLife(life);
            }

        }

        if (!PlayerPrefs.HasKey("life_infinite_duration") && !PlayerPrefs.HasKey("life_infinite_start"))
        {
            SetLifeInfinite(0);
        }
        else
        {
            double _infiniteLifeDuration = GetLifeInfiniteTimeLeft();
            HandleInfiniteLifeDuration(_infiniteLifeDuration);
        }
    }

    private void Update()
    {

        if (lifeTimer > 0)
        {
            lifeTimer -= Time.deltaTime;
            OnLifeTimerChanged?.Invoke(lifeTimer);
        }
        else
        {
            TerminateLifeInfinite();
            RegenLife();
        }
    }

    #region Handle Booster

    private void SetBooster(int amount, Booster booster, bool isGained = false)
    {
        switch (booster)
        {
            case Booster.MIXUP:
                PlayerPrefs.SetInt("mix_up_count", amount);
                OnMixUpBoosterCountChanged?.Invoke(amount, isGained);
                break;
            case Booster.PICKER:
                PlayerPrefs.SetInt("picker_count", amount);
                OnPickerBoosterCountChanged?.Invoke(amount, isGained);
                break;
            case Booster.DYNAMITE:
                PlayerPrefs.SetInt("dynamite_count", amount);
                OnDynamiteBoosterCountChanged?.Invoke(amount, isGained);
                break;
        }

        PlayerPrefs.Save();
    }

    public void GainBooster(int amount, Booster booster)
    {
        int boosterCount = 0;
        switch (booster)
        {
            case Booster.MIXUP:
                boosterCount = mixUpCount;
                break;
            case Booster.PICKER:
                boosterCount = pickerCount;
                break;
            case Booster.DYNAMITE:
                boosterCount = dynamiteCount;
                break;
        }

        SetBooster(boosterCount + amount, booster, true);
    }

    public bool BuyBooster(Booster booster, int _gold, int count)
    {
        //if(UseGold())
        if (UseGold(_gold))
        {
            GainBooster(count, booster);
            return true;
        }
        return false;
    }

    public bool UseBooster(Booster booster)
    {
        int boosterCount = 0;
        switch (booster)
        {
            case Booster.MIXUP:
                boosterCount = mixUpCount;
                break;
            case Booster.PICKER:
                boosterCount = pickerCount;
                break;
            case Booster.DYNAMITE:
                boosterCount = dynamiteCount;
                break;
        }
        if (boosterCount > 0)
        {
            SetBooster(boosterCount - 1, booster);
            GameLevelManager.instance.UpdateBoosterCount(booster);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetBoosterCount(Booster booster)
    {
        switch (booster)
        {
            case Booster.MIXUP:
                return mixUpCount;
            case Booster.PICKER:
                return pickerCount;
            case Booster.DYNAMITE:
                return dynamiteCount;
        }

        return 0;
    }

    #endregion

    #region Handle Life

    private void SetLife(int _life, bool notResetLifeRegen = false)
    {
        Debug.Log(_life);
        if (_life <= maxLifeCount && _life >= 0)
        {
            //update life regen timer if not have infinite life
            if (!isInfiniteLife)
            {
                if (_life < life)
                {
                    //on lost life
                    if (isFullLife)
                    {
                        //start regen time
                        ResetLifeRegenTimer();
                    }
                }
                else if (_life > life)
                {
                    //on gain life
                    if (_life != maxLifeCount)
                    {
                        if (!notResetLifeRegen)
                        {
                            //start regen time
                            ResetLifeRegenTimer();
                        }
                    }
                    else
                    {
                        lifeTimer = 0;
                    }
                }
            }

            PlayerPrefs.SetInt("life_count", _life);
            PlayerPrefs.Save();
            OnLifeChanged?.Invoke(_life);
        }
    }

    private void ResetLifeRegenTimer()
    {
        long current = DateTime.Now.Ticks;
        PlayerPrefs.SetString("regen_life_start", current.ToString());
        lifeTimer = regenLifeTime;
        PlayerPrefs.Save();
    }

    public void RegenLife()
    {
        if (!isFullLife)
        {
            SetLife(life + 1);
        }
    }

    public void LostLife()
    {
        if (!isInfiniteLife)
        {
            SetLife(life - 1);
        }
    }

    public void RefillLife(int amount)
    {
        SetLife(Math.Clamp(life + amount, 0, maxLifeCount), true);
    }

    #endregion

    #region Handle gold
    public void GainGold(int amount)
    {
        SetGold(gold + amount);
    }

    public bool UseGold(int amount)
    {
        if (gold >= amount)
        {
            SetGold(gold - amount);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetGold(int _gold)
    {
        PlayerPrefs.SetInt("gold_count", _gold);
        PlayerPrefs.Save();
        OnGoldChanged?.Invoke(_gold);
    }

    #endregion

    #region Handle infinite life

    public void TerminateLifeInfinite()
    {
        if (isInfiniteLife)
        {
            isInfiniteLife = false;
            OnLifeInfiniteStateChanged?.Invoke(isInfiniteLife);
        }
    }

    public void GainInfiniteLifeDuration(float _duration)
    {
        double _infiniteLifeDuration = GetLifeInfiniteTimeLeft();
        _infiniteLifeDuration = (_infiniteLifeDuration > 0) ? _infiniteLifeDuration : 0;

        SetLifeInfinite(_duration + (float)_infiniteLifeDuration);
    }

    public void SetLifeInfinite(float _duration)
    {
        long current = DateTime.Now.Ticks;
        PlayerPrefs.SetString("life_infinite_start", current.ToString());
        PlayerPrefs.SetFloat("life_infinite_duration", _duration);
        PlayerPrefs.Save();
        //update life
        HandleInfiniteLifeDuration(_duration);
    }

    public void HandleInfiniteLifeDuration(double _duration)
    {
        isInfiniteLife = _duration > 0;
        if (isInfiniteLife)
        {
            lifeTimer = _duration;
            SetLife(maxLifeCount);
        }
        OnLifeInfiniteStateChanged?.Invoke(isInfiniteLife);
    }

    public double GetLifeInfiniteTimeLeft()
    {
        return lifeInfiniteDuration - new TimeSpan(DateTime.Now.Ticks - lifeInfiniteStart).TotalSeconds;
    }

    #endregion

    public static string ConvertTime(double time)
    {
        int hour = (int)time / 3600;
        int min = (int)(time % 3600) / 60;
        int second = (int)(time % 3600) % 60;
        
        string hour_str = hour < 10 ? $"0{hour}h" : $"{hour}h";
        string min_str = min < 10 ? $"0{min}m" : $"{min}m";
        string second_str = second < 10 ? $"0{second}s" : $"{second}s";

        if (hour <= 0)
            return $"{min_str}{second_str}";
        else
            return $"{hour_str}{min_str}";
    }
}
