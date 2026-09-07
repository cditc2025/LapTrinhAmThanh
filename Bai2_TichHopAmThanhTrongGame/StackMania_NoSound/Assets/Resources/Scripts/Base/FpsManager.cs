using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsManager : MonoBehaviour
{
    [SerializeField] private Text _fpsText;
    [SerializeField] private float _hudRefreshRate = 1f;

    private float _timer;
    private float avgFps = 60;
    private List<float> last5 = new List<float>();

    public static FpsManager INSTANCE;

    private void Awake()
    {
        INSTANCE = this;
        Application.targetFrameRate = 300;
        _fpsText = GetComponent<Text>();
    }
    private void Start()
    {
    }

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            last5.Add(fps);
            if (last5.Count > 5)
            {
                last5.RemoveAt(0);
                avgFps = (last5[0] + last5[1] + last5[2] + last5[3] + last5[4]) / 5.0f;
            }
            _fpsText.text = "" + fps + " " + Application.targetFrameRate + " " + QualitySettings.vSyncCount;
            _timer = Time.unscaledTime + _hudRefreshRate;
            Debug.Log("Fps " + _fpsText.text);
        }
    }
    public float getAvgFps()
    {
        return avgFps;
    }
}
