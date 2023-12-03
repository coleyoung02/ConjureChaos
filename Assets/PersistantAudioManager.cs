using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantAudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource hurtSource;
    [SerializeField] private AudioSource UISource;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
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
