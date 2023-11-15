using UnityEngine.Audio;
using UnityEngine;

// from tutorial https://www.youtube.com/watch?v=6OT43pvUyfY&ab_channel=Brackeys

[System.Serializable] //allows you to add audio objects in the editor
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    public bool Loop;

    [HideInInspector]
    public AudioSource source;
}
