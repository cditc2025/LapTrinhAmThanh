using UnityEditor;
using UnityEngine;
namespace Practice
{
    [CustomEditor(typeof(AddAudio))]
    public class AddAudioEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Draws the default fields (variables) of your script
            DrawDefaultInspector();

            AddAudio addAudio = (AddAudio)target;

            // Creates the button and checks if it was clicked
            if (GUILayout.Button("Merge Audio"))
            {
                addAudio.MergeAudio();
            }

            // Creates the button and checks if it was clicked
            if (GUILayout.Button("Generate Audio"))
            {
                addAudio.GenerateTone();
            }
        }
    }
}
