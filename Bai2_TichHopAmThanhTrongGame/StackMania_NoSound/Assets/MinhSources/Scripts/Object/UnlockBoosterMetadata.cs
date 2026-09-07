using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Booster { NONE, MIXUP, PICKER, DYNAMITE }

[CreateAssetMenu(menuName = "Scriptable Objects/UnlockBoosterLevelMetadata")]
public class UnlockBoosterLevelMetadata : ScriptableObject
{
    public int level;
    public Booster newBooster;
    public int givedBoosterAmount;
    public string boosterName;
    public Sprite boosterIcon;
    public Sprite boosterTutoriorImage;
    public int boosterPrice;
    [TextArea(2, 4)]
    public string boosterDescription;

    public string tutorior_id => $"unlock_{newBooster}_tutorior";
}