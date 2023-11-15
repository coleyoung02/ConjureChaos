using System;
using UnityEngine;
using UnityEngine.Audio;

// from tutorial https://www.youtube.com/watch?v=6OT43pvUyfY&ab_channel=Brackeys

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.Loop;
        }

        Play("Background Music");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}
