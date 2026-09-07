using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : BaseObject
{
    public static AudioManager INSTANCE;
    // Start is called before the first frame update
    private AudioSource bg;
    [SerializeField] private AudioSource bgEffect;
    [SerializeField] private List<AudioSource> bgList = new List<AudioSource>();
    private AudioSource bgDemo;
    [SerializeField] private string defaultId;
    void Start()
    {
        INSTANCE = this;
        loadData();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentMusicVolume != targetMusicVolume)
        {
            currentMusicVolume = currentMusicVolume * 0.98f + targetMusicVolume * 0.02f;
            if (Mathf.Abs(currentMusicVolume - targetMusicVolume) < 0.02f)
                currentMusicVolume = targetMusicVolume;
            if (bg)
                bg.GetComponent<AudioSource>().volume = currentMusicVolume;
        }
    }
    private float bgEffectStartVolume;
    private GameObject bgEffectVolumeController;
    [SerializeField] private float bgEffectDurationMin = 10;
    [SerializeField] private float bgEffectDurationMax = 30;
    [SerializeField] private float bgEffectDelayMin = 10;
    [SerializeField] private float bgEffectDelayMax = 15;
    private void initBackgroundEffect()
    {
        bgEffectVolumeController = new GameObject("bgEffectVolumeController");
        bgEffectVolumeController.AddComponent<MoveComponent>();
        bgEffectStartVolume = bgEffect.volume;
        
        startBackgroundEffect();
    }
    public void startBackgroundEffect()
    {
        if (musicEnable)
            bgEffect.Play();
        randomDurationBackgroundEffect();
    }
    public void randomDurationBackgroundEffect()
    {
        if (bgEffectDurationMax < bgEffectDurationMin)
            bgEffectDurationMax = bgEffectDurationMin;
        if (bgEffectDelayMax < bgEffectDelayMin)
            bgEffectDelayMax = bgEffectDelayMin;
        float duration = Random.Range(bgEffectDurationMin, bgEffectDurationMax + 1);
        float durationFadeIn = 1.0f;
        float durationFadeOut = 1.0f;
        bgEffectVolumeController.transform.position = new Vector3(bgEffect.volume, 0);
        bgEffectVolumeController.GetComponent<MoveComponent>().moveTo(new Vector3(bgEffectStartVolume,0), durationFadeIn)
            .thenMoveTo(new Vector3(bgEffectStartVolume, 0), duration - durationFadeIn - durationFadeOut)
            .thenMoveTo(new Vector3(0, 0), durationFadeOut)
            .setChangeAction(() => {
                bgEffect.volume = bgEffectVolumeController.transform.position.x;
            })
            .setEndAction(() => {
                bgEffectVolumeController.GetComponent<MoveComponent>().moveTo(new Vector3(0, 0), Random.Range(bgEffectDelayMin, bgEffectDelayMax + 1))
                    .setEndAction(() => { randomDurationBackgroundEffect(); });
            });
    }
    public void stopBackgroundEffect()
    {
        bgEffect.Stop();
    }
    public void randomBackground()
    {
        if (bg != null)
            bg.Stop();
        bg = bgList[Random.Range(0, bgList.Count)];
        //bg.Play();
        musicVolume = bg.GetComponent<AudioSource>().volume;
        updateMusicVolume(musicEnable ? musicVolume : 0, false);
    }
    public void playBackground()
    {
        if (bg)
            bg.Play();
    }
    public void stopBackground()
    {
        if (bg)
            bg.Stop();
    }
    public void playAudio(string name)
    {
        if (!effectEnable)
            return;
        if (transform.Find(name))
            transform.Find(name).GetComponent<AudioSource>().Play();
    }
    public void playDuplicatedAudio(string name, float volume = -1)
    {
        if (!effectEnable)
            return;
        if (transform.Find(name))
        {
            GameObject duplicated = Instantiate(transform.Find(name).gameObject);
            duplicated.GetComponent<AudioSource>().Play();
            if (volume >= 0)
                duplicated.GetComponent<AudioSource>().volume = volume;
            Destroy(duplicated, 10);
        }
    }
    public bool isMusicEnable()
    {
        return musicEnable;
    }
    public bool isEffectEnable()
    {
        return effectEnable;
    }
    public bool isVibrateEnable()
    {
        return vibrateEnable;
    }
    public bool changeMusicState()
    {
        musicEnable = !musicEnable;
        PlayerPrefs.SetInt("music_enable", musicEnable ? 1 : 0);
        PlayerPrefs.Save();
        updateMusicVolume(musicEnable ? musicVolume : 0, false);
        return musicEnable;
    }
    public bool changeEffectState()
    {
        effectEnable = !effectEnable;
        PlayerPrefs.SetInt("effect_enable", effectEnable ? 1 : 0);
        PlayerPrefs.Save();
        return effectEnable;
    }
    public bool changeVibrateState()
    {
        vibrateEnable = !vibrateEnable;
        PlayerPrefs.SetInt("vibrate_enable", effectEnable ? 1 : 0);
        PlayerPrefs.Save();
        return effectEnable;
    }
    public void doHaptic()
    {
        //if (vibrateEnable)
        //    HapticPatterns.PlayPreset(HapticPatterns.PresetType.SoftImpact);
    }

    public void fadeOutMusic()
    {
        if (musicEnable)
        {
            targetMusicVolume = 0.2f;
        }
    }
    public void fadeInMusic()
    {
        if (musicEnable)
        {
            targetMusicVolume = musicVolume;
        }
    }
    private void updateMusicVolume(float v, bool smooth = false)
    {
        targetMusicVolume = v;
        if (!smooth)
        {
            bg.GetComponent<AudioSource>().volume = v;
            currentMusicVolume = v;
        }
    }
    private bool vibrateEnable = false;
    private bool musicEnable = false;
    private bool effectEnable = false;
    private float musicVolume = 1;
    private float targetMusicVolume = 1;
    private float currentMusicVolume = 1;
    private void loadData()
    {
        musicEnable = PlayerPrefs.GetInt("music_enable", 1) == 1;
        effectEnable = PlayerPrefs.GetInt("effect_enable", 1) == 1;
        vibrateEnable = PlayerPrefs.GetInt("vibrate_enable", 1) == 1;
        targetMusicVolume = musicVolume;
        currentMusicVolume = targetMusicVolume;
        randomBackground();
        initBackgroundEffect();
    }
}
