using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 生物信息UI
/// </summary>
public class UICreatureInfo : MonoBehaviour
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text Name;
    /// <summary>
    /// Hp
    /// </summary>
    public Slider HpBar;
    /// <summary>
    /// Mp
    /// </summary>
    public Slider MpBar;
    /// <summary>
    /// Hp文本
    /// </summary>
    public Text HpText;
    /// <summary>
    /// Mp文本
    /// </summary>
    public Text MpText;

    public UIBuffIcons buffIcons;
    /// <summary>
    /// 目标
    /// </summary>
    private Creature target;
    public Creature Target
    {
        get { return target; }
        set
        {
            this.target = value;
            buffIcons.SetOwner(target);
            this.UpdateUI();
        }
    }
    /// <summary>
    /// 更新UI
    /// </summary>
    private void UpdateUI()
    {
        if (this.target == null) return;
        this.Name.text = string.Format("{0} Lv.{1}", this.target.Name, this.target.Info.Level);
        this.HpBar.maxValue = this.target.Attributes.MaxHP;
        this.HpBar.value = this.target.Attributes.HP;
        this.HpText.text = string.Format("{0}/{1}", this.target.Attributes.HP, this.target.Attributes.MaxHP);
        this.MpBar.maxValue = this.target.Attributes.MaxMP;
        this.MpBar.value = this.target.Attributes.MP;
        this.MpText.text = string.Format("{0}/{1}", this.target.Attributes.MP, this.target.Attributes.MaxMP);
    }

    private void Update()
    {
        UpdateUI();
    }
}
