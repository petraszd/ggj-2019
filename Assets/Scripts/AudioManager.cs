﻿using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioSource musicSource;
    public AudioSource fxSource;

    public AudioClip PissClip;
    public AudioClip BarkClip;
    public AudioClip LoseClip;
    public AudioClip WinClip;

    public float pitchRandomness = 0.2f;
    private float originalFXPitch;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        originalFXPitch = fxSource.pitch;
    }

    // Piss
    public static void PlayPiss()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayPiss();
    }

    private void InstancePlayPiss()
    {
        PlayFxClip(PissClip);
    }

    // Bark
    public static void PlayBark()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayBark();
    }

    private void InstancePlayBark()
    {
        PlayFxClip(BarkClip);
    }

    // Lose
    public static void PlayLose()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayLose();
    }

    private void InstancePlayLose()
    {
        PlayFxClip(LoseClip);
    }

    // Win
    public static void PlayWin()
    {
        if (instance == null)
        {
            return;
        }
        instance.InstancePlayWin();
    }

    private void InstancePlayWin()
    {
        PlayFxClip(WinClip);
    }

    private void PlayFxClip(AudioClip clip)
    {
        fxSource.pitch = originalFXPitch + Random.Range(-pitchRandomness, pitchRandomness);
        fxSource.PlayOneShot(clip);
    }
}