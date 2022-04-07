using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{
    [EventRef] public string level_music_event = "";
    FMOD.Studio.EventInstance level_music;

    // Start is called before the first frame update
    void Start()
    {
        level_music = RuntimeManager.CreateInstance(level_music_event);
        level_music.start();
    }

    public void StopMusic()
    {
        level_music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
}
