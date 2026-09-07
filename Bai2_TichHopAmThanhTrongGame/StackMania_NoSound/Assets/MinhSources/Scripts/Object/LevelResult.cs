using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelResult
{
    public int level = 0;
    public int retry_count = 0;
    public int booster_count = 0;
    public int slot_add = 0;
    public int pointer = 0;
    public int shuffle = 0;
    public int dynamite = 0;
    public int revive = 0;

    public string ToJSON()
    {
        return JsonUtility.ToJson(this);
    }

    public static LevelResult ConvertToObject(string json)
    {
        return JsonUtility.FromJson<LevelResult>(json);
    }
}
