using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string soundName;
    
    public AudioClip clip;
    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    public bool playOnAwake;

    
    [Range(0f, 1f)]
    public float spatialBlend;

    public float minDistance;
    public float maxDistance;

    
    [HideInInspector]
    public AudioSource audioSource;
}
