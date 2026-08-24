using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

namespace Practice
{
    public class AddAudio : MonoBehaviour
    {
        public AudioClip audioClip1;
        public AudioClip audioClip2;

        public bool isMerge = false;

        public void GenerateTone()
        {
            int sampleRate = 44100; // 44.1 kHz
            float duration = 2.0f;  // Duration in seconds
            int totalSamples = (int)(sampleRate * duration);
            float[] sampleData = new float[totalSamples];

            //---------Start Question 1------------




            //---------End Question 1------------

            AudioClip myClip = AudioClip.Create("GeneratedTone", totalSamples, 1, sampleRate, false);

            myClip.SetData(sampleData, 0);

            //save audio
            string path = Path.Combine(Application.dataPath, "DemoAudio/GeneratedTone.wav");
            bool success = WavUtility.Save(path, myClip);

            if (success)
            {
                Debug.Log($"Audio successfully saved to: {path}");
                AssetDatabase.Refresh();

            }
        }

        public void MergeAudio()
        {
            float[] sampleAudio1 = GetSampleFromAudio(audioClip1);
            float[] sampleAudio2 = GetSampleFromAudio(audioClip2);
            //
            int duration = sampleAudio1.Length < sampleAudio2.Length ? sampleAudio1.Length : sampleAudio2.Length;
            float[] sampleMix = new float[duration];

            //-----------Start Question 2----------------
            float maxSample = 0f;
            for (int i = 0; i < duration; i++)
            {
                sampleMix[i] = (sampleAudio1[i] + sampleAudio2[i]);
                if (maxSample < sampleMix[i])
                {
                    maxSample = sampleMix[i];
                }
            }

            for (int i = 0; i < duration; i++)
            {
                sampleMix[i] /= maxSample;
            }
            //-----------End Question 2----------------
            AudioClip myClip = AudioClip.Create("MixAudio", sampleMix.Length, audioClip1.channels, audioClip1.frequency, false);
            myClip.SetData(sampleMix, 0);

            //save audio
            string path = Path.Combine(Application.dataPath, "DemoAudio/myMixAudio.wav");

            bool success = WavUtility.Save(path, myClip);

            if (success)
            {
                Debug.Log($"Audio successfully saved to: {path}");
                AssetDatabase.Refresh();

            }
        }

        public float[] GetSampleFromAudio(AudioClip clip)
        {
            float[] results = new float[clip.samples * clip.channels];
            clip.GetData(results, 0);

            return results;
        }
    }
}