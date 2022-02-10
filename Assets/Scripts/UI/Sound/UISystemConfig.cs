using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 系统设置UI
/// </summary>
public class UISystemConfig : UIWindow
{
    /// <summary>
    /// 音乐关闭image
    /// </summary>
    public Image musicOff;
    /// <summary>
    /// 音效关闭image
    /// </summary>
    public Image soundOff;
    /// <summary>
    /// 音乐开关
    /// </summary>
    public Toggle toggleMusic;
    /// <summary>
    /// 音效开关
    /// </summary>
    public Toggle toggleSound;
    /// <summary>
    /// 音乐滑动条
    /// </summary>
    public Slider sliderMusic;
    /// <summary>
    /// 音效滑动条
    /// </summary>
    public Slider sliderSound;

    private void Start()
    {
        this.toggleMusic.isOn = Config.MusicOn;
        this.toggleSound.isOn = Config.SoundOn;
        this.sliderMusic.value = Config.MusicVolume;
        this.sliderSound.value = Config.SoundVolume;
    }

    public override void OnYesClick()
    {
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        PlayerPrefs.Save();
        base.OnYesClick();
    }

    public void MusicToggle(bool on)
    {
        this.musicOff.enabled = !on;
        Config.MusicOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void SoundToggle(bool on)
    {
        this.soundOff.enabled = !on;
        Config.SoundOn = on;
        SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
    }

    public void MusicVolume(float vol)
    {
        Config.MusicVolume = (int)vol;
        this.PlaySound();
    }

    public void SoundVolume(float vol)
    {
        Config.SoundVolume = (int)vol;
        this.PlaySound();
    }

    float lastPlay = 0;
    private void PlaySound()
    {
        if(Time.realtimeSinceStartup - lastPlay > 0.1)
        {
            lastPlay = Time.realtimeSinceStartup;
            SoundManager.Instance.PlaySound(SoundDefine.SFX_UI_Click);
        }
    }
}
