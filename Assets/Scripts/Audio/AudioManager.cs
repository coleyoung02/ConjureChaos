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
    [SerializeField] private List<AudioClip> songs;
    private int songIndex;


    public const string MUSIC_KEY = "musicVolume";
    public const string SFX_KEY = "SFXVolume";

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
        songs = songs.OrderBy(x => UnityEngine.Random.value).ToList();
        songIndex = 0;
        PlayNextSong();

    }

    private void PlayNextSong()
    {
        songIndex = (songIndex + 1) % songs.Count;
        MusicSource.clip = songs[songIndex];
        MusicSource.Play();
        StartCoroutine(SwitchSong());
    }

    private IEnumerator SwitchSong()
    {
        yield return new WaitForSeconds(MusicSource.clip.length);
        PlayNextSong();
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
