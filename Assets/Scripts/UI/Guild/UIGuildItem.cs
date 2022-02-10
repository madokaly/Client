using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;

/// <summary>
/// 公会元素UI
/// </summary>
public class UIGuildItem : ListView.ListViewItem
{
    /// <summary>
    /// 公会ID
    /// </summary>
    public Text guildID;
    /// <summary>
    /// 公会名字
    /// </summary>
    public Text guildName;
    /// <summary>
    /// 公会成员数
    /// </summary>
    public Text memberNumber;
    /// <summary>
    /// 公会会长
    /// </summary>
    public Text leader;
    /// <summary>
    /// 
    /// </summary>
    public NGuildInfo Info;

    public Image background;

    public Sprite normalBg;

    public Sprite selectedBg;

    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectedBg : normalBg;
    }
    /// <summary>
    /// 设置公会元素UI信息
    /// </summary>
    /// <param name="item"></param>
    public void SetGuildInfo(NGuildInfo item)
    {
        this.Info = item;
        if (this.guildID != null) this.guildID.text = item.Id.ToString();
        if (this.guildName != null) this.guildName.text = item.GuildName;
        if (this.memberNumber != null) this.memberNumber.text = item.memberCount.ToString();
        if (this.leader != null) this.leader.text = item.leaderName;
    }
}
