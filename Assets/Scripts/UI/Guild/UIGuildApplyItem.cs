using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SkillBridge.Message;
using Services;

/// <summary>
/// 公会元素UI
/// </summary>
public class UIGuildApplyItem : ListView.ListViewItem
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
    /// 协议公会申请信息
    /// </summary>
    public NGuildApplyInfo Info;

    /// <summary>
    /// 设置公会元素UI信息
    /// </summary>
    /// <param name="item"></param>
    public void SetItemInfo(NGuildApplyInfo item)
    {
        this.Info = item;
        if (this.nickName != null) this.nickName.text = item.Name;
        if (this.@class != null) this.@class.text = item.Class.ToString();
        if (this.level != null) this.level.text = item.Level.ToString();
    }
    /// <summary>
    /// 点击接受按钮
    /// </summary>
    public void OnAccept()
    {
        MessageBox.Show(string.Format("确认要通过[{0}]的公会申请吗？", this.Info.Name), "审批申请", MessageBoxType.Confirm, "同意加入", "取消").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(true, this.Info);
        };
    }

    public void OnDecline()
    {
        MessageBox.Show(string.Format("确认要拒绝[{0}]的公会申请吗？", this.Info.Name), "审批申请", MessageBoxType.Confirm, "拒绝加入", "取消").OnYes = () =>
        {
            GuildService.Instance.SendGuildJoinApply(false, this.Info);
        };
    }
}
