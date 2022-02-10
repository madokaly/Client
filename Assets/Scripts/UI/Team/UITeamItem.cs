using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITeamItem : ListView.ListViewItem
{
    /// <summary>
    /// 名字
    /// </summary>
    public Text nickName;
    /// <summary>
    /// 职业图标
    /// </summary>
    public Image classIcon;
    /// <summary>
    /// 队长图标
    /// </summary>
    public Image leaderIcon;
    /// <summary>
    /// 背景
    /// </summary>
    public Image background;
    /// <summary>
    /// 未选择背景图
    /// </summary>
    public Sprite normalBg;
    /// <summary>
    /// 选择背景图
    /// </summary>
    public Sprite selectBg;


    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectBg : normalBg;
    }

    public int idx;

    public NCharacterInfo Info;

    /// <summary>
    /// 设置队伍队员信息
    /// </summary>
    /// <param name="idx">索引</param>
    /// <param name="item">队员信息</param>
    /// <param name="isLeader">是否为队长</param>
    public void SetMemberInfo(int idx,NCharacterInfo item,bool isLeader)
    {
        this.idx = idx;
        this.Info = item;
        if (this.nickName != null) this.nickName.text = Info.Level.ToString().PadRight(4) + Info.Name;
        if (this.classIcon != null) this.classIcon.overrideSprite = SpriteManager.Instance.classIcons[(int)this.Info.Class - 1];
        if (this.leaderIcon != null) this.leaderIcon.gameObject.SetActive(isLeader);
    }
}
