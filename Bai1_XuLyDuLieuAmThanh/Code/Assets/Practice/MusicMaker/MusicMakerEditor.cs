
using UnityEditor;
using UnityEngine;

namespace Practice
{
    [CustomEditor(typeof(MusicMaker))]
    public class MusicMakerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draws the default fields (variables) of your script
            DrawDefaultInspector();

            MusicMaker musicMaker = (MusicMaker)target;

            // Creates the button and checks if it was clicked
            if (GUILayout.Button("Generate Audio"))
            {
                musicMaker.GenerateMusicAudioClip();
            }
        }
    }
}

