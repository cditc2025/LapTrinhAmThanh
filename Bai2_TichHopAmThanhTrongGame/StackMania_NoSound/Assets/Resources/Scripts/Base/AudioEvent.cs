using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onAudioEvent(string name)
    {
        AudioManager.INSTANCE.playAudio(name);
    }
    public void onAudioDuplicatedEvent(string name)
    {
        AudioManager.INSTANCE.playDuplicatedAudio(name);
    }
}
