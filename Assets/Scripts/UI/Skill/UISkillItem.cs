using Battle;
using Common.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 技能元素UI
/// </summary>
public class UISkillItem : ListView.ListViewItem
{
    /// <summary>
    /// 图标
    /// </summary>
    public Image icon;
    /// <summary>
    /// 名字
    /// </summary>
    public Text title;
    /// <summary>
    /// 等级
    /// </summary>
    public Text level;
    /// <summary>
    /// 背景
    /// </summary>
    public Image background;
    /// <summary>
    /// 普通背景
    /// </summary>
    public Sprite normalBg;
    /// <summary>
    /// 选择背景
    /// </summary>
    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        this.background.overrideSprite = selected ? selectedBg : normalBg;
    }

    public Skill item;
    /// <summary>
    /// 设置技能元素UI
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="owner"></param>
    /// <param name="equiped"></param>
    public void SetItem(Skill skill, UISkill owner, bool equiped)
    {
        this.item = skill;
        if (this.title != null) this.title.text = this.item.Define.Name;
        if (this.level != null) this.level.text = this.item.Info.Level.ToString();
        if (this.icon != null) this.icon.overrideSprite = Resloader.Load<Sprite>(this.item.Define.Icon);
    }
}
