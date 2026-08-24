using System;
using System.IO;
using UnityEngine;

public static class WavUtility
{
    private const int HEADER_SIZE = 44;

    public static bool Save(string filePath, AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("AudioClip is null.");
            return false;
        }

        // Ensure directories exist
        string directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        try
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                // Write an empty header footprint first
                byte[] emptyByte = new byte[HEADER_SIZE];
                fileStream.Write(emptyByte, 0, HEADER_SIZE);

                // Extract and write raw audio sample data
                float[] samples = new float[clip.samples * clip.channels];
                clip.GetData(samples, 0);

                short[] intData = new short[samples.Length];
                byte[] bytesData = new byte[samples.Length * 2];

                const float rescaleFactor = 32767; // Convert float to Int16 truncation limit

                for (int i = 0; i < samples.Length; i++)
                {
                    intData[i] = (short)(samples[i] * rescaleFactor);
                    byte[] byteArr = BitConverter.GetBytes(intData[i]);
                    byteArr.CopyTo(bytesData, i * 2);
                }

                fileStream.Write(bytesData, 0, bytesData.Length);

                // Rewind and overwrite the real WAV header details
                fileStream.Seek(0, SeekOrigin.Begin);

                // RIFF identifier
                fileStream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);

                // File size minus RIFF/WAVE header chunk
                fileStream.Write(BitConverter.GetBytes(fileStream.Length - 8), 0, 4);

                // WAVE identifier
                fileStream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);

                // fmt chunk identifier
                fileStream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);

                // Chunk size (16 for PCM)
                fileStream.Write(BitConverter.GetBytes(16), 0, 4);

                // Sample format (1 for PCM)
                fileStream.Write(BitConverter.GetBytes((short)1), 0, 2);

                // Channel count
                fileStream.Write(BitConverter.GetBytes((short)clip.channels), 0, 2);

                // Sample rate
                fileStream.Write(BitConverter.GetBytes(clip.frequency), 0, 4);

                // Byte rate: (sample rate * channels * bits per sample) / 8
                fileStream.Write(BitConverter.GetBytes(clip.frequency * clip.channels * 2), 0, 4);

                // Block align: (channels * bits per sample) / 8
                fileStream.Write(BitConverter.GetBytes((short)(clip.channels * 2)), 0, 2);

                // Bits per sample
                fileStream.Write(BitConverter.GetBytes((short)16), 0, 2);

                // data chunk identifier
                fileStream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);

                // Data chunk size
                fileStream.Write(BitConverter.GetBytes(fileStream.Length - HEADER_SIZE), 0, 4);
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save audio file: {ex.Message}");
            return false;
        }
    }
}