using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KienChi
{
    public enum Difficulty { NORMAL, DIFFICULT, BRUTAL };

    public enum NewFeature { NONE, FIRST_TIME, STACKED_BLOCKS, CAGE_BLOCK, LINKED_CARTS, HIDDEN_CART, BIG_CART };

    [CreateAssetMenu(menuName = "Scriptable Objects/UnlockFeatureLevelMetadata")]
    public class UnlockFeatureLevelMetadata : ScriptableObject
    {
        public int level;
        public NewFeature newFeature;
        public Sprite featureIcon;
        public string featureName;

        public string tutorior_id => $"unlock_{newFeature}_tutorior";
    }
}

