using Battle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Buff元素UI
/// </summary>
public class UIBuffItem : MonoBehaviour
{
    /// <summary>
    /// 图标
    /// </summary>
    public Image icon;
    /// <summary>
    /// 遮罩
    /// </summary>
    public Image overlay;
    /// <summary>
    /// 剩余时间
    /// </summary>
    public Text label;
    /// <summary>
    /// Buff
    /// </summary>
    private Buff buff;

    /// <summary>
    /// 设置元素
    /// </summary>
    /// <param name="buff"></param>
    internal void SetItem(Buff buff)
    {
        this.buff = buff;
        if(this.icon != null)
        {
            this.icon.overrideSprite = Resloader.Load<Sprite>(this.buff.Define.Icon);
            this.icon.SetAllDirty();
        }
    }
    /// <summary>
    /// 更新
    /// </summary>
    private void Update()
    {
        if (this.buff == null) return;
        if(this.buff.time > 0)
        {
            if (!this.overlay.enabled) this.overlay.enabled = true;
            if (!this.label.enabled) this.label.enabled = true;
            overlay.fillAmount = this.buff.time / this.buff.Define.Duration;
            this.label.text = ((int)Math.Ceiling(this.buff.Define.Duration - this.buff.time)).ToString();
        }
        else
        {
            if (this.overlay.enabled) this.overlay.enabled = false;
            if (this.label.enabled) this.label.enabled = false;
        }
    }
}
