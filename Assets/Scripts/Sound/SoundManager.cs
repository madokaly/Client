using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// 音量管理器
/// </summary>
public class SoundManager : MonoSingleton<SoundManager>
{
    /// <summary>
    /// 混音器
    /// </summary>
    public AudioMixer audioMixer;
    /// <summary>
    /// 音乐源
    /// </summary>
    public AudioSource musicAudioSource;
    /// <summary>
    /// 音效源
    /// </summary>
    public AudioSource soundAudioSource;

    private const string MusicPath = "Music/";

    private const string SoundPath = "Sound/";

    private bool musicOn;
    public bool MusicOn
    {
        get
        {
            return musicOn;
        }
        set
        {
            musicOn = value;
            this.MusicMute(!musicOn);
        }
    }

    private bool soundOn;
    public bool SoundOn
    {
        get
        {
            return soundOn;
        }
        set
        {
            soundOn = value;
            this.SoundMute(!soundOn);
        }
    }

    private int musicVolume;
    public int MusicVolume
    {
        get
        {
            return musicVolume;
        }
        set
        {
            if(musicVolume != value)
            {
                musicVolume = value;
                if (musicOn) this.SetVolume("musicVolume", musicVolume);
            }
        }
    }

    private int soundVolume;
    public int SoundVolume
    {
        get
        {
            return soundVolume;
        }
        set
        {
            if (soundVolume != value)
            {
                soundVolume = value;
                if (soundOn) this.SetVolume("soundVolume", soundVolume);
            }
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    private void Start()
    {
        this.MusicOn = Config.MusicOn;
        this.SoundOn = Config.SoundOn;
        this.MusicVolume = Config.MusicVolume;
        this.SoundVolume = Config.SoundVolume;
    }

    public void MusicMute(bool mute)
    {
        this.SetVolume("MusicVolume", mute ? 0 : musicVolume);
    }

    public void SoundMute(bool mute)
    {
        this.SetVolume("SoundVolume", mute ? 0 : soundVolume);
    }
    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    private void SetVolume(string name, int value)
    {
        float volume = value * 0.5f - 50f;
        audioMixer.SetFloat(name.Substring(0,1).ToUpper() + name.Substring(1), volume);
    }
    /// <summary>
    /// 播放音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayMusic(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(MusicPath + name);
        if(clip == null)
        {
            Debug.LogWarningFormat("PlayMusic：[{0}] not existed", name);
            return;
        }
        if (musicAudioSource.isPlaying)
        {
            musicAudioSource.Stop();
        }
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name)
    {
        AudioClip clip = Resloader.Load<AudioClip>(SoundPath + name);
        if (clip == null)
        {
            Debug.LogWarningFormat("PlaySound：[{0}] not existed", name);
            return;
        }
        if (soundAudioSource.isPlaying)
        {
            soundAudioSource.Stop();
        }
        soundAudioSource.clip = clip;
        soundAudioSource.Play();
    }
}
