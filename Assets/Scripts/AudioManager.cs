﻿using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;


    private void Awake()
    {
        foreach (var sound in sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.volume = sound.volume;
            sound.audioSource.pitch = sound.pitch;
            sound.audioSource.loop = sound.loop;
        }
    }

    void Start()
    {
        Play("Level1SongDemo");
    }

    public void Play(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.soundName == soundName);
        s?.audioSource.Play();
    }

    public void PlayOneShot(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.soundName == soundName);
        s?.audioSource.PlayOneShot(s.clip);
    }

    public void Pause(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.soundName == soundName);
        s?.audioSource.Pause();
    }
    
    public void UnPause(string soundName)
    {
        var s = Array.Find(sounds, sound => sound.soundName == soundName);
        s?.audioSource.UnPause();
    }
    
    public void SetPitch(string soundName, float pitch)
    {
        var s = Array.Find(sounds, sound => sound.soundName == soundName);
        if (s == null) return;
        s.audioSource.pitch = pitch;
    }
}
