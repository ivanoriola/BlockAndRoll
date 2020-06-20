using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    AudioSource[] audioSources;
    float maxVolume = 0.25f;
    float minVolume = 0f;
    float step = .05f;

    private static string CHANGE_MUSIC_COROUTINE = "ChangeMusic";

    void Start()
    {
        audioSources = GetComponents<AudioSource>();
    }
    private void RoundVolumenes()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.volume < (minVolume + step))
            {
                audioSource.volume = minVolume;
            }
            if (audioSource.volume > (maxVolume - step))
            {
                audioSource.volume = maxVolume;
            }
        }
    }
    public void NewMusic(int level)
    {
        StartCoroutine(CHANGE_MUSIC_COROUTINE, level);
    }

    IEnumerator ChangeMusic(int level)
    {
        for (float i = 0; i < (maxVolume - minVolume) / step; i = i + step)
        {
            if (level >= 0 && level < 5)
                audioSources[0].volume = Mathf.Lerp(audioSources[0].volume, maxVolume, step);
            else audioSources[0].volume = Mathf.Lerp(audioSources[0].volume, minVolume, step);

            if (level > 4 && level < 9)
                audioSources[1].volume = Mathf.Lerp(audioSources[1].volume, maxVolume, step);
            else audioSources[1].volume = Mathf.Lerp(audioSources[1].volume, minVolume, step);

            if (level > 8 && level < 13)
                audioSources[2].volume = Mathf.Lerp(audioSources[2].volume, maxVolume, step);
            else audioSources[2].volume = Mathf.Lerp(audioSources[2].volume, minVolume, step);

            if (level > 12 && level < 17)
                audioSources[3].volume = Mathf.Lerp(audioSources[3].volume, maxVolume, step);
            else audioSources[3].volume = Mathf.Lerp(audioSources[3].volume, minVolume, step);

            if (level > 16 && level < 21)
                audioSources[4].volume = Mathf.Lerp(audioSources[4].volume, maxVolume, step);
            else audioSources[4].volume = Mathf.Lerp(audioSources[4].volume, minVolume, step);

            if (level > 20 && level < 25)
                audioSources[5].volume = Mathf.Lerp(audioSources[5].volume, maxVolume, step);
            else audioSources[5].volume = Mathf.Lerp(audioSources[5].volume, minVolume, step);

            if (level > 24)
                audioSources[6].volume = Mathf.Lerp(audioSources[6].volume, maxVolume, step);
            else audioSources[6].volume = Mathf.Lerp(audioSources[6].volume, minVolume, step);

            yield return null;
        }
        RoundVolumenes();
    }
}