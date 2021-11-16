using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;
    public AudioSource textSfxSource;

    [Header("bgm")]
    public AudioClip packagerBgm;
    public AudioClip inGameBgm;

    [Header("sfx")]
    public AudioClip endLineSfx;
    public AudioClip deathBoxSfx;
    public AudioClip textAudioSfx;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }
    private void Start()
    {
        textSfxSource.clip = textAudioSfx;
    }
    public void PlayBgmSound(AudioClip clip, float volume = 0.3f)
    {
        if (bgmSource.isPlaying) bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.volume = volume;
        bgmSource.Play();
    }

    public void PlaySfxSound(AudioClip clip, float volume)
    {
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayTextSfx(float volume = 0.13f)
    {
        textSfxSource.volume = volume;
        textSfxSource.Play();
    }

    public void StopTextSfx()
    {
        textSfxSource.Stop();
    }
}
