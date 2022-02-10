using SkillBridge.Message;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI好友元素类
/// </summary>
public class UIFriendItem : ListView.ListViewItem
{
    /// <summary>
    /// 好友名字
    /// </summary>
    public Text friendname;
    /// <summary>
    /// 好友等级
    /// </summary>
    public Text level;
    /// <summary>
    /// 好友职业
    /// </summary>
    public Text @class;
    /// <summary>
    /// 好友状态
    /// </summary>
    public Text status;
    /// <summary>
    /// 背景
    /// </summary>
    public Image background;
    /// <summary>
    /// 未选择图片
    /// </summary>
    public Sprite normalbg;
    /// <summary>
    /// 选择图片
    /// </summary>
    public Sprite selectbg;
    /// <summary>
    /// 被选择后执行
    /// </summary>
    /// <param name="selected"></param>
    public override void onSelected(bool selected)
    {
        background.overrideSprite = selected ? selectbg : normalbg;
    }
    /// <summary>
    /// 好友信息
    /// </summary>
    public NFriendInfo Info;
    /// <summary>
    /// 设置好友信息
    /// </summary>
    /// <param name="item"></param>
    public void SetFirendInfo(NFriendInfo item)
    {
        this.Info = item;
        if (this.friendname != null) friendname.text = Info.friendInfo.Name;
        if (this.level != null) level.text = Info.friendInfo.Level.ToString();
        if (this.@class != null) @class.text = Info.friendInfo.Class.ToString();
        if (this.status != null) status.text = Info.Status == 1 ? "在线" : "离线";
    }
}
