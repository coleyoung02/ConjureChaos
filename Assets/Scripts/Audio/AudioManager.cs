using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioMixer mixer;
    [SerializeField] private AudioSource hurtSource;
    [SerializeField] private AudioSource UISource;
    [SerializeField] private AudioSource MusicSource;
    private int songIndex;


    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "SFXVolume";

    public void PauseMusic()
    {
        MusicSource.Pause();
    }

    public void PitchDown(float duration)
    {
        StopAllCoroutines();
        StartCoroutine(DownPitch(duration));
    }

    private IEnumerator DownPitch(float duration)
    {
        for (float f = 0; f < duration / 7f; f += Time.unscaledDeltaTime)
        {
            MusicSource.pitch = Mathf.Lerp(1, .5f, f / (duration / 5));
            yield return new WaitForEndOfFrame();
        }
        MusicSource.pitch = .5f;
        yield return new WaitForSecondsRealtime(duration * 4f / 5);
        for (float f = 0; f < duration / 5f; f += Time.unscaledDeltaTime)
        {
            MusicSource.pitch = Mathf.Lerp(.5f, 1f, f / (duration * 2f / 7));
            yield return new WaitForEndOfFrame();
        }
        MusicSource.pitch = 1f;

    }


    public void PlayMusic()
    {
        MusicSource.Play();
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadVolume();

    }

    void LoadVolume() //Volume saved in VolumeSettings.cs
    {
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
        float SFXVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

        mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
        mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(SFXVolume) * 20);
    }

    public void PlayHurtNoise()
    {
        hurtSource.Play();
    }

    public void PlayUISound(AudioClip clip)
    {
        UISource.clip = clip;
        UISource.Play();
    }
}
