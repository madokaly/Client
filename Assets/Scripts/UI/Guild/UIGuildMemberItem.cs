using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Common.Utils;
/// <summary>
/// 公会成员元素UI
/// </summary>
public class UIGuildMemberItem : ListView.ListViewItem
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text nickName;
    /// <summary>
    /// 职业
    /// </summary>
    public Text @class;
    /// <summary>
    /// 等级
    /// </summary>
    public Text level;
    /// <summary>
    /// 职务
    /// </summary>
    public Text title;
    /// <summary>
    /// 加入时间
    /// </summary>
    public Text joinTime;
    /// <summary>
    /// 在线状态
    /// </summary>
    public Text status;

    public Image background;

    public Sprite normalBg;

    public Sprite selectedBg;

    public NGuildMemberInfo Info;

    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectedBg : normalBg;
    }
    /// <summary>
    /// 设置公会成员信息UI
    /// </summary>
    /// <param name="item"></param>
    public void SetGuildMemberInfo(NGuildMemberInfo item)
    {
        this.Info = item;
        if (this.nickName != null) this.nickName.text = Info.Info.Name;
        if (this.@class != null) this.@class.text = Info.Info.Class.ToString();
        if (this.level != null) this.level.text = Info.Info.Level.ToString();
        if (this.title != null) this.title.text = Info.Title.ToString();
        if (this.joinTime != null) this.joinTime.text = TimeUtil.GetTime(Info.joinTime).ToShortDateString();
        if (this.status != null) this.status.text = Info.Status == 1 ? "在线" : TimeUtil.GetTime(Info.lastTime).ToShortDateString();
    }
}
