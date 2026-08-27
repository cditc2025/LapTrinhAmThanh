
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace Practice
{
    public class MusicMaker : MonoBehaviour
    {
        public TextAsset rightHandSheet;
        public TextAsset leftHandSheet;
        [Header("Settings")]
        [Range(60f, 120f)]
        public float bpm = 65f;
        public SampleType type;
        [Range(1, 3)]
        public float tone = 1;

        int sampleRate = 80000;

        public void GenerateMusicAudioClip()
        {
            List<float> samples = new List<float>();
            //
            List<MusicNote> notes = GetMusicNotes(rightHandSheet);

            for (int i = 0; i < notes.Count; i++)
            {
                samples.AddRange(GenerateNoteSample(notes[i]));
            }

            List<float> samplesLeft = new List<float>();
            //
            List<MusicNote> notesLeft = GetMusicNotes(leftHandSheet);

            for (int i = 0; i < notesLeft.Count; i++)
            {
                samplesLeft.AddRange(GenerateNoteSample(notesLeft[i]));
            }

            float max = 0;
            for (int i = 0; i < samples.Count; i++)
            {
                samples[i] = samples[i] + ((i < samplesLeft.Count) ? samplesLeft[i] : 0);

                if (max < samples[i])
                {
                    max = samples[i];
                }
            }

            for (int i = 0; i < samples.Count; i++)
            {
                samples[i] = samples[i] / max;
            }

            //
            AudioClip myClip = AudioClip.Create("MusicAudio", samples.Count, 1, sampleRate, false);

            myClip.SetData(samples.ToArray(), 0);

            //save audio
            string path = Path.Combine(Application.dataPath, "Practice/MusicMaker/MusicAudio.wav");
            bool success = WavUtility.Save(path, myClip);

            if (success)
            {
                Debug.Log($"Audio successfully saved to: {path}");
                AssetDatabase.Refresh();

            }
        }

        public float[] GenerateNoteSample(MusicNote note)
        {
            if (note.frequency.Length == 0) return new float[0];


            float duration = note.GetDuration(bpm);
            int totalSamples = (int)(sampleRate * duration);
            float[] sampleData = new float[totalSamples];

            //----------Start Question 3-----------










            //----------End Question 3-----------

            return sampleData;
        }

        public enum SampleType { Digital, Piano };

        public float GetSample(float t, float frequency, SampleType type = SampleType.Digital)
        {
            float omega = Mathf.PI * frequency;
            //
            float sample = 0;
            //--------Start Question 3------------
            switch (type)
            {
                case SampleType.Digital:
                    sample = Mathf.Sin(2 * omega * t);
                    return sample;

                case SampleType.Piano:


                    return sample;

                default:
                    sample = Mathf.Sin(2 * omega * t);
                    return sample;
            }
            //--------End Question 3------------
        }

        public List<MusicNote> GetMusicNotes(TextAsset textAsset)
        {
            List<MusicNote> musicSheet = new List<MusicNote>();
            string asset = textAsset.text;
            string[] notes = asset.Split("/");

            for (int i = 0; i < notes.Length; i++)
            {
                MusicNote musicNote = new MusicNote();
                string note = notes[i].Trim();
                string[] splitNote = note.Split("-");
                //
                musicNote.tempo = float.Parse(splitNote[1]);
                //
                string[] splitFrequency = splitNote[0].Split("+");
                musicNote.frequency = new float[splitFrequency.Length];
                for (int j = 0; j < splitFrequency.Length; j++)
                {
                    musicNote.frequency[j] = float.Parse(splitFrequency[j]);
                }
                //
                musicSheet.Add(musicNote);

            }

            return musicSheet;
        }
    }

    public class MusicNote
    {
        public float[] frequency; //frequency of music note
        public float tempo; //tempo of music note

        //--------Start Question 3------------
        public float GetDuration(float bpm)
        {
            return 0;
        }
        //--------End Question 3------------
    }

}